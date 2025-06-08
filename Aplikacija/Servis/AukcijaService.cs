using Grupa5Tim3.Data;
using Grupa5Tim3.Models;
using Microsoft.EntityFrameworkCore;
using Grupa5Tim3.servis;

namespace Grupa5Tim3.servis
{
    public class AukcijaService
    {
        private readonly ApplicationDbContext _context;
        private readonly SendGridEmailSender _emailSender;

        public AukcijaService(ApplicationDbContext context, SendGridEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public async Task ZatvoriAukciju(int aukcijaId)
        {
            var aukcija = await _context.Aukcija
                .Include(a => a.Umjetnina)
                .FirstOrDefaultAsync(a => a.AukcijaID == aukcijaId);

            if (aukcija == null || aukcija.status == Status.Finalizirana)
                return;

            aukcija.status = Status.Finalizirana;
            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(aukcija.kupacID))
            {
                var kupac = await _context.Users.FindAsync(aukcija.kupacID);
                if (kupac != null && !string.IsNullOrEmpty(kupac.Email))
                {
                    string qrSadrzaj = "http://artevopay-001-site1.mtempurl.com";

                    byte[] qrBytes = QrCodeHelper.GenerateQrCode(qrSadrzaj);

                    string subject = "Artevo - Potvrda o kupovini umjetnine";
                    string htmlBody = $@"
                        <p>Poštovani {kupac.UserName},</p>
                        <p>Čestitamo! Osvojili ste aukciju za <strong>{aukcija.Umjetnina?.naziv}</strong> po cijeni od <strong>{aukcija.trenutnaCijena} KM</strong>.</p>
                        <p>QR kod u prilogu predstavlja potvrdu kupovine. Sačuvajte ga kao dokaz.</p>
                        <p>Hvala što koristite Artevo!</p>";

                    await _emailSender.SendQrCodeEmailAsync(kupac.Email, subject, htmlBody, qrBytes);
                }
            }
        }
    }
}

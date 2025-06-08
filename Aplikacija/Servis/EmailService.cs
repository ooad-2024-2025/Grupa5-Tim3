using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Grupa5Tim3.servis
{
    public class EmailService
    {
        public async Task PosaljiEmailSaQrKodAsync(string toEmail, byte[] qrCodeImage)
        {
            using var message = new MailMessage();
            message.To.Add(toEmail);
            message.Subject = "Potvrda o kupovini umjetnine";
            message.Body = "U prilogu se nalazi QR kod kao potvrda o uspješnoj aukciji.";
            message.IsBodyHtml = false;

            // Dodaj QR kod kao attachment
            message.Attachments.Add(new Attachment(new MemoryStream(qrCodeImage), "potvrda.png", "image/png"));

            using var smtp = new SmtpClient("smtp.yourserver.com") 
            {
                Port = 587,
                Credentials = new NetworkCredential("your_email@example.com", "your_password"),
                EnableSsl = true
            };

            message.From = new MailAddress("your_email@example.com");

            await smtp.SendMailAsync(message);
        }
    }
}

using Grupa5Tim3.Data;
using Grupa5Tim3.Models;
using Microsoft.EntityFrameworkCore;

namespace Grupa5Tim3.servis
{
    public class AukcijaService
    {
        private readonly ApplicationDbContext _context;

        public AukcijaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ZatvoriAukciju(int aukcijaId)
        {
            var aukcija = await _context.Aukcija.FindAsync(aukcijaId);
            if (aukcija == null || aukcija.status == Status.Finalizirana) return;

            aukcija.status = Status.Finalizirana;
            await _context.SaveChangesAsync();
        }
    }
}

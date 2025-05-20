using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grupa5Tim3.Models
{
    public class KorisnikAukcija
    {
        [Key] public int id {  get; set; }
        [ForeignKey("Korisnik")] public String korisnikID { get; set; }
        [ForeignKey("Aukcija")] public int aukcijaID { get; set; }


        public Korisnik korisnik { get; set; }
        public Aukcija aukcija { get; set; }

        public KorisnikAukcija() { }
    }
}

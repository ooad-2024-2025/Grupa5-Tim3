using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grupa5Tim3.Models
{
    public class KorisnikAukcija
    {
        [Key]
        private int id {  get; set; }
        [ForeignKey("Korisnik")]
        private int korisnikID { get; set; }
        [ForeignKey("Aukcija")]
        private int aukcijaID { get; set; }


        private Korisnik korisnik { get; set; }
        private Aukcija aukcija { get; set; }

        public KorisnikAukcija() { }
    }
}

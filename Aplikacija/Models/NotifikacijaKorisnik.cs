using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grupa5Tim3.Models
{
    public class NotifikacijaKorisnik
    {
        [Key] public int id {  get; set; }
        [ForeignKey("Notifikacija")] public int notifikacijaID { get; set; }
        [ForeignKey("Korisnik")] public String korisnikID { get; set; }


        public Notifikacija notifikacija { get; set; }
        public Korisnik korisnik { get; set; }

        public NotifikacijaKorisnik() { }
    }
}

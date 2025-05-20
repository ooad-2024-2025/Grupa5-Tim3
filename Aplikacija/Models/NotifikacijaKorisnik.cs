using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grupa5Tim3.Models
{
    public class NotifikacijaKorisnik
    {
        [Key]
        private int id {  get; set; }
        [ForeignKey("Notifikacija")]
        private int notifikacijaID { get; set; }
        [ForeignKey("Korisnik")]
        private int KorisnikID { get; set; }


        private Notifikacija notifikacija { get; set; }
        private Korisnik korisnik { get; set; }

        public NotifikacijaKorisnik() { }
    }
}

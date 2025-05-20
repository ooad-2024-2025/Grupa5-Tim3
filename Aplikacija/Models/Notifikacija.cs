using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Grupa5Tim3.Models
{
    public class Notifikacija
    {
        [Key]
        private int notifikacijaID {  get; set; }
        private Obavijest tipObavijesti { get; set; }
        private String kodQR { get; set; }
        private String verifikacijskiKod {  get; set; }
        private int umjetninaID { get; set; }
        private int korisnikID { get; set; }

        public Notifikacija() { }
    }
}

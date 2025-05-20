using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Grupa5Tim3.Models
{
    public class Notifikacija
    {
        [Key] public int notifikacijaID {  get; set; }
        public Obavijest tipObavijesti { get; set; }
        public String kodQR { get; set; }
        public String verifikacijskiKod {  get; set; }
        public int umjetninaID { get; set; }
        public int korisnikID { get; set; }

        public Notifikacija() { }
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;

namespace Grupa5Tim3.Models
{
    public class Notifikacija
    {
        [Key] public int notifikacijaID {  get; set; }
        public Obavijest tipObavijesti { get; set; }
        public string? kodQR { get; set; }
        public String verifikacijskiKod {  get; set; }
        public int umjetninaID { get; set; }
        public String korisnikID { get; set; }
    


    }
}

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Grupa5Tim3.Models
{
    public class Korisnik : IdentityUser
    {
        [Display(Name = "Ime")]
        [Required(ErrorMessage = "Obavezna vrijednost")]
        public String ime {  get; set; }
        [Display(Name = "Prezime")]
        [Required(ErrorMessage = "Obavezna vrijednost")]
        public String prezime { get; set; }

        [Display(Name = "Datum rodjenja")]
        public DateTime datumRodjenja { get; set; }

        public ICollection<KorisnikAukcija> KorisnikAukcije { get; set; }

    }
}

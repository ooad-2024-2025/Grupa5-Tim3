using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Grupa5Tim3.Models
{
    public class Korisnik : IdentityUser
    {
        [Key]
        private int korisnikID {  get; set; }
        private String korisnickoIme { get; set; }
        private String ime {  get; set; }
        private String prezime { get; set; }
        private DateTime datumRodjenja { get; set; }
        private String lozinka {  get; set; }
        private String email { get; set; }
        private Uloga uloga { get; set; }
        private bool verifikovan {  get; set; }
    }
}

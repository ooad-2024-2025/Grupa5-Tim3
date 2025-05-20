using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Grupa5Tim3.Models
{
    public class Korisnik : IdentityUser
    {
        public String korisnickoIme { get; set; }
        public String ime {  get; set; }
        public String prezime { get; set; }
        public DateTime datumRodjenja { get; set; }
        public String lozinka {  get; set; }
        public String email { get; set; }
        public Uloga uloga { get; set; }
        public bool verifikovan {  get; set; }
    }
}

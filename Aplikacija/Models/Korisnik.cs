using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Grupa5Tim3.Models
{
    public class Korisnik : IdentityUser
    {
        public String ime {  get; set; }
        public String prezime { get; set; }
        public DateTime datumRodjenja { get; set; }
    }
}

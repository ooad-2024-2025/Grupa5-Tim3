using System.ComponentModel.DataAnnotations;

namespace Grupa5Tim3.Models
{
    public class Umjetnina
    {
        [Key] public int umjetinaID {  get; set; }
        public String naziv {  get; set; }
        public String autor {  get; set; }
        public String period { get; set; }
        public DateTime datum { get; set; }
        public String tehnika { get; set; }
        public string? SlikaPath { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Početna cijena ne smije biti manja od 0.")]
        public double pocetnaCijena { get; set; }

        public Umjetnina() { }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Grupa5Tim3.Models
{
    public class Umjetnina
    {
        [Key] public int umjetinaID {  get; set; }

        [Display(Name = "Naziv")]
        [Required(ErrorMessage = "Obavezna vrijednost")]
        public String naziv {  get; set; }

        [Display(Name = "Autor")]
        [Required(ErrorMessage = "Obavezna vrijednost")]
        public String autor {  get; set; }

        [Display(Name = "Period")]
        [Required(ErrorMessage = "Obavezna vrijednost")]
        public String period { get; set; }

        [Display(Name = "Datum")]
        [Required(ErrorMessage = "Obavezna vrijednost")] 
        public DateTime datum { get; set; }

        [Display(Name = "Tehnika")]
        [Required(ErrorMessage = "Obavezna vrijednost")]
        public String tehnika { get; set; }
        public string? SlikaPath { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Početna cijena ne smije biti manja od 0.")]
        public double pocetnaCijena { get; set; }
        public String ? opis { get; set; }

        public Umjetnina() { }
    }
}

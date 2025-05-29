using System.ComponentModel.DataAnnotations;

namespace Grupa5Tim3.Models
{
    public enum Status
    {
        [Display(Name = "Aktivna aukcija")]
        Aktivna,
        [Display(Name = "Finalizirana aukcija")]
        Finalizirana,
        [Display(Name = "Otkazana aukcija")]
        Otkazana
    }
}

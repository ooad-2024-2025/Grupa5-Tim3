using System.ComponentModel.DataAnnotations;

namespace Grupa5Tim3.Models
{
    public class Umjetnina
    {
        [Key]
        private int umjetinaID {  get; set; }
        private String naziv {  get; set; }
        private String autor {  get; set; }
        private String period { get; set; }
        private DateTime datum { get; set; }
        private String tehnika { get; set; }
        private double pocetnaCijena { get; set; }

        public Umjetnina() { }
    }
}

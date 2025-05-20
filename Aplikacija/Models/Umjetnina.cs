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
        public double pocetnaCijena { get; set; }

        public Umjetnina() { }
    }
}

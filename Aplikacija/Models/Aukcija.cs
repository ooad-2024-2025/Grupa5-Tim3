using System.ComponentModel.DataAnnotations;

namespace Grupa5Tim3.Models
{
    public class Aukcija {
        [Key]
        private int AukcijaID { get; set; }
        private int umjetninaID { get; set; }
        private double trenutnaCijena { get; set; }
        private DateTime pocetakAukcije {  get; set; }
        private DateTime zavrsetakAukcije { get; set; }
        private Status status { get; set; }
        private int kupacID { get; set; }

        public Aukcija() { }
    }
}

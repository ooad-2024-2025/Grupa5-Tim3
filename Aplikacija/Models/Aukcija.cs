using System.ComponentModel.DataAnnotations;

namespace Grupa5Tim3.Models
{
    public class Aukcija {
        [Key] public int AukcijaID { get; set; }
        public int umjetninaID { get; set; }
        public double trenutnaCijena { get; set; }
        public DateTime pocetakAukcije {  get; set; }
        public DateTime zavrsetakAukcije { get; set; }
        [EnumDataType(typeof(Status))]    public Status status { get; set; }
        public String ?kupacID { get; set; }
        public Aukcija() { }
    }
}

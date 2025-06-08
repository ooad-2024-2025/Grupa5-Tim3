using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [ForeignKey("umjetninaID")]
        public virtual Umjetnina? Umjetnina { get; set; }

        public ICollection<KorisnikAukcija> KorisnikAukcije { get; set; }


        public Aukcija() { }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grupa5Tim3.Models
{
    public class NotifikacijaUmjetnina
    {
        [Key] public int id { get; set; }
        [ForeignKey("Notifikacija")] public int notifikacijaID {  get; set; }
        [ForeignKey("Umjetnina")] public int umjetninaID { get; set; }


        public Notifikacija notifikacija {  get; set; }
        public Umjetnina umjetnina { get; set; }

        public NotifikacijaUmjetnina() { }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grupa5Tim3.Models
{
    public class NotifikacijaUmjetnina
    {
        [Key]
        private int id;
        [ForeignKey("Notifikacija")]
        private int notifikacijaID {  get; set; }
        [ForeignKey("Umjetnina")]
        private int umjetninaID { get; set; }


        private Notifikacija notifikacija {  get; set; }
        private Umjetnina umjetnina { get; set; }

        public NotifikacijaUmjetnina() { }
    }
}

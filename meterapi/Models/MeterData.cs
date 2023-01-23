using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace meterapi.Models
{
    public class MeterData
    {

        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("Meter")]
        public int MeterId { get; set; }

        public float TotalConsumptionDay { get; set; }

        public float TotalConsumptionNight { get; set; }

        public float AllPhaseConsumption { get; set; }

        public float GasConsumption { get; set; }
        public virtual Meter Meter { get; set; }


    }
}

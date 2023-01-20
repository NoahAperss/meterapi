using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace meterapi.Data.Mappers
{
    public class MeterDataDTO
    {
       
        public DateTime Date { get; set; }

        [ForeignKey("Meter")]
        public int MeterId { get; set; }

        public float TotalConsumptionDay { get; set; }

        public float TotalConsumptionNight { get; set; }

        public float AllPhaseConsumption { get; set; }

        public float GasConsumption { get; set; }
        
    }
}

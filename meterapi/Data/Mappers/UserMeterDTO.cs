using meterapi.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace meterapi.Data.Mappers
{
    public class UserMeterDTO
    {
       
        [ForeignKey("Meter")]
        public int MeterId { get; set; }

        public int RpId { get; set; }
        public int MeterAId { get; set; }




        [ForeignKey("User")]
        public int UserId { get; set; }
        public string Address { get; set; }

       
    }
}

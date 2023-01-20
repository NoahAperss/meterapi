using meterapi.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace meterapi.Data.Mappers
{
    public class UserMeterDTO
    {
       
        [ForeignKey("Meter")]
        public int MeterId { get; set; }

        public string RpId { get; set; }
        public string MeterDeviceId { get; set; }




        [ForeignKey("User")]
        public int UserId { get; set; }
        public string Address { get; set; }

       
    }
}

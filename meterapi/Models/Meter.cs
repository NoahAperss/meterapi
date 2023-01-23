using Microsoft.Xrm.Sdk.Metadata;
using System.ComponentModel.DataAnnotations;

namespace meterapi.Models
{
    public class Meter
    {
        [Key]
        public int Id { get; set; }

       
        public string MeterDeviceId { get; set; }

    
        public string RpId { get; set; }

        public virtual ICollection<MeterData> MeterDatas { get; set; }
        public virtual ICollection<UserMeter> UserMeter { get; set; }


    }
}

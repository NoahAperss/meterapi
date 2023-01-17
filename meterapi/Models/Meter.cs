using Microsoft.Xrm.Sdk.Metadata;
using System.ComponentModel.DataAnnotations;

namespace meterapi.Models
{
    public class Meter
    {
        [Key]
        public int Id { get; set; }

       
        public int MeterId { get; set; }

    
        public int RpId { get; set; }

        public DateTime Date { get; set; }

        public int measurement { get; set; }

        public virtual ICollection<UserMeter> UserMeters { get; set; }


    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace meterapi.Models
{
    public class UserMeter
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Meter")]
        public int MeterId { get; set; }
        
        public int RpId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public string Address { get; set; }
        
        public virtual Meter Meter { get; set; }

        public virtual User User { get; set; }
    }
}

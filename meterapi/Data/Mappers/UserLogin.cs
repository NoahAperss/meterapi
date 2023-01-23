using System.ComponentModel.DataAnnotations;

namespace meterapi.Data.Mappers
{
    public class UserLogin
    {
        [Key]
        public int Id { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

using meterapi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace meterapi.Data.Mappers
{
    public class UserDTO
    {
        [Key]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = String.Empty;

        public UserDTO(User x)
        {
            Id = x.Id;
            FirstName = x.FirstName;
            LastName = x.LastName;
            Email = x.Email;

        }
    }
}

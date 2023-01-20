using meterapi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace meterapi.Data.Mappers
{
    public class UserDTO
    {
        
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = String.Empty;

        public UserDTO(User x)
        {
            LastName = x.LastName;
            FirstName = x.FirstName;
            Email = x.Email;

        }
    }
}

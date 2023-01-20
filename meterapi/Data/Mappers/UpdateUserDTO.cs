using meterapi.Models;
using System.ComponentModel.DataAnnotations;

namespace meterapi.Data.Mappers
{
    public class UpdateUserDTO
    {
        
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = String.Empty;

        public string Password { get; set; } = string.Empty;




    }
}

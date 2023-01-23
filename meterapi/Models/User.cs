using System.ComponentModel.DataAnnotations;

namespace meterapi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = String.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }

        public virtual ICollection<UserMeter> UserMeters { get; set; }
        public string? VerificationToken { get; internal set; }
           public bool IsEmailVerified { get; set; } = false;
        public string? ResetPasswordToken { get; internal set; }
        public DateTime? ResetPasswordExpiration { get; internal set; }
    }
}

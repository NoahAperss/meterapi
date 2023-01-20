using Microsoft.AspNetCore.Mvc;
using meterapi.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using meterapi.Data;
using Microsoft.Xrm.Sdk;
using MimeKit;
using MailKit.Net.Smtp;
using System.Security.Cryptography;

namespace meterapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly DataContext _context;

        public EmailController(DataContext context)
        {
            _context = context;
        }


        [HttpPost("verify/{token}")]
        public async Task<IActionResult> Verify(string token)
        {
            // Find the user with the matching token
            var user = await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);
            if (user == null)
            {
                return NotFound("Invalid token");
            }
            if (user.IsEmailVerified)
            {
                return BadRequest("Email already verified");
            }

            // Set the user's email as verified
            user.IsEmailVerified = true;
            await _context.SaveChangesAsync();

            return Ok("Email verified");
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyEmail(string email)
        {
            // Verify the email address
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return NotFound("User not found");
            }
            if (user.IsEmailVerified)
            {
                return BadRequest("Email already verified");
            }

            // Generate a verification token
            var token = Guid.NewGuid().ToString();


            // Send verification email
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Elek3City Support", "elek3citysupport@pjotrb.be"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Verify Email";
            message.Body = new TextPart("plain") { Text = $"Please click on the link to verify your email: https://meterapiproject4.azurewebsites.net/api/email/verify/{token}" };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp-auth.mailprotect.be", 587, false);
                client.Authenticate("elek3citysupport@pjotrb.be", "GS1A05l918UX4K4lUK4O");
                client.Send(message);
                client.Disconnect(true);
            }

            // Save the token in the database
            user.VerificationToken = token;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            // Check if the user exists
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Generate a password reset token
            var token = Guid.NewGuid().ToString();

            // Save the token to the user's record
            user.ResetPasswordToken = token;
            user.ResetPasswordExpiration = DateTime.Now.AddMinutes(30);
            await _context.SaveChangesAsync();

            // Send the reset password email
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Elek3City Support", "elek3citysupport@pjotrb.be"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Reset Password";
            message.Body = new TextPart("plain") { Text = $"Please click on the link to reset your password: https://meterapiproject4.azurewebsites.net/resetpassword/{token}" };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp-auth.mailprotect.be", 587, false);
                client.Authenticate("elek3citysupport@pjotrb.be", "GS1A05l918UX4K4lUK4O");
                client.Send(message);
                client.Disconnect(true);
            }

            return Ok();
        }

        [HttpPost("resetpassword/{token}")]
        public async Task<IActionResult> ResetPassword(string token, string newPassword)
        {
            // Check if the token is valid
            var user = await _context.Users.SingleOrDefaultAsync(u => u.ResetPasswordToken == token && u.ResetPasswordExpiration > DateTime.Now);
            if (user == null)
            {
                return BadRequest("Invalid token or token has expired.");
            }
            // Hash the new password
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);

            // Update the user's password
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.ResetPasswordToken = null;
            user.ResetPasswordExpiration = null;
            await _context.SaveChangesAsync();

            return Ok();

        }










            private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // Create a new instance of the HMACSHA512 algorithm
            using (var hmac = new HMACSHA512())
            {
                // Assign the generated key to the passwordSalt output parameter
                passwordSalt = hmac.Key;
                // Compute the hash of the plaintext password and assign it to the passwordHash output parameter
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }


    }
}

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using meterapi.Data;
using meterapi.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Cryptography;
using meterapi.Data.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using System.Text;

using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;


// This is the UserController class that handles HTTP requests for the "User" resource
namespace meterapi.Controllers
{


    [Authorize]
    [Route("api/[controller]")]
    [ApiController]




    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;


        public UserController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/User
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _context.Users.Include(u => u.UserMeters);
            return Ok(users);
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _context.Users.Include(u => u.UserMeters).SingleOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDTO request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            if (_context.Users.Any(x => x.Email == request.Email && x.Id != id))
            {
                return BadRequest("A user with this email already exists.");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }






        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return NoContent();
        }


        [AllowAnonymous]
        [HttpPost]
        // This action handles POST requests to the "api/User" route
        // It creates a new user based on the information in the request body (UserAuthDTO)

        public async Task<ActionResult<User>> Register(UserAuthDTO request)
        {
            if ((_context.Users?.Any(x => x.Email == request.Email)).GetValueOrDefault())
            {
                return BadRequest("User with this email already exists.");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User();
            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            await VerifyEmail(user.Email);


            return Ok(user);
        }




        [HttpGet("verify/{token}")]
        [AllowAnonymous]

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
        [AllowAnonymous]
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
            message.Body = new TextPart("plain") { Text = $"Please click on the link to verify your email: localhost:3000/verify?token={token}" };

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



        [HttpGet("verifyEmailChangeToken/{token}")]
        [AllowAnonymous]

        public async Task<IActionResult> VerifyEmailChangeToken(string token, string newMail)
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
            user.Email = newMail;
            await _context.SaveChangesAsync();

            return Ok("Email verified");
        }



        [HttpPost("verifyEmailChange")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmailChange(string email, string newMail)
        {
            // Verify the email address
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var user1 = await _context.Users.FirstOrDefaultAsync(u => u.Email == newMail);
            if (user1 != null)
            {
                return NotFound("Email already being used");
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
            message.To.Add(new MailboxAddress("", newMail));
            message.Subject = "Verify Email";
            message.Body = new TextPart("plain") { Text = $"Please click on the link to verify your email: localhost:3000/verifyEmailChangeToken?token={token}&email={newMail}" };

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



        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login(UserLogin request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            if (!user.IsEmailVerified)
            {
                return BadRequest("Email not verified.");
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }

            string token = CreateToken(user);

            var userDto = new UserDTO(user);

            return Ok(new { token, userDto });
        }


        [AllowAnonymous]
        [HttpPut("changeemail")]
        public async Task<IActionResult> ChangeEmail(ChangeEmailDTO request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.OldEmail);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Verify that the new email is not already in use
            if (_context.Users.Any(x => x.Email == request.NewEmail))
            {
                return BadRequest("A user with this email already exists.");
            }

            // Verify that the provided password matches the user's current password
            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Invalid password");
            }

            // Update the user's email
           
            user.IsEmailVerified = false;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            await VerifyEmailChange(request.OldEmail, request.NewEmail);

            return Ok();
        }
        [AllowAnonymous]
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
            message.Body = new TextPart("plain") { Text = $"Please click on the link to reset your password: localhost:3000/resetpassword?token={token}" };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp-auth.mailprotect.be", 587, false);
                client.Authenticate("elek3citysupport@pjotrb.be", "GS1A05l918UX4K4lUK4O");
                client.Send(message);
                client.Disconnect(true);
            }

            return Ok();
        }
        [AllowAnonymous]
        [HttpPost("resetpassword/{token}")]
        public async Task<IActionResult> ResetPassword(string token, newPasswordDTO newPassword)
        {
            // Check if the token is valid
            var user = await _context.Users.SingleOrDefaultAsync(u => u.ResetPasswordToken == token && u.ResetPasswordExpiration > DateTime.Now);
            if (user == null)
            {
                return BadRequest("Invalid token or token has expired.");
            }
            // Hash the new password
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(newPassword.newPassword, out passwordHash, out passwordSalt);
            // Update the user's password
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.ResetPasswordToken = null;
            user.ResetPasswordExpiration = null;
            await _context.SaveChangesAsync();

            return Ok();
        }



        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO passwords)
        {
            //Retrieve the user from the database using the email provided in the ChangePasswordDTO
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == passwords.email);
            //If the user does not exist, return a bad request
            if (user == null)
            {
                return BadRequest("Invalid user");
            }

            //Verify that the old password provided matches the password on file
            if (!VerifyPasswordHash(passwords.oldPassword, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Invalid password");
            }

            //Create new password hash and salt for the new password provided
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(passwords.newPassword, out passwordHash, out passwordSalt);

            //Update the user's password hash and salt in the database
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            //Save changes to the database
            await _context.SaveChangesAsync();

            //Return an OK status
            return Ok();
        }







        //This function creates a JSON Web Token (JWT) to authenticate the user
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
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

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            // Create a new instance of the HMACSHA512 algorithm using the provided salt
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                // Compute the hash of the plaintext password
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                // Compare the computed hash to the provided password hash
                return computedHash.SequenceEqual(passwordHash);
            }
        }

    }
}
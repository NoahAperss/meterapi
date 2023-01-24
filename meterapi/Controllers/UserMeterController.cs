using System.Linq;
using Microsoft.AspNetCore.Mvc;
using meterapi.Data;
using meterapi.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.EntityFrameworkCore;
using meterapi.Data.Mappers;
using Microsoft.AspNetCore.Authorization;

namespace meterapi.Controllers
{

    [Authorize] 
    [Route("api/[controller]")]
    [ApiController]


    public class UserMeterController : ControllerBase
    {
        private readonly DataContext _context;
      

        public UserMeterController(DataContext context)
        {
            _context = context;
          
        }

        // GET: api/UserMeter
        [HttpGet]
        public IActionResult GetUserMeters()
        {
            return Ok(_context.UserMeters);
        }

        // GET: api/UserMeter/5
        [HttpGet("{id}")]
        public IActionResult GetUserMeter(int id)
        {
            var userMeter = _context.UserMeters.Find(id);

            if (userMeter == null)
            {
                return NotFound();
            }

            return Ok(userMeter);
        }

        [HttpPut("{id}")]
        public IActionResult PutUserMeter(int id, UserMeterDTO userMeterDTO)
        {
            // Find the UserMeter object with the matching id
            var userMeter = _context.UserMeters
                        .Include(u => u.Meter)
                        .Include(u => u.User)
                        .FirstOrDefault(u => u.Id == id);

            // Return a 404 error if no matching UserMeter object was found
            if (userMeter == null)
            {
                return NotFound("UserMeter with id " + id + " not found.");
            }

            // Find the Meter object with the matching id
            var meter = _context.Meters.FirstOrDefault(m => m.Id == userMeterDTO.MeterId);
            if (meter == null)
            {
                return NotFound("Meter with id " + userMeterDTO.MeterId + " not found.");
            }

            // Find the User object with the matching id
            var user = _context.Users.FirstOrDefault(u => u.Id == userMeterDTO.UserId);
            if (user == null)
            {
                return NotFound("User with id " + userMeterDTO.UserId + " not found.");
            }

            // Update the properties of the UserMeter object
            userMeter.RpId = meter.RpId;
            userMeter.MeterDeviceId = meter.MeterDeviceId;
            userMeter.UserId = userMeterDTO.UserId;
            userMeter.Address = userMeterDTO.Address;

            // Save the changes to the database
            _context.UserMeters.Update(userMeter);
            _context.SaveChanges();

            return NoContent();
        }




        // POST: api/UserMeter
        [HttpPost]
        public IActionResult PostUserMeter(UserMeterDTO userMeterDTO)
        {
            var meter = _context.Meters.Find(userMeterDTO.MeterId);
            if (meter == null)
            {
                return NotFound();
            }

            var userMeter = new UserMeter
            {
                MeterId = userMeterDTO.MeterId,
                RpId = meter.RpId,
                MeterDeviceId = meter.MeterDeviceId,
                UserId = userMeterDTO.UserId,
                Address = userMeterDTO.Address
            };

            _context.UserMeters.Add(userMeter);
            _context.SaveChanges();

            return CreatedAtAction("GetUserMeter", new { id = userMeter.Id }, userMeter);

        }





      

            // DELETE: api/UserMeter/5
            [HttpDelete("{id}")]
        public IActionResult DeleteUserMeter(int id)
        {
            var userMeter = _context.UserMeters.Find(id);
            if (userMeter == null)
            {
                return NotFound();
            }

            _context.UserMeters.Remove(userMeter);
            _context.SaveChanges();

            return NoContent();
        }
    }
}

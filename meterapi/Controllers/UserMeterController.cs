using System.Linq;
using Microsoft.AspNetCore.Mvc;
using meterapi.Data;
using meterapi.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.EntityFrameworkCore;

namespace meterapi.Controllers
{
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

        // POST: api/UserMeter
        [HttpPost]
        public IActionResult PostUserMeter([FromBody] UserMeter userMeter)
        {
            _context.UserMeters.Add(userMeter);
            _context.SaveChanges();

            return CreatedAtAction("GetUserMeter", new { id = userMeter.Id }, userMeter);
        }

        // PUT: api/UserMeter/5
        [HttpPut("{id}")]
        public IActionResult PutUserMeter(int id, [FromBody] UserMeter userMeter)
        {
            if (id != userMeter.Id)
            {
                return BadRequest();
            }

            _context.Entry(userMeter).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
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

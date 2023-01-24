using System.Linq;
using Microsoft.AspNetCore.Mvc;
using meterapi.Data;
using meterapi.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.EntityFrameworkCore;
using meterapi.Data.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace meterapi.Controllers

// This is the MeterController class that handles HTTP requests for the "Meter" resource
{
    // The Route attribute specifies the route template for this controller

    [Authorize]
    [Route("api/[controller]")]
 



    // The ApiController attribute enables model validation and automatic HTTP 400 responses
    [ApiController]
    public class MeterController : ControllerBase
    {
        // The DataContext is injected into the constructor and used to access the database
        private readonly DataContext _context;

        public MeterController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Meter
        [HttpGet]
        // This action handles GET requests to the "api/Meter" route
        // It retrieves all meters from the database and return them as an OK response
        public IActionResult GetMeters()
        {
            var meters = _context.Meters;
            return Ok(meters);
        }

        // GET: api/Meter/5
        // This action handles GET requests to the "api/Meter/{id}" route
        // It retrieves the meter with the specified ID, including its related data
        // If the meter is not found, it returns a Not Found response
        [HttpGet("{id}")]
        public IActionResult GetMeter(int id)
        {
            var meter = _context.Meters.Include(m => m.MeterDatas).Include(m => m.UserMeter).SingleOrDefault(m => m.Id == id);
            if (meter == null)
            {
                return NotFound();
            }
            return Ok(meter);
        }

        // PUT: api/Meter/5
        // This action handles PUT requests to the "api/Meter/{id}" route
        // It updates the existing meter with the new data, and returns a No Content response indicating the update was successful
        [HttpPut("{id}")]
        public IActionResult PutMeter(int id, NewMeterViewModel meter)
        {
            var existingMeter = _context.Meters.Find(id);
            if (existingMeter == null)
            {
                return NotFound();
            }

            existingMeter.MeterDeviceId = meter.MeterDeviceId;
            existingMeter.RpId = meter.RpId;
            _context.Meters.Update(existingMeter);
            _context.SaveChanges();
            return NoContent();
        }

        // POST: api/Meter
        // This action handles POST requests to the "api/Meter" route
        // It creates a new meter with the provided data, and returns a CreatedAtAction response with the new meter's data and location     
        [HttpPost]
        public IActionResult PostMeter(NewMeterViewModel newMeter)
        {
            if (string.IsNullOrEmpty(newMeter.MeterDeviceId) || string.IsNullOrEmpty(newMeter.RpId))
            {
                return BadRequest();
            }
            var meter = new Meter { MeterDeviceId = newMeter.MeterDeviceId, RpId = newMeter.RpId };
            _context.Meters.Add(meter);
            _context.SaveChanges();
            return CreatedAtAction("GetMeter", new { id = meter.Id }, meter);
        }


        // DELETE: api/Meter/5
        // This action handles DELETE requests to the "api/Meter/{id}" route
        // It deletes the meter with the specified ID and returns a No Content response indicating the delete was successful 
        [HttpDelete("{id}")]
        public IActionResult DeleteMeter(int id)
        {
            var meter = _context.Meters.SingleOrDefault(m => m.Id == id);
            if (meter == null)
            {
                return NotFound();
            }

            _context.Meters.Remove(meter);
            _context.SaveChanges();
            return NoContent();
        }
    }

}


using System.Linq;
using Microsoft.AspNetCore.Mvc;
using meterapi.Data;
using meterapi.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.EntityFrameworkCore;
using meterapi.Data.Mappers;

namespace meterapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeterController : ControllerBase
    {
        private readonly DataContext _context;

        public MeterController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Meter
        [HttpGet]
        public IActionResult GetMeters()
        {
            var meters = _context.Meters;
            return Ok(meters);
        }

        // GET: api/Meter/5
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


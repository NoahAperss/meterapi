﻿using System.Linq;
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
            var meters = _context.Meters.Include(m => m.MeterDatas).Include(m => m.UserMeter);
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

            existingMeter.MeterAId = meter.MeterAId;
            existingMeter.RpId = meter.RpId;
            _context.Meters.Update(existingMeter);
            _context.SaveChanges();
            return NoContent();
        }
        // POST: api/Meter
        [HttpPost]
        public IActionResult PostMeter(NewMeterViewModel newMeter)
        {
            if (newMeter.MeterAId <= 0 || newMeter.RpId <= 0)
            {
                return BadRequest();
            }
            var meter = new Meter { MeterAId = newMeter.MeterAId, RpId = newMeter.RpId };
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

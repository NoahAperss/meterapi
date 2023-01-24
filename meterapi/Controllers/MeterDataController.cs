using meterapi.Data;
using meterapi.Data.Mappers;
using meterapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


// This is the MeterDataController class that handles HTTP requests for the "MeterData" resource
namespace meterapi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]



    public class MeterDataController : ControllerBase
    {
        private readonly DataContext _context;
        public MeterDataController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        // This action handles GET requests to the "api/MeterData" route
        // It retrieves all meter data from the database, including the related meter, and returns it as a List<MeterData>
        public ActionResult<List<MeterData>> GetAll()
        {
            return _context.MeterDatas.ToList();
        }

        // This action handles GET requests to the "api/MeterData/{id}" route
        // It retrieves the meter data with the specified ID, including the related meter, and returns it as a MeterData
        // If the meter data is not found, it returns a Not Found response
        [HttpGet("{id}")]

        public ActionResult<MeterData> GetById(int id)
        {
            var meterData = _context.MeterDatas.Include(md => md.Meter).FirstOrDefault(md => md.Id == id);
            if (meterData == null)
            {
                return NotFound();
            }
            return meterData;
        }



        //POST: api/MeterData
        // This action handles POST requests to the "api/MeterData" route
        // It creates a new meter data with the provided data and returns a CreatedAtAction response with the new meter data's location and data
        [HttpPost]
        public ActionResult<MeterData> Create(MeterDataDTO meterDataDTO)
        {
            if (meterDataDTO.MeterId <= 0 || meterDataDTO.Date == null)
            {
                return BadRequest();
            }
            var meterData = new MeterData
            {
                Date = meterDataDTO.Date,
                MeterId = meterDataDTO.MeterId,
                TotalConsumptionDay = meterDataDTO.TotalConsumptionDay,
                TotalConsumptionNight = meterDataDTO.TotalConsumptionNight,
                AllPhaseConsumption = meterDataDTO.AllPhaseConsumption,
                GasConsumption = meterDataDTO.GasConsumption
            };
            _context.MeterDatas.Add(meterData);
            _context.SaveChanges();

            return CreatedAtAction("GetById", new { id = meterData.Id }, meterData);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, MeterDataDTO meterDataDTO)
        {
            var meterData = _context.MeterDatas.Find(id);
            if (meterData == null)
            {
                return NotFound();
            }
            meterData.Date = meterDataDTO.Date;
            
           meterData.TotalConsumptionDay = meterDataDTO.TotalConsumptionDay;
            meterData.TotalConsumptionNight = meterDataDTO.TotalConsumptionNight;
            meterData.AllPhaseConsumption = meterDataDTO.AllPhaseConsumption;
            meterData.GasConsumption = meterDataDTO.GasConsumption;

            _context.SaveChanges();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public ActionResult<MeterData> Delete(int id)
        {
            var meterData = _context.MeterDatas.Find(id);
            if (meterData == null)
            {
                return NotFound();
            }

            _context.MeterDatas.Remove(meterData);
            _context.SaveChanges();

            return meterData;
        }

    }

}
   
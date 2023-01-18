using meterapi.Data;
using meterapi.Data.Mappers;
using meterapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace meterapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeterDataController : ControllerBase
    {
        private readonly DataContext _context;
        public MeterDataController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<MeterData>> GetAll()
        {
            return _context.MeterDatas.Include(md => md.Meter).ToList();
        }

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
   
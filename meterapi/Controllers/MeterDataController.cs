using meterapi.Data;
using meterapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Xrm.Sdk;

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

        [HttpPost]
        public ActionResult<MeterData> Create(MeterData meterData)
        {
            _context.MeterDatas.Add(meterData);
            _context.SaveChanges();

            return CreatedAtAction("GetById", new { id = meterData.Id }, meterData);
        }

       /* [HttpPut("{id}")]
        public ActionResult Update(int id, MeterData meterData)
        {
            if (id != meterData.Id)
            {
                return BadRequest();
            }

            _context.Entry(meterData).State = Entity.EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }*/

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
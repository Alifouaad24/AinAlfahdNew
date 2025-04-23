using AinAlfahd.Data;
using AinAlfahd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmendmentLogsController : ControllerBase
    {
        private readonly MasterDBContext dBContext;
        public AmendmentLogsController(MasterDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAmendmentLogs()
        {
            var amendmentLogss = await dBContext.Amendment_Logs.Include(a => a.Service).Include(a => a.Customer).Include(a => a.Amendments).ToListAsync();
            return Ok(amendmentLogss);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAmendmentLogById(int id)
        {
            var amendmentLog = await dBContext.Amendment_Logs.Include(a => a.Service).Include(a => a.Customer).Include(a => a.Amendments)
                .Where(a => a.Amendment_LogId == id)
                .FirstOrDefaultAsync();
            return Ok(amendmentLog);
        }


        [HttpPost]
        public async Task<IActionResult> AddAmendment([FromBody] Amendment_Log model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await dBContext.Amendment_Logs.AddAsync(model);
            await dBContext.SaveChangesAsync();

            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditAmendment(int id, [FromBody] Amendment_Log model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.Amendment_LogId = id;
            dBContext.Entry(model).State = EntityState.Modified;
            await dBContext.SaveChangesAsync();

            return Ok(model);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLog(int id)
        {
            var log = await dBContext.Amendment_Logs.FindAsync(id);
            if (log == null) return BadRequest();

            dBContext.Amendment_Logs.Remove(log);
            await dBContext.SaveChangesAsync();

            return Ok(new
            {
                msg = "Deleted successfuly"
            });
        }

    }
}

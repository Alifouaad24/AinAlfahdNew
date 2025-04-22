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
            var amendmentLogss = await dBContext.Amendment_Logs.ToListAsync();
            return Ok(amendmentLogss);
        }

        [HttpPost]
        public async Task<IActionResult> AddAmendment([FromBody] Amendment_Log model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await dBContext.Amendment_Logs.AddAsync(model);
            await dBContext.SaveChangesAsync();

            return Ok(model);
        }
    }
}

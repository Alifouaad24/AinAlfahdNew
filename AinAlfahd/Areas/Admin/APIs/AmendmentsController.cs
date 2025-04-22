using AinAlfahd.Data;
using AinAlfahd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmendmentsController : ControllerBase
    {
        private readonly MasterDBContext dBContext;
        public AmendmentsController(MasterDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAmendments()
        {
            var amendments = await dBContext.Amendments.ToListAsync();
            return Ok(amendments);
        }

        [HttpPost]
        public async Task<IActionResult> AddAmendment([FromBody] Amendment model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await dBContext.Amendments.AddAsync(model);
            await dBContext.SaveChangesAsync();

            return Ok(model);
        }

    }
}

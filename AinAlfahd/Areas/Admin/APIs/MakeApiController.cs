using AinAlfahd.Data;
using AinAlfahd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class MakeApiController : ControllerBase
    {
        MasterDBContext dBContext;
        public MakeApiController(MasterDBContext dBContext = null)
        {
            this.dBContext = dBContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMakes()
        {
            var makes = await dBContext.Makes.ToListAsync();
            return Ok(makes);
        }

        [HttpPost]
        public async Task<IActionResult> AddMake([FromBody] Make model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var make = new Make
            {
                MakeDescription = model.MakeDescription,
            };

            await dBContext.Makes.AddAsync(make);
            await dBContext.SaveChangesAsync();
            return Ok(make);
        }
    }
}

using AinAlfahd.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        MasterDBContext dBContext;
        public PlatformsController(MasterDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlatforms()
        {
            var plats = await dBContext.Platforms.ToListAsync();
            return Ok(plats);
        }
    }
}

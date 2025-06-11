using AinAlfahd.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        MasterDBContext dBContext;
        public SystemController(MasterDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSystems()
        {
            var sys = await dBContext.Systems.ToListAsync();
            return Ok(sys);
        }
    }
}

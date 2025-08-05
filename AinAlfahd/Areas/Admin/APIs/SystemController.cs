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

        [HttpGet("GetAllSystemsForApx")]
        public async Task<IActionResult> GetAllSystemsForApx()
        {
            var sys = await dBContext.Systems.Where(s => s.is_active == true).ToListAsync();
            return Ok(sys);
        }

        [HttpGet("GetAllSystemsForAinAlFhd")]
        public async Task<IActionResult> GetAllSystemsForAinAlFhd()
        {
            var sys = await dBContext.Systems.Where(s => s.sub_category_id == 2).ToListAsync();
            return Ok(sys);
        }

        [HttpGet("GetAllSystemsByMainSys/{id}")]
        public async Task<IActionResult> GetAllSystemsByMainSys(int id)
        {
            var sys = await dBContext.Systems.Where(s => s.sub_category_id == id).ToListAsync();
            return Ok(sys);
        }


    }
}

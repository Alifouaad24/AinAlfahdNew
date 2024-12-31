using AinAlfahd.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnviromentController : ControllerBase
    {
        MasterDBContext dBContext;
        public EnviromentController(MasterDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetEnv()
        {
            var env = await dBContext.TblConfigs.FindAsync(2);
            return Ok(env);
        }

        [HttpGet("GetExChg")]
        public async Task<IActionResult> GetExChg()
        {
            var ex = await dBContext.Exchanges.FirstOrDefaultAsync();
            return Ok(ex);
        }
    }
}

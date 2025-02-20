using AinAlfahd.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantAPIController : ControllerBase
    {
        MasterDBContext dBContext;
        public MerchantAPIController(MasterDBContext dBContext = null)
        {
            this.dBContext = dBContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMerchants()
        {
            var merchants = await dBContext.Merchants.Where(m => m.Sorting != null).ToListAsync();
            return Ok(merchants);
        }
    }
}

using AinAlfahd.Data;
using AinAlfahd.Models;
using AinAlfahd.Models_New;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemCondetionsController : ControllerBase
    {
        MasterDBContext dBContext;
        public ItemCondetionsController(MasterDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCondetions()
        {
            var conds = await dBContext.Set<ItemCondition>().ToListAsync();
            return Ok(conds);
        }
    }
}

using AinAlfahd.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizesAPIController : ControllerBase
    {
        MasterDBContext dBContext;
        public SizesAPIController(MasterDBContext dBContext = null)
        {
            this.dBContext = dBContext;
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetSisesByCategory(int categoryId)
        {
            var sizes = await dBContext.TblSizes.Where(s => s.CategoryId == categoryId).ToListAsync();
            return Ok(sizes);
        }
    }
}

using AinAlfahd.Data;
using AinAlfahd.Models;
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

        [HttpGet]
        public async Task<IActionResult> GetSises()
        {
            var sizes = await dBContext.TblSizes.ToListAsync();
            return Ok(sizes);
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetSisesByCategory(int categoryId)
        {
            List<TblSize> sizes;
            if (categoryId == 1 || categoryId == 4)
            {
                sizes = await dBContext.TblSizes.Where(s => s.GroupIndex == categoryId).ToListAsync();
                return Ok(sizes);
            }

            sizes = await dBContext.TblSizes.Where(s => s.CategoryId == categoryId).ToListAsync();
            return Ok(sizes);
        }
    }
}

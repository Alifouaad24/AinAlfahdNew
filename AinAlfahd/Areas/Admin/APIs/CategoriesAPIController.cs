using AinAlfahd.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesAPIController : ControllerBase
    {
        MasterDBContext dBContext;
        public CategoriesAPIController(MasterDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var cateories = await dBContext.Categories.ToListAsync();
            return Ok(cateories);
        }

        [HttpGet("GetSubCategoriesForDetectedCategory/{id}")]
        public async Task<IActionResult> GetSubCategoriesForDetectedCategory(int id)
        {
            var cateories = await dBContext.Categories.Where(c => c.MainCategoryId == id && c.MainCategoryId != null).ToListAsync();
            return Ok(cateories);
        }
    }
}

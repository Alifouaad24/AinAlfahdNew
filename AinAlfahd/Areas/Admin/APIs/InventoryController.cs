using AinAlfahd.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        MasterDBContext dBContext;
        public InventoryController(MasterDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductsInInventory()
        {
            var inventory = await dBContext.Inventory
                .Include(i => i.Systemm)
                .Include(i => i.ItemCondetion)
                .Include(i => i.Size)
                .Include(i => i.Merchant)
                .Include(i => i.Item)
                    .ThenInclude(i => i.ItemImages)
                .Include(i => i.Item)
                    .ThenInclude(i => i.Make)
                .Include(i => i.Item)
                    .ThenInclude(i => i.Make)
                 .Include(i => i.Item)
                    .ThenInclude(i => i.Platform)
                 .Include(i => i.Item)
                    .ThenInclude(i => i.Category)
                .ToListAsync();

            return Ok(inventory);
        }

        [HttpGet("DownloadImage")]
        public async Task<IActionResult> DownloadImage(string url)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return NotFound("Failed to load image.");

            var imageBytes = await response.Content.ReadAsByteArrayAsync();
            var contentType = response.Content.Headers.ContentType?.ToString() ?? "image/jpeg";

            return File(imageBytes, contentType, "downloaded-image.jpg");
        }

    }
}

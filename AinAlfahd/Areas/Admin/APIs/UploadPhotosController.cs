using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadPhotosController : ControllerBase
    {
        public UploadPhotosController()
        {
            
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto([FromForm] UploadPhotoRequest request)
        {
            var photo = request.Photo;

            if (photo == null || photo.Length == 0)
            {
                return BadRequest("لم يتم تحميل صورة.");
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            var relativePath = Path.Combine("uploads", uniqueFileName).Replace("\\", "/");
            var fullUrl = $"{Request.Scheme}://{Request.Host}/{relativePath}";

            return Ok(new { fileName = uniqueFileName, url = fullUrl, relativePath });
        }



    }

    public class UploadPhotoRequest
    {
        [FromForm]
        public IFormFile Photo { get; set; }
    }
}

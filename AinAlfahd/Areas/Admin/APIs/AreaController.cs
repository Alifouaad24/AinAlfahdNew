using AinAlfahd.Data;
using AinAlfahd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        MasterDBContext dBContext;
        public AreaController(MasterDBContext dBContext)
        {
            this.dBContext = dBContext;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cities = await dBContext.Areas.ToListAsync();
            return Ok(cities);
        }


        [HttpPost]
        public async Task<IActionResult> AddData([FromBody] Area model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var area = new Area
            {
                Description = model.Description,
            };

            await dBContext.Areas.AddAsync(area);
            await dBContext.SaveChangesAsync();

            return Ok(area);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateData(int id, [FromBody] Area model)
        {
            var existingArea = await dBContext.Areas.FindAsync(id);
            if (existingArea == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            existingArea.Description = model.Description;

            dBContext.Areas.Update(existingArea);
            await dBContext.SaveChangesAsync();

            return Ok(existingArea);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteData(int id)
        {
            var area = await dBContext.Areas.FindAsync(id);
            if (area == null)
                return NotFound();

            dBContext.Areas.Remove(area);
            await dBContext.SaveChangesAsync();

            return Ok();
        }
    }
}

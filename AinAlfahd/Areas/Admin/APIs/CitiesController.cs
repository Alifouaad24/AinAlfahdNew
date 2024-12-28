using AinAlfahd.BL;
using AinAlfahd.Data;
using AinAlfahd.Models;
using AinAlfahd.ModelsDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        MasterDBContext dBContext;
        public CitiesController(MasterDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cities = await dBContext.Cities.ToListAsync();
            return Ok(cities);
        }

        [HttpPost]
        public async Task<IActionResult> AddData([FromBody] City model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var city = new City
            {
                 Description = model.Description,
            };

            await dBContext.Cities.AddAsync(city);
            await dBContext.SaveChangesAsync();

            return Ok(city);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateData(int id, [FromBody] City model)
        {
            var existingCity = await dBContext.Cities.FindAsync(id);
            if (existingCity == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            existingCity.Description = model.Description;

            dBContext.Cities.Update(existingCity);
            await dBContext.SaveChangesAsync();

            return Ok(existingCity);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteData(int id)
        {
            var city = await dBContext.Cities.FindAsync(id);
            if (city == null)
                return NotFound();

            dBContext.Cities.Remove(city);
            await dBContext.SaveChangesAsync();

            return Ok();
        }
    }
}

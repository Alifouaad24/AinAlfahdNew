using AinAlfahd.Data;
using AinAlfahd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingTypesController : ControllerBase
    {
        MasterDBContext dBContext;
        public ShippingTypesController(MasterDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var shippingTypes = await dBContext.ShippingTypes.ToListAsync();
            return Ok(shippingTypes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var shippingType = await dBContext.ShippingTypes.FindAsync(id);
            return Ok(shippingType);
        }

        [HttpPost]
        public async Task<IActionResult> AddData(ShippingTypes model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            await dBContext.ShippingTypes.AddAsync(model);
            await dBContext.SaveChangesAsync();
            return Ok("mode added successfully !");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateData(int id, [FromBody] ShippingTypes model)
        {
            var shippingType = await dBContext.ShippingTypes.FindAsync(id);
            if(shippingType != null)
            {
                shippingType.Description = model.Description;

                dBContext.ShippingTypes.Update(shippingType);
                await dBContext.SaveChangesAsync();

                return Ok("model updated successfully !");
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteData(int id)
        {
            var shippingType = await dBContext.ShippingTypes.FindAsync(id);
            if (shippingType != null)
            {
                dBContext.ShippingTypes.Remove(shippingType);
                await dBContext.SaveChangesAsync();

                return Ok("model deleted successfully !");
            }

            return NotFound();
        }
    }
}

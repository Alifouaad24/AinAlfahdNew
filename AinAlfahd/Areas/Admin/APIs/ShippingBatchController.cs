using AinAlfahd.Data;
using AinAlfahd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingBatchController : ControllerBase
    {
        MasterDBContext dBContext;
        public ShippingBatchController(MasterDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var shippingBatch = await dBContext.ShippingBatchs
                .Include(s => s.ShippingTypes)
                .Select(s => new
                {
                    s.ShippingBatchId,
                    s.ShippingDate,
                    s.EntryDate,
                    s.ShippingTypeId,
                    s.ArrivelDate,
                    BatchCostUS = s.Recipts.Sum(r => r.Cost),
                    s.ShippingTypes,
                    ReciptsNu = s.Recipts != null ? s.Recipts.Count() : 0,
                    s.Notes,
                    SellingIQ = s.Recipts.Sum(r => r.TotalPriceFromCust)

                }).OrderByDescending(s => s.ArrivelDate)
                .ToListAsync();

            return Ok(shippingBatch);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var shippingBatch = await dBContext.ShippingBatchs.Include(s => s.ShippingTypes).FirstOrDefaultAsync(s => s.ShippingBatchId == id);
            return Ok(shippingBatch);
        }

        [HttpPost]
        public async Task<IActionResult> AddData(ShippingBatch model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await dBContext.ShippingBatchs.AddAsync(model);
            await dBContext.SaveChangesAsync();
            return Ok("mode added successfully !");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateData(int id, [FromBody] ShippingBatch model)
        {
            var shippingBatch = await dBContext.ShippingBatchs.FindAsync(id);
            if (shippingBatch != null)
            {
                shippingBatch.ShippingDate = model.ShippingDate;
                shippingBatch.ArrivelDate = model.ArrivelDate;
                shippingBatch.EntryDate = DateTime.Now;
                shippingBatch.batchCostUS = model.batchCostUS;
                shippingBatch.ShippingTypeId = model.ShippingTypeId;
                shippingBatch.Notes = model.Notes;

                dBContext.ShippingBatchs.Update(shippingBatch);
                await dBContext.SaveChangesAsync();

                return Ok("model updated successfully !");
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteData(int id)
        {
            var shippingBatchs = await dBContext.ShippingBatchs.FindAsync(id);
            if (shippingBatchs != null)
            {
                if (shippingBatchs.Recipts.Any())
                {
                    return BadRequest("Recipts related with this batch");
                }
                dBContext.ShippingBatchs.Remove(shippingBatchs);
                await dBContext.SaveChangesAsync();

                return Ok("model deleted successfully !");
            }

            return NotFound();
        }
    }
}

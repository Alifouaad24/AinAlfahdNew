using AinAlfahd.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderAPIController : ControllerBase
    {
        private MasterDBContext dbContext;
        public OrderAPIController(MasterDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("SearchAboutOrderByDate/{datee}")]
        public async Task<IActionResult> SearchAboutOrderByDate(DateOnly datee)
        {
            try
            {
                var orders = await dbContext.OrderDetails.Include(o => o.Item).Include(od => od.Order)
                    .Where(o => o.Order.OrderDt >= datee & o.Item.WebUrl != null)
                    .ToListAsync();

                var ord = orders.Where(o => Regex.IsMatch(o.Item.PCode, @"^\d")).OrderBy(o => o.Order.OrderDt).Take(100)
                    .ToList();
                return Ok(ord);
            }
            catch (Exception ex)
            {
                ex.ToString();
                return Ok();
            }

        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> SearchAboutOrder(int orderId)
        {
            try
            {
                var order = await dbContext.OrderDetails.Where(o => o.OrderNo == orderId).Include(o => o.Item).ToListAsync();
                return Ok(order);
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }

        }

        [HttpPost("{id}/{newSKU}")]
        public async Task<IActionResult> UpdateSKU(int id, string newSKU)
        {
            var iteem = await dbContext.Items.FindAsync(id);

            iteem.OldCode = iteem.PCode;
            iteem.PCode = newSKU;
            var x = iteem.PCode;
            var y = iteem.OldCode;
            dbContext.Update(iteem);
            await dbContext.SaveChangesAsync();

            await SearchAboutOrder(iteem.Id);
            return Ok(new
            {
                PcCode = x,
                OlldCode = y

            });
        }
    }
}

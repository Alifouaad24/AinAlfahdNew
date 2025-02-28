using AinAlfahd.Data;
using AinAlfahd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        MasterDBContext dBContext;
        public OrderDetailsController(MasterDBContext dBContext)
        {
            this.dBContext = dBContext;
        }


        [HttpGet("")]
        [HttpGet("{start?}/{end?}")]
        public async Task<IActionResult> GetAllOrderDetails(int? start, int? end)
        {
            int s = start ?? 0;
            int e = end ?? int.MinValue;
            var order_Details = await dBContext.OrderDetails.Include(o => o.Item).Include(o => o.Size).Skip(s).Take(e).ToListAsync();
            return Ok(order_Details);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetailsById(int id)
        {

            var order_Detail = await dBContext.OrderDetails.Include(o => o.Item).Include(o => o.Size).Where(o => o.Id == id).FirstOrDefaultAsync();
            return Ok(order_Detail);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderDetails([FromBody] OrderDetailsModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = await dBContext.Items.Where(i => i.PCode == model.Sku).FirstOrDefaultAsync();
            if (item == null)
            {
                var i = new Item
                {
                    PCode = model.Sku,
                    ImgUrl = model.ImgUrl,
                    MakeId = model.MakeId,
                    CategoryId = model.CategoryId,
                };
                await dBContext.Items.AddAsync(i);  
                await dBContext.SaveChangesAsync();

                var orderDetail = new OrderDetail
                {
                    OrderNo = 3572,
                    ItemCode = i.Id,
                    Size = model.Size,
                    Returned = 1,
                    WebsitePrice = model.WebsitePrice,
                    MerchantId = model.MerchantId,
                };
                await dBContext.OrderDetails.AddAsync(orderDetail);
                await dBContext.SaveChangesAsync();
                return Ok(orderDetail);
            }

            var orderDetail1 = new OrderDetail
            {
                OrderNo = 3572,
                ItemCode = item.Id,
                Size = model.Size,
                Returned = 1,
                WebsitePrice = model.WebsitePrice,
                MerchantId = model.MerchantId,


            };
            await dBContext.OrderDetails.AddAsync(orderDetail1);
            await dBContext.SaveChangesAsync();
            return Ok(orderDetail1);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditOrderDetail(int id, [FromBody] OrderDetail model)
        {
            var orderDetail1 = await dBContext.OrderDetails.FindAsync(id);
            if (orderDetail1 == null)
                return NotFound();

            orderDetail1.Size = model.Size;
            orderDetail1.WebsitePrice = model.WebsitePrice;
            orderDetail1.ItemCode = model.ItemCode;

            dBContext.Update(orderDetail1);
            await dBContext.SaveChangesAsync();
            return Ok(orderDetail1);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            var orderDetail1 = await dBContext.OrderDetails.FindAsync(id);
            if (orderDetail1 == null)
                return NotFound();
            
            dBContext.Remove(orderDetail1);
            await dBContext.SaveChangesAsync();
            return Ok();

        }

    }
}

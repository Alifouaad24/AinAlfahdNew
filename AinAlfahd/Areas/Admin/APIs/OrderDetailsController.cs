using AinAlfahd.Data;
using AinAlfahd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            };
            await dBContext.OrderDetails.AddAsync(orderDetail1);
            await dBContext.SaveChangesAsync();
            return Ok(orderDetail1);

        }

    }
}

using AinAlfahd.Data;
using AinAlfahd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> AddOrderDetails([FromBody] OrderDetail model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var orderDetail = new OrderDetail
            {
                OrderNo = 3572,
                ItemCode = model.ItemCode,
                Size = model.Size,  
                Returned = 1,
            };
            await dBContext.OrderDetails.AddAsync(orderDetail); 
            await dBContext.SaveChangesAsync();
            return Ok(orderDetail);
        }

    }
}

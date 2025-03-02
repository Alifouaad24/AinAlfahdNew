﻿using AinAlfahd.Data;
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
        [HttpGet("{pageNumber:int}/{pageSize:int}")]
        public async Task<IActionResult> GetAllOrderDetails(int pageNumber = 1, int pageSize = 100)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 100;

            var query = dBContext.OrderDetails
                .Include(o => o.Item).Include(o => o.SizeTB)
                .OrderBy(o => o.OrderId);

            var totalRecords = await query.CountAsync();
            var order_Details = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new { data = order_Details, totalRecords });
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetailsById(int id)
        {

            var order_Detail = await dBContext.OrderDetails.Include(o => o.Item).Include(o => o.SizeTB).Where(o => o.Id == id).FirstOrDefaultAsync();
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
        public async Task<IActionResult> EditOrderDetail(int id, [FromBody] OrderDetailDto model)
        {
            var orderDetail1 = await dBContext.OrderDetails.Include(o => o.SizeTB)
                .Include(o => o.Item).ThenInclude(i => i.Category).Where(o => o.Id == id).FirstOrDefaultAsync();
            if (orderDetail1 == null)
                return NotFound();

            orderDetail1.Size = model.Size;
            orderDetail1.WebsitePrice = model.WebsitePrice;
            orderDetail1.MerchantId = model.MerchantId;
            orderDetail1.Item.CategoryId = model.CategoryId;
            orderDetail1.Item.MakeId = model.MakeId;
            orderDetail1.Item.PCode = model.Sku;

            dBContext.OrderDetails.Update(orderDetail1);
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

    public class OrderDetailDto
    {
        public int? Size { get; set; }
        public decimal WebsitePrice { get; set; }
        public string? Sku { get; set; }
        public int? MerchantId { get; set; }
        public int? CategoryId { get; set; }
        public int? MakeId { get; set; }
        public string? ImgUrl { get; set; }
    }
}

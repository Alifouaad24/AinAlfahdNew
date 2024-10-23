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
    public class AddressController : ControllerBase
    {
        MasterDBContext dBContext;
        public AddressController(MasterDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpGet("GetAllForDetectedCustomer")]
        public async Task<IActionResult> GetAllForCuctomer(int id)
        {
            var adds  = await dBContext.Addresses.Where(a => a.CustomerId == id).ToListAsync();
            return Ok(adds);
        }

        [HttpPost]
        public async Task<IActionResult> AddData(AddressDto data)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var address = new Address
            {
                Area = data.Area,
                City = data.City,
                CustomerId = data.CustomerId,
                InsertDate = DateTime.Now,
                LandMark = data.LandMark,
                IsDefault = data.IsDefault,
                IsDeleted = false,
            };

            await dBContext.Addresses.AddAsync(address);
            await dBContext.SaveChangesAsync();
            return Ok(address);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateData(int id, [FromBody] AddressDto model)
        {
            var address = await dBContext.Addresses.FindAsync(id);
            if (address == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            dBContext.Entry(address).CurrentValues.SetValues(model);
            await dBContext.SaveChangesAsync();

            return Ok(address);
        }

        [HttpDelete]
        public async Task<IActionResult> Deletedata(int id)
        {
            var add = await dBContext.Addresses.FindAsync(id);
            if (add == null)
                return NotFound();

            dBContext.Addresses.Remove(add);
            await dBContext.SaveChangesAsync();
            return Ok($"Address {add.Id} deleted successfuly");

        }


        [HttpPost("HotFix")]
        public async Task<IActionResult> HotFix(int id)
        {
            int area;
            int city;
            string landMark;
            int customer_Id;
            
            var customer = await dBContext.Customers.FindAsync(id);

                area = customer.CustArea ?? 0;
                city = customer.CustCity ?? 0;
                landMark = customer.CustLandmark ?? "";
                customer_Id = customer.Id;

                var add = await dBContext.Addresses.FirstOrDefaultAsync(a => a.Area == area && a.City == city && a.LandMark == landMark && a.CustomerId == customer_Id);

                if(add == null)
                {
                    var address = new Address
                    {
                        Area = area,
                        City = city,
                        LandMark = landMark,
                        CustomerId = customer_Id,
                        IsDefault = true,
                        IsDeleted = false,
                        InsertDate = DateTime.Now,
                    };

                    await dBContext.Addresses.AddAsync(address);
                    await dBContext.SaveChangesAsync();
                }
                

                area = 0;
                city = 0;
                landMark = string.Empty;
                customer_Id = 0;

            

            return Ok("Done! ");

        }

    }
}

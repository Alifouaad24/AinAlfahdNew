using AinAlfahd.BL;
using AinAlfahd.Data;
using AinAlfahd.Models;
using AinAlfahd.ModelsDTO;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        MasterDBContext db;
        ICustomer icustomer;
        public CustomersController(MasterDBContext db, ICustomer icustomer1)
        {
            this.db = db;
            icustomer = icustomer1;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await icustomer.GetAll();
            return Ok(customers);
        }

        [HttpGet("GetBySearch/{wordSearch}")]
        public async Task<IActionResult> GetBySearch(string wordSearch)
        {
            var customers = await icustomer.GetByWord(wordSearch);
            return Ok(customers);
        }


        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await icustomer.GetByID(id);
            return Ok(customer);
        }


        [HttpPost]
        public async Task<IActionResult> AddData([FromBody] CustomerDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var customer = new Customer
            {
                CustArea = model.CustArea,
                CustCity = model.CustCity,
                CustLandmark = model.CustLandmark,
                CustMob = model.CustMob,
                CustMob2 = model.CustMob2,
                CustStatus = model.CustStatus,
                CustName = model.CustName,
                Fbid = model.Fbid,
                FullPackage = model.FullPackage,
                CustProfile = model.CustProfile,
                Lat = model.Lat,
                InsertDt = DateOnly.FromDateTime(DateTime.Now),
                Gisurl = model.Gisurl,
                Hexcode = model.Hexcode,
                Lon = model.Lon,
                

            };
            await icustomer.AddData(customer);

            return Ok(customer.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateData(int id, [FromBody] CustomerDto model)
        {
            var existingCustomer = await db.Customers.FindAsync(id);
            if (existingCustomer == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            existingCustomer.CustArea = model.CustArea;
            existingCustomer.CustCity = model.CustCity;
            existingCustomer.CustLandmark = model.CustLandmark;
            existingCustomer.CustMob = model.CustMob;
            existingCustomer.CustMob2 = model.CustMob2;
            existingCustomer.CustStatus = model.CustStatus;
            existingCustomer.CustName = model.CustName;
            existingCustomer.Fbid = model.Fbid;
            existingCustomer.FullPackage = model.FullPackage;
            existingCustomer.CustProfile = model.CustProfile;
            existingCustomer.Lat = model.Lat;
            existingCustomer.InsertDt = DateOnly.FromDateTime(DateTime.Now);
            existingCustomer.Gisurl = model.Gisurl;
            existingCustomer.Hexcode = model.Hexcode;
            existingCustomer.Lon = model.Lon;

            await icustomer.UpdateData(existingCustomer);

            return Ok(existingCustomer.Id);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteData(int id)
        {
            var cus = await icustomer.GetByID(id);
            if (cus == null)
                return NotFound();

            var result = await icustomer.DeleteData(id);
            if (!result)
                return BadRequest();

            return Ok(result);
        }


        [HttpGet("SearchAboutCustomerApi/{wordSearch}")]

        public async Task<IActionResult> SearchAboutCustomerApi(string wordSearch)
        {
            var customers = await db.Customers.Where(c => c.CustName.Contains(wordSearch) || c.CustMob.Contains(wordSearch)).ToListAsync();
            return Ok(customers);
        }

        [HttpGet("SearchAboutDetectedCustomerApi/{wordSearch}")]

        public async Task<IActionResult> SearchAboutDetectedCustomerApi(string wordSearch)
        {
            var customer = await db.Customers.FirstOrDefaultAsync(c => c.CustName.Contains(wordSearch) || c.CustMob.Contains(wordSearch));
            return Ok(customer);
        }
    }
}

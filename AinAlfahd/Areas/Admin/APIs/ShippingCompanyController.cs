using AinAlfahd.Data;
using AinAlfahd.Models;
using AinAlfahd.Models.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingCompanyController : ControllerBase
    {
        private readonly MasterDBContext dBContext;
        public ShippingCompanyController(MasterDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllShippingCompanies()
        {
            var companies = await dBContext.ShippingCompanies.ToListAsync();
            return Ok(companies);
        }

        [HttpPost]
        public async Task<IActionResult> AddCompany([FromBody] ShippingCompanyDTO model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var company = new ShippingCompany
            {
                Description = model.Description,
                CurrencyType = model.CurrencyType,
                ShipingTypeId = model.ShipingTypeId,
            };

            await dBContext.AddAsync(company);
            await dBContext.SaveChangesAsync();
            return Ok(company);
        }
    }



}

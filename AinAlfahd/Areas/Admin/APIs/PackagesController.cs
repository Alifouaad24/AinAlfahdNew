using System.ComponentModel.DataAnnotations.Schema;
using AinAlfahd.Data;
using AinAlfahd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        MasterDBContext dBContext;
        UserManager<IdentityUser> userManager;
        public PackagesController(MasterDBContext dBContext, UserManager<IdentityUser> userManager)
        {
            this.dBContext = dBContext;
            this.userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var packages = await dBContext.Packages.ToListAsync();
            return Ok(packages);
        }

        [HttpGet("{shippId}")]
        public async Task<IActionResult> GetAllFilterByShippingType(int shippId)
        {
            var packages = await dBContext.Packages
                .Include(p =>  p.ShippingType)
                .Include(p =>  p.Customer)
                .Include(p =>  p.SaleCurrency)
                .Include(p =>  p.PurchaseCurrency)
                .Where(sh => sh.ShippingTypeId == shippId).ToListAsync();
            return Ok(packages);
        }

        [HttpPost]
        public async Task<IActionResult> Addpackage([FromBody] PackageDto model)
        {
            string userName = string.Empty;

            var user = await userManager.GetUserAsync(User);
            if (user != null)
                userName = user.UserName;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var package = new Package
            {
                ActualWeight = model.ActualWeight,
                DimentioalWeight = model.DimentioalWeight,
                ShippingTypeId = model.ShippingTypeId,
                PurcheasCost = model.PurcheasCost,
                CurrencyCostId = model.CurrencyCostId,
                CurrencySaleId = model.CurrencySaleId,
                CustomerId = model.CustomerId,
                ActualWeightForCustomer = model.ActualWeightForCustomer,
                SallingPrice = model.SallingPrice,
                ModifyBy = userName,
                ModifyOn = DateOnly.FromDateTime(DateTime.UtcNow),
            };

            await dBContext.Packages.AddAsync(package);
            await dBContext.SaveChangesAsync();
            return Ok(package); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage(int id)
        {
            var pk = await dBContext.Packages.FindAsync(id);
            if (pk == null) 
              return BadRequest();

            dBContext.Packages.Remove(pk);
            await dBContext.SaveChangesAsync(); 
            return Ok(new
            {
                msg = "package deleted successfuly"
            });
        }

    }
}

public class PackageDto
{
    public decimal ActualWeight { get; set; }
    public decimal DimentioalWeight { get; set; }
    public int ShippingTypeId { get; set; }
    public decimal PurcheasCost { get; set; }
    public int? CurrencyCostId { get; set; }
    public int CurrencySaleId { get; set; }
    public int CustomerId { get; set; }
    public decimal ActualWeightForCustomer { get; set; }
    public decimal SallingPrice { get; set; }
}
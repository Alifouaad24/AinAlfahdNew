using AinAlfahd.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceingRolesController : ControllerBase
    {
        MasterDBContext dBContext;
        public PriceingRolesController(MasterDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var priceRoles = await dBContext.shipping_pricerole
                .Include(p => p.weight_cat).Include(p => p.ShippingTypes)
                .Include(p => p.trade_types).Include(p => p.Currency)
                .ToListAsync();
            return Ok(priceRoles);
        }

        [HttpGet("{shippId}")]
        public async Task<IActionResult> GetByShippingType(int shippId)
        {
            var priceRoles = await dBContext.shipping_pricerole
                .Include(p => p.weight_cat).Include(p => p.ShippingTypes)
                .Include(p => p.trade_types).Include(p => p.Currency)
                .Where(p => p.shipping_type_id == shippId).ToListAsync();
            return Ok(priceRoles);
        }
    }
}

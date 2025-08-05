using AinAlfahd.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerShippingController : ControllerBase
    {
        MasterDBContext dBContext;
        public CustomerShippingController(MasterDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomerShipping()
        {
            var cus_Shipp = await dBContext.CustomerShipping.Include(cs => cs.Customer).Include(cs => cs.ShippingType).ToListAsync();
            return Ok(cus_Shipp);
        }
    }
}

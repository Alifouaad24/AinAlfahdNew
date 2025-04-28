using AinAlfahd.Data;
using AinAlfahd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersShippingController : ControllerBase
    {
        private readonly MasterDBContext dBContext;
        public CustomersShippingController(MasterDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        //[HttpPost]
        //public async Task<IActionResult> AddShippToCustomer([FromBody] CustomerShipping model)
        //{

        //}
    }
}

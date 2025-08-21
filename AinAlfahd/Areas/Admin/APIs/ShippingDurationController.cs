using AinAlfahd.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingDurationController : ControllerBase
    {
        MasterDBContext dBContext { get; set; }
        public ShippingDurationController(MasterDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpGet("{shippId}/{ShippingDate}")]
        public async Task<IActionResult> GetPeriodArrive(int shippId, string ShippingDate)
        {
            var shippType = await dBContext.ShippingTypes.FindAsync(shippId);

            var arriveDate = DateOnly.FromDateTime(Convert.ToDateTime(ShippingDate));

            var startRange = arriveDate.AddDays(shippType.StartRange ?? 0);
            var endRange = arriveDate.AddDays(shippType.EndRange ?? 0);

            return Ok(new
            {
                start = startRange,
                end = endRange
            });
        }
    }
}
    
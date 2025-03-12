using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeDepotController : ControllerBase
    {
        public HomeDepotController()
        {
            
        }

        [HttpPost("{wordSearch}")]
        public async Task<IActionResult> GetProductInfo(string wordSearch)
        {
            List<string> imgs = new List<string>();
            decimal price = 0;
            string upc = string.Empty;

            try
            {
                HtmlWeb web = new HtmlWeb();
                var document = web.Load($"https://www.homedepot.com/s/{wordSearch}");

                var imageNodes = document.DocumentNode.SelectNodes("//div[contains(@class,'mediagallery')]//img");
                if (imageNodes != null)
                {
                    imgs = imageNodes
                        .Select(node => node.GetAttributeValue("src", ""))
                        .Where(src => !string.IsNullOrEmpty(src))
                        .Distinct()
                        .ToList();
                }

                var priceNode = document.DocumentNode.SelectSingleNode("//div[@data-fusion-component='@thd-olt-component-react/price']//span[contains(@class, 'sui-text-9xl')]");
                if (priceNode != null)
                {
                    var priceText = priceNode.InnerText.Trim();
                    if (decimal.TryParse(priceText.Replace("$", "").Replace(",", ""), out decimal parsedPrice))
                    {
                        price = parsedPrice;
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "حدث خطأ أثناء جلب البيانات.", details = ex.Message });
            }

            return Ok(new { images = imgs, price = price });
        }
    }
}

using AinAlfahd.Data;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemAPIController : ControllerBase
    {
        MasterDBContext dBContext;
        public ItemAPIController(MasterDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpGet("{sku}")]
        public async Task<IActionResult> GetItem(string sku)
        {
            var item = await dBContext.Items.FirstOrDefaultAsync(i => i.PCode == sku);
            if (item != null)
            {
                return Ok(item.ImgUrl);
            }

            else
            {
               var imgUrl = await GetImgUrlFromScraping(sku);
               return Ok(imgUrl);
            }
        }


        // get image url from scrape
        private async Task<string> GetImgUrlFromScraping(string itemSKU)
        {
            string baseURL = "https://ar.shein.com/pdsearch/";
            string url = baseURL + itemSKU;
            using HttpClient client = new HttpClient();
            string html = await client.GetStringAsync(url);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var imgNode = doc.DocumentNode
                             .SelectNodes("//img")
                             ?.FirstOrDefault();

            return imgNode?.GetAttributeValue("src", "لا يوجد صورة");
        }

    }
}

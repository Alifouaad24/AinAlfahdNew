using AinAlfahd.Models.Helpers;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class SheInController : ControllerBase
    {

        public SheInController()
        {
            
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> GetInfoFromSheIn(IFormFile htmlFile)
        {
            if (htmlFile == null || htmlFile.Length == 0)
                return BadRequest("الملف غير موجود أو فارغ");

            string htmlContent;
            using (var reader = new StreamReader(htmlFile.OpenReadStream(), Encoding.UTF8))
            {
                htmlContent = await reader.ReadToEndAsync();

            }

            htmlContent = htmlContent.Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
            htmlContent = htmlContent.Replace("\\\"", "\"");
            // تحميل المحتوى في HtmlAgilityPack
            var document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(htmlContent);

            // استخراج السعر
            var priceNode = document.DocumentNode.SelectSingleNode("//div[contains(@class,'ProductIntroHeadPrice__head-mainprice')]//div[contains(@class, 'original')]/span");
            string price = priceNode?.InnerText?.Trim().Replace("$", "") ?? "لم يتم العثور على السعر";

            // استخراج SKU
            var skuNode = document.DocumentNode.SelectSingleNode("//span[contains(@class,'product-intro__head-sku-text')]");
            string sku = skuNode?.InnerText?.Replace("SKU:", "").Trim() ?? "لم يتم العثور على SKU";

            // استخراج رابط الصورة
            var imgNode = document.DocumentNode.SelectSingleNode("//div[contains(@style,'background:url')]//img");
            string imageUrl = imgNode?.GetAttributeValue("src", null);
            if (!string.IsNullOrEmpty(imageUrl) && imageUrl.StartsWith("//"))
            {
                imageUrl = "https:" + imageUrl;
            }
            imageUrl ??= "لم يتم العثور على صورة";

            // إرجاع النتائج
            return Ok(new
            {
                img = imageUrl,
                price = price,
                sku = sku,
                htmlContent = htmlContent
            });
        }







        //[HttpPost]
        //public IActionResult GetInfoFromSheIn([FromBody] string html)
        //{
        //    if (string.IsNullOrEmpty(html))
        //    {
        //        return BadRequest("HTML content is required.");
        //    }


        //    var document = new HtmlDocument();
        //    document.LoadHtml(html);

        //    // استخراج السعر
        //    var priceNode = document.DocumentNode.SelectSingleNode("//div[contains(@class,'ProductIntroHeadPrice__head-mainprice')]//div[contains(@class, 'original')]/span");
        //    string price = priceNode?.InnerText?.Trim() ?? "لم يتم العثور على السعر";

        //    // استخراج SKU
        //    var skuNode = document.DocumentNode.SelectSingleNode("//span[contains(@class,'product-intro__head-sku-text')]");
        //    string sku = skuNode?.InnerText?.Replace("SKU:", "").Trim() ?? "لم يتم العثور على SKU";

        //    // استخراج رابط الصورة
        //    var imgNode = document.DocumentNode.SelectSingleNode("//div[contains(@style,'background:url')]//img");
        //    string imageUrl = imgNode?.GetAttributeValue("src", null);
        //    if (!string.IsNullOrEmpty(imageUrl) && imageUrl.StartsWith("//"))
        //    {
        //        imageUrl = "https:" + imageUrl;
        //    }
        //    imageUrl ??= "لم يتم العثور على صورة";

        //    // العودة بالبيانات
        //    return Ok(new
        //    {
        //        img = imageUrl,
        //        price = price,
        //        sku = sku
        //    });
        //}

    }
}

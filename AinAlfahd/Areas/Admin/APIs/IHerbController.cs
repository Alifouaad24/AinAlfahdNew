using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text.RegularExpressions;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class IHerbController : ControllerBase
    {
        [HttpGet("images/{code}")]
        public IActionResult GetImages(string code)
        {
            var options = new ChromeOptions();
            //options.AddArgument("--headless");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");

            using var driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Navigate().GoToUrl($"https://ae.iherb.com/pr/{code}");

            var imageUrls = new List<string>();
            try
            {
                var mainImage = driver.FindElement(By.Id("iherb-product-image"));
                imageUrls.Add(mainImage.GetAttribute("src"));
            }
            catch { }

            foreach (var thumb in driver.FindElements(By.CssSelector(".thumbnail-item img")))
            {
                var largeImg = thumb.GetAttribute("data-large-img");
                imageUrls.Add(!string.IsNullOrEmpty(largeImg)
                    ? largeImg
                    : thumb.GetAttribute("src"));
            }

            string productName = "Not found";
            try
            {
                productName = driver.FindElement(By.CssSelector("h1#name"))
                                    .Text.Trim();
            }
            catch { }

            string brandName = "Not found";
            try
            {
                brandName = driver.FindElement(By.CssSelector("#brand a span bdi"))
                                  .Text.Trim();
            }
            catch { }

            var html = driver.PageSource;
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var priceNode = doc.DocumentNode
                .SelectSingleNode("//b[contains(@class,'discount-price')]");
            string rawPrice = priceNode?.InnerText.Trim() ?? "";

            var mCur = Regex.Match(rawPrice, @"^[^\d]+");
            var mAmt = Regex.Match(rawPrice, @"[\d\.]+");
            string currency = mCur.Success ? mCur.Value : "";
            string amount = mAmt.Success ? mAmt.Value : "";

            return Ok(new
            {
                Images = imageUrls.Distinct(),
                Name = productName,
                Brand = brandName,
                Price = amount,
                Currency = currency
            });
        }
    }
}

using AinAlfahd.Data;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System.Security.Policy;
using AinAlfahd.Models;
using System.Net;
using AinAlfahd.Models_New;
using OpenQA.Selenium.Edge;

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


        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] Item model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = new Item {

                PCode = model.PCode,
                ImgUrl = model.ImgUrl,
                CategoryId = model.CategoryId,
                MakeId = model.MakeId,

            };

            await dBContext.Items.AddAsync(item);
            await dBContext.SaveChangesAsync();
            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] DtoForAdd model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await dBContext.Items.FindAsync(id);
            if(item != null)
            {
                item.CategoryId = model.CategoryId;
                item.Upc = model.Upc;

                dBContext.Items.Update(item);


                var inventory = new Inventory
                {
                    item_id = item.Id,
                    sellingprice = model.Price,
                    sizeId = model.SizeId,
                    MerchantId = model.MerchantId
                };

                await dBContext.Inventory.AddAsync(inventory);
                await dBContext.SaveChangesAsync();


                return Ok(inventory);
            }
            return NotFound(new
            {
                msg = "Item not found"
            });
        }


        // get image url from scrape
        private async Task<object> GetImgUrlFromScraping(string itemSKU)
        {
            string url = $"https://us.shein.com/pdsearch/{itemSKU}";

            var options = new EdgeOptions();
            options.AddArgument("disable-gpu");
            options.AddArgument("no-sandbox");
            options.AddArgument("disable-dev-shm-usage");

            using (var driver = new EdgeDriver(options))
            {
                try
                {
                    driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
                    driver.Navigate().GoToUrl(url);

                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                    // الصورة الأساسية – نحاول جلب الصورة من مكانها الفعلي وليس من الـ thumbnail
                    var imgElement = wait.Until(d =>
                        d.FindElement(By.XPath("//div[contains(@class, 'product-main-image')]//img")) // غيّر هذا حسب الموقع الفعلي
                        ?? d.FindElement(By.XPath("//img[contains(@class, 'main-image')]")) // بديل إضافي
                    );

                    string imgUrl = imgElement?.GetAttribute("src") ?? "Image not found";

                    // السعر
                    var priceElement = wait.Until(d =>
                        d.FindElement(By.XPath("//span[contains(@class, 'price')]"))
                        ?? d.FindElement(By.XPath("//p[contains(@class, 'product-item__camecase-wrap')]//span"))
                    );

                    string priceText = priceElement?.Text ?? "$0";
                    string currentPrice = priceText.Replace("$", "").Trim();

                    return new
                    {
                        Image = imgUrl,
                        Price = currentPrice
                    };
                }

                catch (Exception)
                {
                    return "Error during Get image url and price";
                }
            }
        }

        // get from final url
        [HttpPost("GetPhotoAndPrise")]
        public async Task<IActionResult> GetPhotoAndPrise(string url)
        {

            var options = new ChromeOptions();
            options.AddArgument("start-maximized");

            using (var driver = new ChromeDriver(options))
            {
                try
                {
                    driver.Navigate().GoToUrl(url);

                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                    wait.Until(driver => driver.FindElement(By.XPath("//img")));

                    var imageElement = driver.FindElements(By.XPath("//img[contains(@class, 'crop-image-container__img')]")).FirstOrDefault();
                    var priceElement = driver.FindElements(By.XPath("//span[contains(@class, 'product-price__price')]")).FirstOrDefault();

                    if (imageElement != null && priceElement != null)
                    {
                        var imageUrl = imageElement.GetAttribute("src");
                        var price = priceElement.Text.Replace("$", "").Trim();

                        return Ok(new { ImageURL = imageUrl, Price = price });
                    }
                    else
                    {
                        return Ok(new { ImageURL = "Not Found", Price = "0" });
                    }
                }


                catch (Exception ex)
                {

                }
                return Ok(new
                {

                });
            }
        }

    }

    public class DtoForAdd { 
    
          public string Upc {  get; set; }
          public int CategoryId {  get; set; }
          public int SizeId {  get; set; }
          public int MerchantId {  get; set; }
          public decimal Price {  get; set; }


    }

}

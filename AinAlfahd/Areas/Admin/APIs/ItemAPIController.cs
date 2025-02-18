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
            if(!ModelState.IsValid)
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


        // get image url from scrape
        private async Task<string> GetImgUrlFromScraping(string itemSKU)
        {
            string url = $"https://ar.shein.com/pdsearch/{itemSKU}";

            var options = new ChromeOptions();
            options.AddArgument("--headless"); 
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");

            using (var driver = new ChromeDriver(options))
            {
                try
                {
                    driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
                    driver.Navigate().GoToUrl(url);

                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    var imgElement = wait.Until(d => d.FindElement(By.XPath("//div[contains(@class, 'crop-image-container')]//img")));

                    return imgElement?.GetAttribute("src") ?? "Image not found";
                }
                catch (Exception)
                {
                    return "Error during Get image url";
                }
            }
        }

    }
}

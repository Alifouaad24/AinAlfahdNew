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
            string baseURL = "https://us.shein.com/pdsearch/";
            string url = baseURL + itemSKU;

            var options = new ChromeOptions();
            options.AddArgument("start-maximized");

            using (var driver = new ChromeDriver(options))
            {
                try
                {
                    driver.Navigate().GoToUrl(url);
                    await Task.Delay(3000);

                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

                    wait.Until(driver => driver.FindElement(By.XPath("//div[contains(@class, 'crop-image-container')]//img")));

                    var imgElement = driver.FindElements(By.XPath("//div[contains(@class, 'crop-image-container')]//img")).FirstOrDefault();

                    if (imgElement != null)
                    {
                        var imgSrc = imgElement.GetAttribute("src");
                        return imgSrc;
                    }
                    else
                    {
                        return "Image not found";
                    }


                }


                catch (Exception ex)
                {
                    return "Error during Get SKU";
                }
            }
        }
    }
}

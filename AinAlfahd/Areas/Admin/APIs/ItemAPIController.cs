﻿using AinAlfahd.Data;
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
        private async Task<Object> GetImgUrlFromScraping(string itemSKU)
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
                    driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(1);
                    driver.Navigate().GoToUrl(url);

                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                    var imgElement = wait.Until(d => d.FindElement(By.XPath("//div[contains(@class, 'crop-image-container')]//img")));
                    var imgUrl = imgElement?.GetAttribute("src") ?? "Image not found";

                    var priceElement = wait.Until(d => d.FindElement(By.XPath("//p[contains(@class, 'product-item__camecase-wrap')]//span")));
     
                    var price = priceElement?.Text ?? "Price not found $";
                    var currentPrice = price.Replace("$", "").Trim();

                    return new {
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
}

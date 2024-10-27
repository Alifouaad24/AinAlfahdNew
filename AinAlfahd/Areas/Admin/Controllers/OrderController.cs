using AinAlfahd.Data;
using AinAlfahd.Models;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Net;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace AinAlfahd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        MasterDBContext dbContext;
        public OrderController(MasterDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("/Admin/Order/SearchAboutOrder/{orderId}")]
        public async Task<IActionResult> SearchAboutOrder(int orderId)
        {
            try
            {
                var order = await dbContext.OrderDetails.Where(o => o.OrderNo == orderId).Include(o => o.Item).ToListAsync();
                return Ok(order);
            }catch (Exception ex)
            {
                ex.ToString();
                return null;
            }

        }

        [HttpGet("/Admin/Order/fixSKU/{orderId}")]
        public async Task<IActionResult> fixSKU(int orderId)
        {
            int total = 0;
            int sum = 0;
            try
            {
                var order = await dbContext.OrderDetails.Where(o => o.OrderNo == orderId).Include(o => o.Item).ToListAsync();

                foreach (var item in order)
                {
                    if (item.Item != null && Regex.IsMatch(item.Item.PCode, @"^\d"))
                    {
                        total++;
                        if(item.Item.WebUrl != null)
                        {
                            var newSku = await GetSKU(item.Item.WebUrl);

                            var iteem = await dbContext.Items.FindAsync(item.Item.Id);
                            if (iteem != null)
                            {
                                item.Item.OldCode = item.Item.PCode;
                                if (newSku.Contains("Not found") || newSku.Contains("Error during Get SKU"))
                                {
                                    continue;
                                }
                                else
                                {
                                    item.Item.OldCode = item.Item.PCode;
                                    item.Item.PCode = newSku;
                                    dbContext.Update(iteem);
                                    await dbContext.SaveChangesAsync();
                                    sum++;
                                }

                            }
                        }
                        
                    }
                }

                await SearchAboutOrder(orderId);
                
                return Ok(new
                {
                    fixedd = sum,
                    total = total,
                });
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }

        }

        public async Task<string> GetSKU(string newModel)
        {

            var options = new ChromeOptions();
            options.AddArgument("start-maximized");

            using (var driver = new ChromeDriver(options))
            {
                try
                {
                    driver.Navigate().GoToUrl(newModel);
                    await Task.Delay(3000);

                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                    wait.Until(driver => driver.FindElement(By.XPath("//img")));
                    var skuElement = driver.FindElements(By.XPath("//div[contains(@class, 'product-intro__head-sku')]//span[contains(@class, 'product-intro__head-sku-text')]")).FirstOrDefault();
                    if (skuElement != null)
                    {
                        var sku = skuElement.Text.Replace("SKU: ", "");

                        return sku;
                    }
                    else
                    {
                        return "Not found";
                    }
                }


                catch (Exception ex)
                {
                    return "Error during Get SKU";
                }
                
            }
        }



        [HttpGet("/Admin/Order/UpdateSKU/{id}/{newSKU}")]
        public async Task<IActionResult> UpdateSKU(int id, string newSKU)
        {
            var iteem = await dbContext.Items.FindAsync(id);

            var existPCode = await dbContext.Items.Where(i => i.PCode == newSKU).FirstOrDefaultAsync();

            if (existPCode == null)
            {
                iteem.OldCode = iteem.PCode;
                iteem.PCode = newSKU;
                var x = iteem.PCode;
                var y = iteem.OldCode;
                dbContext.Update(iteem);
                await dbContext.SaveChangesAsync();

                await SearchAboutOrder(iteem.Id);
                return Ok(new
                {
                    PcCode = x,
                    OlldCode = y

                });
            }

            return Ok();

            }
    }
}

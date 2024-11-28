using AinAlfahd.Data;
using AinAlfahd.Models;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.Modules.Network;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AinAlfahd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        MasterDBContext dbContext;
        private static readonly HttpClient client = new HttpClient();
        public OrderController(MasterDBContext dbContext)
        {
            this.dbContext = dbContext;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult InsertItemToSys()
        {
            return View();
        }

        [HttpPost]
        public async Task<Item> GetItemData([FromBody] string sku)
        {
            var item = await dbContext.Items.FirstOrDefaultAsync(i => i.PCode == sku);
            return item;
        }

        [HttpPost("/Admin/Order/GetSizes")]
        public async Task<List<TblSize>> GetSizes([FromBody] int id)
        {
            var size = await dbContext.TblSizes.Where(s => s.CategoryId == id).ToListAsync();
            return size;
        }


        public async Task<IActionResult> SaveOrderDetails(OrderDetail model)
        {
            if (ModelState.IsValid)
            {
                TempData["message"] = "حدث خطأ اثناء ادخال الطلب يرجى التحقق من الحقول";
                return RedirectToAction("InsertItemToSys");
            }

            if(model.Id == 0)
            {

                var OrderDetail = new OrderDetail
                {
                    ItemCode = model.ItemCode,
                    Size = model.Size,
                    OrderNo = model.OrderNo,
                };

                await dbContext.OrderDetails.AddAsync(OrderDetail);
                await dbContext.SaveChangesAsync();
                TempData["message"] = "تم حفظ الطلب بنجاح";
                return RedirectToAction("InsertItemToSys");
            }
            else
            {
                var OrderDetail = await dbContext.OrderDetails.FindAsync(model.Id);
                if(OrderDetail != null)
                {
                    OrderDetail.ItemCode = model.ItemCode;
                    OrderDetail.Size = model.Size;
                    OrderDetail.OrderNo = model.OrderNo;

                    dbContext.OrderDetails.Update(OrderDetail);
                    await dbContext.SaveChangesAsync();
                    TempData["message"] = "تم تعديل الطلب بنجاح";
                    return RedirectToAction("InsertItemToSys");
                }

                TempData["message"] = "حدث خطأ اثناء تعديل الطلب يرجى التحقق من الحقول";
                return RedirectToAction("InsertItemToSys");
            }
        }



        public async Task<IActionResult> OrderTable()
        {
            var orders = await dbContext.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Item)
                .Include(o => o.Customer)
                .Take(50)
                .ToListAsync();

            var groupedOrders = orders.GroupBy(o => o.OrderOwner).ToList();

            return View(orders);
        }

        public async Task<IActionResult> Special()
        {
            return View();
        }

        [HttpGet("/Admin/Order/SearchAboutOrderByDate/{datee}")]
        public async Task<IActionResult> SearchAboutOrderByDate(DateOnly datee)
        {
            try
            {
                var orders = await dbContext.OrderDetails.Include(o => o.Item).Include(od => od.Order)
                    .Where(o => o.Order.OrderDt >= datee & o.Item.WebUrl != null)
                    .ToListAsync();

                var ord = orders.Where(o => Regex.IsMatch(o.Item.PCode, @"^\d")).OrderBy(o => o.Order.OrderDt).Take(100)
                    .ToList();
                return Ok(ord);
            }
            catch(Exception ex)
            {
                ex.ToString();
                return Ok();
            }

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
            int Total = 0;
            int sum = 0;
            try
            {
                var order = await dbContext.OrderDetails.Where(o => o.OrderNo == orderId).Include(o => o.Item).ToListAsync();

                foreach (var item in order)
                {
                    if (item.Item != null && Regex.IsMatch(item.Item.PCode, @"^\d"))
                    {
                        Total++;
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
                    total = Total,
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

        [HttpGet("/Admin/Order/UpdateSKUAuto/{id}")]
        public async Task<IActionResult> UpdateSKUAuto(int id)
        {
            var iteem = await dbContext.Items.FindAsync(id);

            string url = "http://www.apiainalfahad.somee.com/api/SheIn/GetSKU";
            var jsonBody = $"{{\"url\": \"{iteem.WebUrl}\"}}";

            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            try
            {
                client.DefaultRequestHeaders.Add("accept", "*/*");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    return Ok(responseData);
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    return Ok(errorResponse);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return BadRequest(e.Message);
            }
        }


        public async Task<IActionResult> SeaShipphngScreen()
        {
            return View();
        }

        [HttpPost("/Admin/Order/ExecuteOperation")]
        public async Task<IActionResult> ExecuteOperation([FromBody] List<OrderMo> orders)
        {

            try
            {
                var ordsGr = orders.GroupBy(or => or.CustomerId).ToList();
                int total = 0;

                foreach (var group in ordsGr)
                {
                    var order = new Order
                    {
                        OrderOwner = Convert.ToInt32(group.Key),
                        OrderStatus = Convert.ToInt32(1),
                        OrderDt = DateOnly.FromDateTime(DateTime.Now),
                    };


                    await dbContext.Orders.AddAsync(order);
                    await dbContext.SaveChangesAsync();

                    foreach (var o in group)
                    {
                        var detail = new OrderDetail
                        {
                            OrderNo = order.Id,
                            OriginalAmount = (int)o.Amount,
                            ItemCode = 3011,
                            weight = o.Weight
                        };


                        await dbContext.OrderDetails.AddAsync(detail);
                        await dbContext.SaveChangesAsync();
                    }

                    foreach (var o in group)
                    {
                        total += (int)o.Amount;
                    }

                    order.NetAmount = total;
                    dbContext.Orders.Update(order);
                    await dbContext.SaveChangesAsync();
                    total = 0;
                }


                TempData["msg"] = "تم إدخال البيانات بنجاح !";
                return RedirectToAction("SeaShipphngScreen");
            }
            catch (Exception ex)
            {
                ex.ToString();
                TempData["err"] = "حدث خطأ أثناء إدخال البيانات يرجى المحولة مجددا والتحقق من ادخال كافة المعلومات المطلوبة !";
                return RedirectToAction("SeaShipphngScreen");
            }

        }

    }

    class ItemRequest
    {
        public string url { get; set; }
    }

    public class OrderMo
    {
        public string CustomerId { get; set; }
        public decimal Weight { get; set; }
        public decimal Amount { get; set; }
    }
}

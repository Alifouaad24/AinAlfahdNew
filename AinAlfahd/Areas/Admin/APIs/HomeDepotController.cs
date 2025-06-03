using AinAlfahd.Data;
using AinAlfahd.Models;
using AinAlfahd.Models_New;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Reflection.Emit;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeDepotController : ControllerBase
    {
        MasterDBContext dBContext;
        private readonly HttpClient _httpClient;

        public HomeDepotController(MasterDBContext dBContext, HttpClient httpClient)
        {
            this.dBContext = dBContext;
            _httpClient = httpClient;

        }

        [HttpGet("lookup")]
        public async Task<IActionResult> Lookup(string upc)
        {
            var result = await LookupUPCAsync(upc);
            return Ok(result);
        }

        [HttpPost("{wordSearch}")]
        public async Task<IActionResult> GetProductInfo(string wordSearch)
        {
            List<string> imgs = new List<string>();
            decimal price = 0;
            string upc = string.Empty;
            string brand = string.Empty;
            string internet = string.Empty;
            string model = string.Empty;
            string storeSku = string.Empty;
            string productTitle = string.Empty;
            string source = string.Empty;
            List<string> description = new List<string>();

            var itemExcst = await dBContext.Items.Where(i => i.Sku == wordSearch || i.Model == wordSearch || i.InternetId == wordSearch).FirstOrDefaultAsync();

            if (itemExcst != null)
            {
                return Ok(new { images = itemExcst.ImgUrl, price = itemExcst.SitePrice, Model = itemExcst.Model, Internet = itemExcst.InternetId, SKU = itemExcst.Sku, Brand = itemExcst.Make.MakeDescription, Source = "Ain Alfahd DB", desc = "" });
            }

            bool foundInHomeDepot = false;

            try
            {
                HtmlWeb web = new HtmlWeb();
                var document = web.Load($"https://www.homedepot.com/s/{wordSearch}");

                // var imageNodes = document.DocumentNode.SelectNodes("//div[contains(@class,'mediagallery')]//img");
                var imageNodes = document.DocumentNode.SelectNodes("//img[contains(@src, 'images.thdstatic.com/productImages/')]");

                if (imageNodes != null)
                {
                        imgs = imageNodes
                        .Select(node => node.GetAttributeValue("src", ""))
                        .ToList();

                }

                var imageNodesSmall = document.DocumentNode.SelectNodes("//div[contains(@class,'mediagallery__thumbnailImageFit')]//img");
                if (imageNodesSmall != null)
                {
                    var final = imageNodesSmall.Select(img => img.GetAttributeValue("src", "")).ToList();

                    imgs.AddRange(final);

                }
                // استخراج Store SKU #
                var skuNode = document.DocumentNode.SelectSingleNode("//h2[contains(text(), 'Store SKU #')]/span");
                storeSku = skuNode != null ? skuNode.InnerText.Trim() : "Store SKU not found";
                var titleNode = document.DocumentNode.SelectSingleNode("//div[contains(@class, 'product-details__badge-title--wrapper')]//h1");

                productTitle = titleNode != null ? titleNode.InnerText.Trim() : " not found";

                var descNode = document.DocumentNode.SelectNodes("//div[@data-fusion-slot='slotF8']//ul//li");

                foreach(var i in descNode)
                {
                    description.Add(descNode != null ? i.InnerText.Trim() : "not found");

                }



                var brandNode = document.DocumentNode.SelectSingleNode("//div[@data-component='product-details:ProductDetailsBrandCollection:v9.13.3']//h2");
                brand = brandNode != null ? brandNode.InnerText.Trim() : " not found";

                var internetNode = document.DocumentNode.SelectSingleNode("//h2[contains(text(), 'Internet #')]/span");
                internet = internetNode != null ? internetNode.InnerText.Trim() : " not found";

                // استخراج Model #
                var modelNode = document.DocumentNode.SelectSingleNode("//h2[contains(text(), 'Model #')]/span");
                model = modelNode != null ? modelNode.InnerText.Trim() : " not found";

                var priceNode = document.DocumentNode.SelectSingleNode("//div[@data-fusion-component='@thd-olt-component-react/price']//span[contains(@class, 'sui-text-9xl')]");
                if (priceNode != null)
                {
                    var priceText = priceNode.InnerText.Trim();
                    if (decimal.TryParse(priceText.Replace("$", "").Replace(",", ""), out decimal parsedPrice))
                    {
                        price = parsedPrice;
                    }
                }
                if (imgs.Count == 0 &&
                    price == 0 &&
                    model.Contains(" not found") &&
                    internet.Contains(" not found") &&
                    storeSku.Contains(" not found") &&
                    brand.Contains(" not found") &&
                    productTitle.Contains(" not found"))
                {
                    string url = $"https://api.upcitemdb.com/prod/trial/lookup?upc={wordSearch}";

                    HttpResponseMessage response = await _httpClient.GetAsync(url);
                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var json = JObject.Parse(responseContent);
                        var item = json["items"]?.FirstOrDefault();

                        if (item != null)
                        {
                            var imgs1 = item["images"]?.ToObject<List<string>>();
                            var offers = item["offers"]?.FirstOrDefault();
                            var price1 = offers?["price"]?.ToObject<decimal?>() ?? 0;
                            var model1 = item["model"]?.ToString();
                            var internet1 = item["asin"]?.ToString(); // Internet = ASIN
                            var storeSku1 = item["sku"]?.ToString(); // غير موجود بشكل صريح، ممكن يكون null
                            var brand1 = item["brand"]?.ToString();
                            var productTitle1 = item["title"]?.ToString();
                            var description2 = item["description"]?.ToString();

                            return Ok(new
                            {
                                images = imgs1,
                                price = price1,
                                Model = model1,
                                Internet = internet1,
                                SKU = storeSku1,
                                Brand = brand1,
                                Title = productTitle1,
                                Source = "UPC Items DB",
                                desc = description2
                            });
                        }

                        return NotFound(new { msg = "Item not found in response" });
                    }
                    else
                    {
                        return NotFound(new { msg = "Item Not Found" });
                    }
                }

                    return Ok(new { images = imgs, price = price, Model = model, Internet = internet, SKU = storeSku, Brand = brand, Title = productTitle, Source = "Home Depot DB", desc = description });

            }
            catch (Exception ex){}
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> SaveItemFromHomeDepot([FromBody] ItemDto item)
        {
            int makeIId = 0;
            var brand = await dBContext.Makes.Where(m => m.MakeDescription == item.Brand).FirstOrDefaultAsync();
            
            if (brand == null)
            {
                var newBrand = new Make
                {
                    MakeDescription = item.Brand
                };
                await dBContext.Makes.AddAsync(newBrand);
                await dBContext.SaveChangesAsync();

                makeIId = newBrand.MakeId;
            }
            else
            {
                makeIId = brand.MakeId;
            }

            var itemNew = new Item
            {
                CategoryId = item.CategoryId,
                InternetId = item.Internet,
                MakeId = makeIId,
                Model = item.Model,
                Sku = item.SKU,
                ImgUrl = item.ImgUrl,
                SitePrice = item.Price,

            };
            try
            {
                await dBContext.Items.AddAsync(itemNew);
                await dBContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                ex.ToString();
            }

            //var inventory = new Inventory
            //{
            //    ItemId = itemNew.Id,
            //    ItemConditionId = item.ItemCondetionId,
            //    IsRemoved = 0,
            //    InsertDate = DateOnly.FromDateTime(DateTime.Now),
            //    InsertBy = "No ",
            //    ItemNotes = item.Notes,
            //};

            //await dBContext.AddAsync<Inventory>(inventory);
            //await dBContext.SaveChangesAsync();

            return Ok(new { item = itemNew });

        }

        private async Task<string> LookupUPCAsync(string upcCode)
        {
            string url = $"https://api.upcitemdb.com/prod/trial/lookup?upc={upcCode}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return responseContent;
            }
            else
            {
                return responseContent;
            }
        }

    }

    public class ItemDto {
        
        public string? Brand { get; set; }
        public string? SKU { get; set; }
        public string? Model { get; set; }
        public decimal? Price { get; set; }
        public string? ImgUrl { get; set; }
        public string? Internet { get; set; }
        public string? Notes { get; set; }
        public int? CategoryId { get; set; }
        public int? ItemCondetionId { get; set; }

    }

}

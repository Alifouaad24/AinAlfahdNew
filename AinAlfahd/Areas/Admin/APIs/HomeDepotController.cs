using AinAlfahd.Data;
using AinAlfahd.Models;
using AinAlfahd.Models_New;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

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

            var itemExcst = await dBContext.Items.Where(i => i.Sku == storeSku || i.Model == storeSku || i.InternetId == storeSku).FirstOrDefaultAsync();

            if (itemExcst != null)
            {
                return Ok(new { images = itemExcst.ImgUrl, price = itemExcst.SitePrice, Model = itemExcst.Model, Internet = itemExcst.InternetId, SKU = itemExcst.Sku, Brand = itemExcst.Make.MakeDescription });
            }
            else
            {
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
                    storeSku = skuNode != null ? skuNode.InnerText.Trim() : "لم يتم العثور على Store SKU";
                    var titleNode = document.DocumentNode.SelectSingleNode("//div[contains(@class, 'product-details__badge-title--wrapper')]//h1");

                    productTitle = titleNode != null ? titleNode.InnerText.Trim() : "لم يتم العثور على العنوان";

                    

                    var brandNode = document.DocumentNode.SelectSingleNode("//div[@data-component='product-details:ProductDetailsBrandCollection:v9.13.3']//h2");
                    brand = brandNode != null ? brandNode.InnerText.Trim() : "لم يتم العثور على اسم الشركة";

                    var internetNode = document.DocumentNode.SelectSingleNode("//h2[contains(text(), 'Internet #')]/span");
                    internet = internetNode != null ? internetNode.InnerText.Trim() : "لم يتم العثور على Internet";

                    // استخراج Model #
                    var modelNode = document.DocumentNode.SelectSingleNode("//h2[contains(text(), 'Model #')]/span");
                    model = modelNode != null ? modelNode.InnerText.Trim() : "لم يتم العثور على Model";

                    var priceNode = document.DocumentNode.SelectSingleNode("//div[@data-fusion-component='@thd-olt-component-react/price']//span[contains(@class, 'sui-text-9xl')]");
                    if (priceNode != null)
                    {
                        var priceText = priceNode.InnerText.Trim();
                        if (decimal.TryParse(priceText.Replace("$", "").Replace(",", ""), out decimal parsedPrice))
                        {
                            price = parsedPrice;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(new { error = "حدث خطأ أثناء جلب البيانات.", details = ex.Message });
                }
            }

          

            return Ok(new { images = imgs, price = price, Model = model, Internet = internet, SKU = storeSku, Brand = brand, Title = productTitle });
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

            await dBContext.Items.AddAsync(itemNew);
            await dBContext.SaveChangesAsync();

            var inventory = new Inventory
            {
                ItemId = itemNew.Id,
                ItemConditionId = item.ItemCondetionId,
                IsRemoved = 0,
                InsertDate = DateOnly.FromDateTime(DateTime.Now),
                InsertBy = "No ",
                ItemNotes = item.Notes,
            };

            await dBContext.AddAsync<Inventory>(inventory);
            await dBContext.SaveChangesAsync();

            return Ok(new { inventory = inventory, item = itemNew });

        }

        private async Task<string> LookupUPCAsync(string upcCode)
        {
            string url = $"https://api.upcitemdb.com/prod/trial/lookup?upc={upcCode}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return jsonResponse;
            }
            else
            {
                return $"Error: {response.StatusCode}";
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

using AinAlfahd.Data;
using AinAlfahd.Models;
using AinAlfahd.ModelsDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace AinAlfahd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReciptController : Controller
    {
        MasterDBContext dBContext;

        public ReciptController(MasterDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public async Task<IActionResult> Index()
        {
            var recipts = await dBContext.Reciepts.Where(c => c.CurrentState == true).Include(r => r.Customer).Include(r => r.ShippingBatch).OrderBy(r => r.RecieptDate).ToListAsync();
            return View(recipts);
        }

        public async Task<IActionResult> AddReciept(int? id)
        {
            var customers = await dBContext.Customers.Include(c => c.CustomerServices)
                .ThenInclude(cs => cs.Service)
                .Where(c => c.CustomerServices.Any(cs => cs.Service.Description == "Air Shipping"))
                .ToListAsync();

            var shippingBatch = await dBContext.ShippingBatchs.ToListAsync();
 
            ViewBag.shippingBatchs = shippingBatch.Select(c => new SelectListItem
            {
                Value = c.ShippingBatchId.ToString(),
                Text = c.ArrivelDate?.ToString("yyyy-MM-dd"),
            }).ToList();

            var excgange = await dBContext.Exchanges.FirstOrDefaultAsync();

            ViewBag.ex = excgange.ExchangeRate;
            ViewBag.Customer = customers.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CustName,
            }).ToList();

            var reciepts = await dBContext.Reciepts.ToListAsync();

            if(id.HasValue && id != 0)
            {
                var reciept = await dBContext.Reciepts.FindAsync(id);
                return View(reciept);
            }

            return View(new Reciept());
        }

        [HttpPost]
        public async Task<IActionResult> SaveRe(RecirptDto model)
        {
            if(model.RecieptDate > DateTime.Now)
            {
                TempData["error"] = "يرجى التحقق من التاريخ المدخل!";
                return RedirectToAction("AddReciept");
            }

            if (!ModelState.IsValid)
            {
                TempData["error"] = "البيانات المدخلة غير صحيحة الرجاء إعادة المحاولة !";
                return RedirectToAction("AddReciept");
            }
            if(model.RecieptId == 0)
            {
                var reci = await dBContext.Reciepts.Where(r => r.CustomerId == model.CustomerId & r.Weight == model.Weight & r.RecieptDate == model.RecieptDate).FirstOrDefaultAsync();
                if (reci != null)
                {
                    TempData["error"] = "الوصل موجود مسبقا  !";
                    return RedirectToAction("AddReciept");
                }
                var re = new Reciept
                {
                    Cost = model.Cost,
                    Currency = "$",
                    CustomerId = model.CustomerId,
                    DisCount = model.DisCount,
                    InsertBy = "Anyy one",
                    IsFinanced = model.IsFinanced,
                    InsertDate = DateTime.Now,
                    SellingCurrency = "IQ",
                    RecieptDate = model.RecieptDate,
                    SellingDisCount = model.SellingDisCount,
                    SellingPrice = model.SellingPrice,
                    Weight = model.Weight,
                    TotalPriceFromCust = Math.Ceiling(model.TotalPriceFromCust),
                    CurrentState = model.CurrentState,
                    Notes = model.Notes,
                    ShippingBatchId = model.ShippingBatchId,
                };

                await dBContext.Reciepts.AddAsync(re);
                await dBContext.SaveChangesAsync();
                TempData["error"] = "تم الحفظ بنجاح  !";
                return RedirectToAction("AddReciept");
            }
            else
            {
                var rec = await dBContext.Reciepts.FindAsync(model.RecieptId);
                if (rec != null)
                {
                    rec.Cost = model.Cost;
                    rec.Currency = "$";
                    rec.CustomerId = model.CustomerId;
                    rec.DisCount = model.DisCount;
                    rec.InsertBy = "Anyy one";
                    rec.IsFinanced = model.IsFinanced;
                    rec.InsertDate = DateTime.Now;
                    rec.SellingCurrency = "IQ";
                    rec.RecieptDate = model.RecieptDate;
                    rec.SellingDisCount = model.SellingDisCount;
                    rec.SellingPrice = model.SellingPrice;
                    rec.Weight = model.Weight;
                    rec.TotalPriceFromCust = Math.Ceiling(model.TotalPriceFromCust);
                    rec.CurrentState = true;

                    dBContext.Reciepts.Update(rec);
                    await dBContext.SaveChangesAsync();
                    TempData["msg"] = "تم التعديل بنجاح  !";
                    return RedirectToAction("Index");
                }
                else {
                    TempData["error"] = "خطأ أثناء تعديل البيانات !";
                    return RedirectToAction("AddReciept");
                }

            }
            
        }

        public async Task<IActionResult> DeleteData(int id)
        {
            var rec = await dBContext.Reciepts.FindAsync(id);
            if (rec != null) {

                rec.CurrentState = false;
                await dBContext.SaveChangesAsync();
                TempData["msg"] = "تم الحذف بنجاح !";
                return RedirectToAction("Index");
            }
            TempData["msg"] = "حدث خطأ اثناء حذف السجل !";
            return RedirectToAction("Index");
        }

        [HttpGet("/Admin/Customer/SerachAboutCust/{word}")]
        public async Task<IActionResult> SerachAboutCust(string word)
        {
            var costomers = await dBContext.Customers.Where(c => c.CustMob.Contains(word) || c.CustName.Contains(word) 
            || c.Id.ToString() == word).ToListAsync();
            return Ok(costomers);
        }

        [HttpGet("/Admin/Customer/SerachAboutCust1/{word}")]
        public async Task<IActionResult> SerachAboutCust1(string word)
        {
            var costomers = await dBContext.Customers.Where(c => c.CustMob.Contains(word) || c.CustName.Contains(word)
            || c.Id.ToString() == word).ToListAsync();
            return Ok(costomers);
        }

        [HttpGet("/Admin/Recipt/SerachByDate/{from}/{to}")]
        public async Task<IActionResult> SerachByDate(string from, string to)
        {
            var recipts = await dBContext.Reciepts
                .Where(c => c.CurrentState == true & c.RecieptDate >= DateTime.Parse(from) & c.RecieptDate <= DateTime.Parse(to))
                .OrderBy(r => r.RecieptDate)
                .Include(r => r.Customer).ToListAsync();


            return Ok(recipts);
        }
    }

    }




using AinAlfahd.Data;
using AinAlfahd.Models;
using AinAlfahd.ModelsDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            var recipts = await dBContext.Reciepts.Include(r => r.Customer).ToListAsync();
            return View(recipts);
        }

        public async Task<IActionResult> AddReciept(int? id)
        {
            var customers = await dBContext.Customers.ToListAsync();
            var excgange = await dBContext.Exchanges.FirstOrDefaultAsync();

            ViewBag.ex = excgange.BankRate;
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

            if (!ModelState.IsValid)
            {
                TempData["error"] = "البيانات المدخلة غير صحيحة الرجاء إعادة المحاولة !";
                return RedirectToAction("AddReciept");
            }

            var re = new Reciept
            {
                Cost = model.Cost,
                Currency ="$",
                CustomerId = model.CustomerId,
                DisCount = model.DisCount,
                FinanceDate = model.FinanceDate,
                InsertBy = "Anyy one",
                IsFinanced = model.IsFinanced,
                InsertDate = DateTime.Now,
                SellingCurrency = "IQ",
                RecieptDate = model.RecieptDate,
                SellingDisCount = model.SellingDisCount,
                SellingPrice = model.SellingPrice,
                Weight = model.Weight,
                
            };

            await dBContext.Reciepts.AddAsync(re);
            await dBContext.SaveChangesAsync();
            TempData["error"] = "تم الحفظ بنجاح  !";
            return RedirectToAction("AddReciept");
            }

        [HttpGet("/Admin/Customer/SerachAboutCust/{word}")]
        public async Task<IActionResult> SerachAboutCust(string word)
        {
            var costomers = await dBContext.Customers.Where(c => c.CustMob.Contains(word) || c.CustName.Contains(word)).ToListAsync();
            return Ok(costomers);
        }
    }

    }




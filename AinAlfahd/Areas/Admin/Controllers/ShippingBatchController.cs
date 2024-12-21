using AinAlfahd.Data;
using AinAlfahd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShippingBatchController : Controller
    {
        MasterDBContext dBContext;
        public ShippingBatchController(MasterDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public async Task<IActionResult> Index()
        {
            var shippingBatchs = await dBContext.ShippingBatchs.Include(sh => sh.ShippingTypes).ToListAsync();
            return View(shippingBatchs);
        }

        public async Task<IActionResult> AddShippingBatch(int? iid)
        {
            var shipp = await dBContext.ShippingBatchs.Include(s => s.ShippingTypes).FirstOrDefaultAsync(s => s.ShippingBatchId == iid);
            var types = await dBContext.ShippingTypes.ToListAsync();
            ViewBag.shippingType = types.Select(a => new SelectListItem
            {
                Value = a.ShippingTypeId.ToString(),
                Text = a.Description
            }).ToList();

            if (shipp != null)
                return View(shipp);

            return View(new ShippingBatch());
        }

        public async Task<IActionResult> SaveShipping(ShippingBatch model)
        {
            if (!ModelState.IsValid)
            {
                TempData["msg"] = "تحقق من البيانات المدخلة من فضلك";
                return RedirectToAction("AddShippingBatch");
            }

            if (model.ShippingBatchId == 0)
            {
                var shipp = new ShippingBatch
                {
                    ArrivelDate = model.ArrivelDate,
                    EntryDate = model.EntryDate,
                    ShippingDate = model.ShippingDate,
                    batchCostUS = model.batchCostUS,
                    ShippingTypeId = model.ShippingTypeId,
                };

                await dBContext.ShippingBatchs.AddAsync(shipp);
                await dBContext.SaveChangesAsync();
                TempData["msg"] = "تم الحفظ بنجاح";
                return RedirectToAction("Index");
            }
            else if (model.ShippingBatchId > 0)
            {
                var shipp = await dBContext.ShippingBatchs.FindAsync(model.ShippingBatchId);
                shipp.ArrivelDate = model.ArrivelDate;
                shipp.EntryDate = model.EntryDate;
                shipp.ShippingDate = model.ShippingDate;
                shipp.ShippingTypeId = model.ShippingTypeId;
                shipp.batchCostUS = model.batchCostUS;

                dBContext.ShippingBatchs.Update(shipp);
                await dBContext.SaveChangesAsync();
                TempData["msg"] = "تم الحفظ بنجاح";
                return RedirectToAction("Index");
            }

            TempData["msg"] = "حدث خطأ ما يرجى المحاولة مجددا !";
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> DeleteData(int id)
        {
            var shipp = await dBContext.ShippingBatchs.FindAsync(id);
            if (shipp == null)
            {
                TempData["msg"] = "حدث خطأ ما يرجى المحاولة مجددا !";
                return RedirectToAction("Index");
            }
            dBContext.ShippingBatchs.Remove(shipp);
            await dBContext.SaveChangesAsync();
            TempData["msg"] = "تم الحذف بنجاح !";
            return RedirectToAction("Index");

        }


    }
}

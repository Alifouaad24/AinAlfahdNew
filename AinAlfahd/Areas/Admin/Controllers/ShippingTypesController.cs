using AinAlfahd.Data;
using AinAlfahd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace AinAlfahd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShippingTypesController : Controller
    {
        MasterDBContext dBContext;
        public ShippingTypesController(MasterDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<IActionResult> Index()
        {
            var shippingTypes = await dBContext.ShippingTypes.ToListAsync();
            return View(shippingTypes);
        }

        public async Task<IActionResult> AddShippingType(int iid)
        {
            var shipp = await dBContext.ShippingTypes.FindAsync(iid);

            if(shipp != null)
                return View(shipp);

            return View(new ShippingTypes());
        }

        public async Task<IActionResult> SaveShipp(ShippingTypes model)
        {
            if (!ModelState.IsValid)
            {
                TempData["msg"] = "تحقق من البيانات المدخلة من فضلك";
                return RedirectToAction("AddShippingType");
            }

            if(model.ShippingTypeId == 0)
            {
                var shipp = new ShippingTypes
                {
                    Description = model.Description,
                };

                await dBContext.ShippingTypes.AddAsync(shipp);
                await dBContext.SaveChangesAsync();
                TempData["msg"] = "تم الحفظ بنجاح";
                return RedirectToAction("Index");
            }
            else if(model.ShippingTypeId > 0)
            {
                var shipp = await dBContext.ShippingTypes.FindAsync(model.ShippingTypeId);
                shipp.Description = model.Description;
                dBContext.ShippingTypes.Update(shipp);
                await dBContext.SaveChangesAsync();
                TempData["msg"] = "تم الحفظ بنجاح";
                return RedirectToAction("Index");
            }

            TempData["msg"] = "حدث خطأ ما يرجى المحاولة مجددا !";
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> DeleteData(int id)
        {
            var shipp = await dBContext.ShippingTypes.FindAsync(id);
            if (shipp == null)
            {
                TempData["msg"] = "حدث خطأ ما يرجى المحاولة مجددا !";
                return RedirectToAction("Index");
            }
            dBContext.ShippingTypes.Remove(shipp);
            await dBContext.SaveChangesAsync();
            TempData["msg"] = "تم الحذف بنجاح !";
            return RedirectToAction("Index");

        }
    }
}

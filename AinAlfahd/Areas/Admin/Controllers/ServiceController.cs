using AinAlfahd.Data;
using AinAlfahd.Models;
using Microsoft.AspNetCore.Mvc;

namespace AinAlfahd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {
        MasterDBContext dBContext;
        public ServiceController(MasterDBContext dBContext = null)
        {
            this.dBContext = dBContext;
        }
        public async Task<IActionResult> Index(int? iid)
        {
            var service = await dBContext.Services.FindAsync(iid);
            if (service != null)
                return View(service);

            return View(new Service());
        }

        public async Task<IActionResult> SaveService(ServiceDTO model)
        {
            if (ModelState.IsValid)
            {
                var service = new Service
                {
                    Description = model.Description,
                };

                await dBContext.Services.AddAsync(service);
                await dBContext.SaveChangesAsync();

                TempData["message"] = "تم إضافة الخدمة بنجاح !";
                return RedirectToAction("Index"); 
            }
            TempData["message"] = "حدث خطأ أثناء إضافة الخدمة يرجى المحاولة مجددا !";
            return RedirectToAction("Index");
        }
    }

    public class ServiceDTO 
    {
        public string Description { get; set; }
    }

}

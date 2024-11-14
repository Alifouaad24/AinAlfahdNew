using AinAlfahd.Data;
using AinAlfahd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerServicesController : Controller
    {
        MasterDBContext dBContext;
        public CustomerServicesController(MasterDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<IActionResult> AddCustomerService()
        {

            var services = await dBContext.Services.ToListAsync();

            ViewBag.Services = services.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Description
            }).ToList();

            var CustSer = new CustomerService();
            return View(CustSer);
        }

        [HttpPost]
        public async Task Save(CustomerService customerService)
        {
            if (!ModelState.IsValid)
                TempData["message"] = "حدث خطأ أثناء حفظ البيانات";
               RedirectToAction("AddCustomerService");

            var CustomerService = new CustomerService
            {
                CustomerId = customerService.CustomerId,
                ServiceId = customerService.ServiceId,
            };

            await dBContext.CustomerServices.AddAsync(CustomerService);
            await dBContext.SaveChangesAsync();
            TempData["message"] = "تم الحفظ بنجاح";
            RedirectToAction("AddCustomerService");

        }
    }
}

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

            ViewBag.serrr = services;

            var CustSer = new CustomerService();
            return View(CustSer);
        }

        [HttpPost]
        public async Task Save(CustomerService customerService, List<int> ServiceIds)
        {
            if (!ModelState.IsValid)
                TempData["message"] = "حدث خطأ أثناء حفظ البيانات";
               RedirectToAction("AddCustomerService");

            if (ServiceIds != null && ServiceIds.Any())
            {
                foreach (var serviceId in ServiceIds)
                {
                    var CustomerService = new CustomerService
                    {
                        CustomerId = customerService.CustomerId,
                        ServiceId = serviceId,
                    };

                    await dBContext.CustomerServices.AddAsync(CustomerService);
                    await dBContext.SaveChangesAsync();
                }
            }


            TempData["message"] = "تم الحفظ بنجاح";
            RedirectToAction("AddCustomerService");

        }

        [HttpGet("/Admin/CustomerServices/GetCustomerServices/{customerId}")]
        public async Task<IActionResult> GetCustomerServices(int customerId)
        {

            var services = await dBContext.CustomerServices.Where(cs => cs.CustomerId == customerId).Include(cs => cs.Service).ToListAsync();
            var actulSer = services.Select(s => s.Service).ToList();
            return Ok(actulSer);

        }
    }
}

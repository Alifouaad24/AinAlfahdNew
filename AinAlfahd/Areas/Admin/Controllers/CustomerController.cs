using AinAlfahd.BL;
using AinAlfahd.Data;
using AinAlfahd.Models;
using AinAlfahd.ModelsDTO;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController : Controller
    {
        private readonly ICustomer _customerService;
        MasterDBContext _dbContext;

        public CustomerController(ICustomer customerService, MasterDBContext dbContext)
        {
            _customerService = customerService;
            _dbContext = dbContext;
        }


        public IActionResult Search()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            var airCargoCustomers = await _dbContext.Customers
                .Include(c => c.CustomerServices)
                .ThenInclude(cs => cs.Service)
                .Where(c => c.CustomerServices.Any(cs => cs.Service.Description == "Air Shipping"))
                .Select(c => new 
                {
                    CustId = c.Id,
                    CustName = c.CustName,
                    CustMob = c.CustMob,
                    Services = c.CustomerServices
                                .Where(cs => cs.Service.Description == "Air Shipping")
                                .Select(cs => 
                                
                                     cs.Service.Description)
                                
                                .ToList()
                })
                .ToListAsync();

            return View(airCargoCustomers);
        }



        public async Task<IActionResult> AddCustomer(int? custId)
        {
            var cities = await _dbContext.Cities.ToListAsync();

            ViewBag.Cities = cities.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Description
            }).ToList();

            
            var areas = await _dbContext.Areas.ToListAsync();

            ViewBag.Areas = areas.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Description 
            }).ToList();

            var customer = await _customerService.GetAll();
            if (custId != null) 
            {
                var customer1 = await _customerService.GetByID(Convert.ToInt32(custId));
                return View(customer1);
            }

            return View(customer);
        }

        public async Task AddZero()
        {
            var customers = await _customerService.GetAll();
            foreach (var customer in customers)
            {
                if (customer.CustMob.StartsWith('0')) continue;

                customer.CustMob = '0' + customer.CustMob;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveData(CustomerDto dto)
        {
            bool result;
            try
            {
                if (ModelState.IsValid)
                {

                    var customer = new Customer
                    {
                        CustName = dto.CustName,
                        CustArea = dto.CustArea,
                        CustCity = dto.CustCity,
                        CustMob = dto.CustMob,
                        CustLandmark = dto.CustLandmark,
                        CustStatus = dto.CustStatus,
                        CustMob2 = dto.CustMob2,
                        Fbid = dto.Fbid,
                        CustProfile = dto.CustProfile,
                        FullPackage = dto.FullPackage,
                        Gisurl = dto.Gisurl,
                        InsertDt = DateOnly.FromDateTime(DateTime.Now),
                        Lat = dto.Lat,
                        Lon = dto.Lon,
                        Hexcode = dto.Hexcode,
 
                    };
                    if (dto.Id == 0)
                    {
                       result = await _customerService.AddData(customer);
                        var Cust = await _dbContext.Customers.FirstOrDefaultAsync(s => s.CustName == dto.CustName && s.CustMob == dto.CustMob);
                        var service = await _dbContext.Services.FirstOrDefaultAsync(s => s.Description == "Air Shipping");
                        if (result)
                        {
                            var cusSer = new CustomerService
                            {
                                CustomerId = Cust.Id,
                                ServiceId = service.Id,
                            };

                            await _dbContext.CustomerServices.AddAsync(cusSer);
                            await _dbContext.SaveChangesAsync();
                        }

                        TempData["Message"] = "تم حفظ العميل بنجاح!";
                    }
                    else
                    {
                        var cust = await _customerService.GetByID(dto.Id);

                        cust.CustName = dto.CustName;
                        cust.CustArea = dto.CustArea;
                        cust.CustCity = dto.CustCity;
                        cust.CustMob = dto.CustMob;
                        cust.CustLandmark = dto.CustLandmark;
                        cust.CustStatus = dto.CustStatus;
                        cust.CustMob2 = dto.CustMob2;
                        cust.Fbid = dto.Fbid;
                        cust.CustProfile = dto.CustProfile;
                        cust.FullPackage = dto.FullPackage;
                        cust.Gisurl = dto.Gisurl;
                        cust.InsertDt = DateOnly.FromDateTime(DateTime.Now);
                        cust.Lat = dto.Lat;
                        cust.Lon = dto.Lon;
                        cust.Hexcode = dto.Hexcode;


                         result = await _customerService.UpdateData(cust);
                        TempData["Message"] = "تم تعديل العميل بنجاح!";
                    }


                   


                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = "حدث خطأ أثناء حفظ بيانات العميل";
                    return View("AddCustomer", dto.Id);
                    
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.Substring(0,20);
            }
            return View("AddCustomer");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateData(Customer dto)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var customer = new Customer
                    {
                        CustName = dto.CustName,
                        CustArea = dto.CustArea,
                        CustCity = dto.CustCity,
                        CustMob = dto.CustMob,
                        CustLandmark = dto.CustLandmark,
                        CustStatus = dto.CustStatus,
                        CustMob2 = dto.CustMob2,
                        Fbid = dto.Fbid,
                        CustProfile = dto.CustProfile,
                        FullPackage = dto.FullPackage,
                        Gisurl = dto.Gisurl,
                        InsertDt = DateOnly.FromDateTime(DateTime.Now),
                        Lat = dto.Lat,
                        Lon = dto.Lon,
                        Hexcode = dto.Hexcode,

                    };
                    var result = await _customerService.AddData(customer);

                    var Cust = await _dbContext.Customers.FirstOrDefaultAsync(s => s.CustName == dto.CustName && s.CustMob == dto.CustMob);
                    var service = await _dbContext.Services.FirstOrDefaultAsync(s => s.Description == "Air Shipping");
                    if (result)
                    {
                        var cusSer = new CustomerService
                        {
                            CustomerId = Cust.Id,
                            ServiceId = service.Id,
                        };

                        _dbContext.CustomerServices.Update(cusSer);
                        await _dbContext.SaveChangesAsync();
                    }

                    TempData["Message"] = "تم حفظ العميل بنجاح!";
                    return RedirectToAction("AddCustomer");
                }
                else
                {
                    TempData["Message"] = "حدث خطأ أثناء حفظ بيانات العميل";
                    return View("AddCustomer");

                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message.Substring(0, 20);
            }
            return View("AddCustomer");
        }
    }
}

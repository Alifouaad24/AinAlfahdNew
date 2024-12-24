using AinAlfahd.Models.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AinAlfahd.Controllers
{
    public class AccountsController : Controller
    {
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LogInModel model)
        {

            if (model.Email == "Saif@Gmail.com" && model.Password == "123456")
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            TempData["ErrorMessage"] = "البريد الإلكتروني أو كلمة المرور غير صحيحة. يرجى المحاولة مرة أخرى.";
            return RedirectToAction("LogIn", "Accounts");

        }

       
    }
}

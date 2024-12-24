using AinAlfahd.Models.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AinAlfahd.ControllersApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsAPIController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login(LogInModel model)
        {

            if (model.Email == "Saif@saif.com" && model.Password == "123456")
            {
                return Ok();
            }

            return BadRequest();

        }
    }
}

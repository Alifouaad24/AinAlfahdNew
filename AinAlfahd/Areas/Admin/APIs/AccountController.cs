using AinAlfahd.Authontocation;
using AinAlfahd.HelpModels;
using AinAlfahd.Models.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AinAlfahd.Areas.Admin.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> roleManager;
        CreateJWT jWT;

        public AccountController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, CreateJWT jWT)
        {
            _userManager = userManager;
            this.roleManager = roleManager;
            this.jWT = jWT;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new IdentityUser
            {
                Email = model.Email,
                UserName = model.FirstName + '-' + model.LastName,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                var role = await _userManager.GetRolesAsync(user);

                var token = jWT.GenerateToken(user.Id, user.UserName, role[0]);
                return Ok(new { token = token });

            }

            return Ok(result);
        }

        [HttpPost("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new IdentityUser
            {
                Email = model.Email,
                UserName = model.FirstName + "-" + model.LastName
            };

            var existAdmin = await _userManager.FindByEmailAsync(user.Email);
            if (existAdmin == null)
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                    return BadRequest(result.Errors);

                // تأكد من وجود الدور Admin
                if (!await roleManager.RoleExistsAsync("Admin"))
                    await roleManager.CreateAsync(new IdentityRole("Admin"));

                await _userManager.AddToRoleAsync(user, "Admin");

                return Ok(new { message = "تم إنشاء الأدمن بنجاح" });
            }

            return Conflict(new { message = "الأدمن موجود مسبقًا" });

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return Ok(new
                {
                    message = "user deleted successfuly"
                });
            }
            else
            {
                return Ok(new
                {
                    message = "Some error happend please try again"
                });
            }

        }


        [HttpGet("GetAdmins")]
        public async Task<IActionResult> GetAdmins()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            return Ok(admins);
        }


        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] LogInModelNew model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _userManager.CheckPasswordAsync(user, model.Password);
                if(result == true)
                {
                    var role = await _userManager.GetRolesAsync(user);
                    var token = jWT.GenerateToken(user.Id, user.UserName, role[0]);
                    return Ok(new { token  = token });
                }

                return Unauthorized();
            }
            return NotFound();  
        }

        [HttpPost("AddRoleToUser/{id}")]
        public async Task<IActionResult> AddRoleToUser(string id, [FromBody] RoleDto model)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, model.RoleName);
                return Ok(new
                {
                    msg = "Role added successfuly"
                });
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole([FromBody] RoleDto model)
        {
            var result = await roleManager.CreateAsync(new IdentityRole { Name = model.RoleName});

            if (result.Succeeded)
            {
                return Ok(new
                {
                    msg = "Role added successfuly"
                });
            }

            return BadRequest();

        }
    }
}

using AinAlfahd.Authontocation;
using AinAlfahd.HelpModels;
using AinAlfahd.Models.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

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

                if (!await roleManager.RoleExistsAsync("User"))
                    await roleManager.CreateAsync(new IdentityRole("User"));
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
            var roles = new[] { "Admin", "AinAlFhd_Center", "Sub_Admin" };
            var allAdmins = new List<(IdentityUser User, string Role)>();

            foreach (var role in roles)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role);
                foreach (var user in usersInRole)
                {
                    allAdmins.Add((user, role));
                }
            }

            var uniqueAdmins = allAdmins
                .GroupBy(x => x.User.Id)
                .Select(g => new
                {
                    Id = g.Key,
                    Email = g.First().User.Email,
                    UserName = g.First().User.UserName,
                    Role = string.Join(", ", g.Select(x => x.Role).Distinct())
                })
                .ToList();

            return Ok(uniqueAdmins);
        }



        [HttpGet("{name}")]
        public async Task<IActionResult> GetCurrentUser(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            return Ok(user);
        }

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await roleManager.Roles.ToListAsync();
            return Ok(roles);
        }

        [HttpPost("AddRoleForSys")]
        public async Task<IActionResult> AddRoleForSys([FromBody] string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return BadRequest("اسم الرول لا يمكن أن يكون فارغًا.");

            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (roleExists)
                return BadRequest("الرول موجود بالفعل.");

            var result = await roleManager.CreateAsync(new IdentityRole(roleName));

            if (result.Succeeded)
                return Ok("تمت إضافة الرول بنجاح.");
            else
                return BadRequest(result.Errors);
        }

        [HttpPost("RegisterUserWithRole")]
        public async Task<IActionResult> RegisterUserWithRole([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new IdentityUser
            {
                Email = model.Email,
                UserName = model.FirstName + '-' + model.LastName,
            };

            var createResult = await _userManager.CreateAsync(user, model.Password);

            if (!createResult.Succeeded)
                return BadRequest(createResult.Errors);

            var roleExists = await roleManager.RoleExistsAsync(model.Role);
            if (!roleExists)
                return BadRequest($"الرول '{model.Role}' غير موجود.");

            var addToRoleResult = await _userManager.AddToRoleAsync(user, model.Role);

            if (!addToRoleResult.Succeeded)
                return BadRequest(addToRoleResult.Errors);

            return Ok(new
            {
                message = "تم إنشاء المستخدم وإسناد الصلاحية بنجاح."
            });
        }




        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] LogInModelNew model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var normalizedEmail = _userManager.NormalizeEmail(model.Email);

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);
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
        [Authorize]
        [HttpGet("check-token")]
        public IActionResult CheckToken()
        {
            return Ok(new { message = "Token is valid", user = User.Identity.Name });
        }
    }
}

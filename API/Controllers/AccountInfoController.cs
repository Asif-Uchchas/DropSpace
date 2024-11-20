using DropSpace.Areas.Auth.Models;
using DropSpace.Data.Entity;
using DropSpace.ERPService.AuthService.Interfaces;
using DropSpace.ERPServices.AuthService.Interfaces;
using DropSpace.Helpers.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DropSpace.API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class AccountInfoController : ControllerBase
    {
        private readonly IUserInfoes userInfoes;
        private readonly IDbChangeService dbChangeService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountInfoController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserInfoes userInfoes, IDbChangeService dbChangeService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.userInfoes = userInfoes;
            this.dbChangeService = dbChangeService;
        }
        //GET :api/AccountInfo/LogIn
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userInfos = await userInfoes.GetUserInfoByUser(model.Name);
            var user = await _userManager.FindByNameAsync(model.Name);



            if (user != null && (await _userManager.CheckPasswordAsync(user, model.Password)))
            {
                var roles = await _userManager.GetRolesAsync(user);
                string role = roles.FirstOrDefault();


                var obj = new ReturnObject
                {
                    userInfo = userInfos,
                    role = role
                };

                return new OkObjectResult(obj);
            }

            return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
        }

        public class ReturnObject
        {
            public string? message { get; set; }
            public object? userInfo { get; set; }
            public string? role { get; set; }
        }
    }
}

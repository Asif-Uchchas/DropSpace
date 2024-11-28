using DropSpace.Areas.Auth.Models;
using DropSpace.Areas.Home.Models;
using DropSpace.Data.Entity;
using DropSpace.Data.Entity.LogInfo;
using DropSpace.Data.Entity.MasterData;
using DropSpace.ERPService.AuthService.Interfaces;
using DropSpace.ERPServices.MobilePhoneValidation;
using DropSpace.ERPServices.MobilePhoneValidation.Interfaces;
using DropSpace.ERPServices.PersonData.Interfaces;
using DropSpace.Helpers;
using DropSpace.Repository.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using System.Drawing;
using System.Net;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DropSpace.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMobilePhoneValidation _mobilePhoneValidation;
        private readonly IUserInfoes userInfoes;
        private readonly IPersonData _persondata;

        private const string FIXED_OTP = "123456";
        private static readonly Dictionary<string, string> _otpStorage = new Dictionary<string, string>();
        private static readonly Regex _mobileRegex = new Regex(@"^01[3-9]\d{8}$");
        public AccountController(UserManager<ApplicationUser> userManager, IPersonData persondata, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IUserInfoes userInfoes, IMobilePhoneValidation mobilePhoneValidation
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mobilePhoneValidation = mobilePhoneValidation;
            _persondata = persondata;
            this.userInfoes = userInfoes;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userInfos = await userInfoes.GetUserInfoByUser(model.Name);
            if (userInfos == null || userInfos.isActive != 1)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or account inactive.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }


                return RedirectToAction("Dashboard", "DashBoard", new { area = "Dboard" });
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Account is locked.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> IsTheSamePerson(string mobile, string otp)
        {
            // Check if the mobile number is provided
            if (!string.IsNullOrWhiteSpace(mobile))
            {
                // Validate the OTP against the last requested OTP for the mobile number
                bool isValidOtp = await _persondata.IsTheSamePerson(IdMasking.Decode(mobile), IdMasking.Decode(otp));

                if (isValidOtp)
                {
                    // Return a success response if the OTP is valid
                    return Json(new { success = true, message = "OTP is valid." });
                }
                else
                {
                    // Return a response indicating the OTP is invalid
                    return Json(new { success = false, message = "Invalid OTP." });
                }
            }

            // Return a response indicating the mobile number is not provided
            return Json(new { success = false, message = "Mobile number is required." });
        }

        [AllowAnonymous]
        public async Task<IActionResult> MobileLogin(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateNewUser(UserInformationModel model)
        //{
        //    string? response = "";
        //    int empId = 0;
        //    var user = new ApplicationUser { UserName = model.userId, isActive = 1, Email = model.email, bpNo = model.bpNo.Replace("BP","").Replace("CIV",""), PhoneNumber = model.mobile };
        //    var result = await _userManager.CreateAsync(user, model.password);
        //    if (result.Succeeded)
        //    {
        //        var roleAssign = await _userManager.AddToRoleAsync(user, model.roleId);
        //        //userInfoes.DeleteUserInfoBeforeRegister(model.UserName);
        //        var emp = new EmployeeInfo
        //        {
        //            nameBangla = model.empNameBn,
        //            nameEnglish = model.empName,
        //            employeeCode = model.userId,
        //            emailAddressPersonal = model.email,
        //            mobileNumberPersonal = model.mobile,
        //            designation = model.designationName,
        //            ApplicationUserId = user.Id,
        //            freedomFighter = false,
        //            designationCheck = 1,
        //            rankId = model.rankId,
        //            branchId = model.branchId,
        //            sectionId=model.sectionId,
        //        };
        //        empId = await _userInfoServices.SaveEmployeeInformation(emp);
        //        response = "success";
        //        if (model.imgUrl != null)
        //        {
        //            var baseImge = "";
        //            if (model.imgUrl.Contains("data:image/"))
        //            {

        //                if (model.imgUrl.Contains("data:image/png"))
        //                {
        //                    baseImge = model.imgUrl.Substring?(22);
        //                }
        //                else
        //                {
        //                    baseImge = model.imgUrl.Substring?(23);
        //                }
        //            }
        //            else
        //            {
        //                baseImge = model.imgUrl;
        //            }

        //            if (model.imgUrl != null)
        //            {
        //                int photoId = 0;
        //                //if (empId > 0)
        //                //{
        //                //    var photoInfo = await photographService.GetPhotographByType(empId, "profile");
        //                //    if (photoInfo != null)
        //                //    {
        //                //        photoId = photoInfo.Id;
        //                //    }
        //                //}

        //                byte[] bytes = Convert.FromBase64string?(baseImge);
        //                Image image;
        //                using (MemoryStream ms = new MemoryStream(bytes))
        //                {
        //                    image = Image.FromStream(ms);
        //                }
        //                var randomFileName = Guid.NewGuid().Tostring?().Substring?(0, 8) + ".jpeg";
        //                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EmployeePhoto", randomFileName);
        //                var size = new Size(300, 300);
        //                var i2 = new Bitmap(image, size);
        //                i2.Save(path, System.Drawing.Imaging.ImageFormat.Png);

        //                Photograph photograph = new Photograph
        //                {
        //                    Id = photoId,
        //                    employeeId = empId,
        //                    url = "\\EmployeePhoto\\" + randomFileName,
        //                    type = "profile"
        //                };
        //                await _userInfoServices.SavePhotograph(photograph);

        //            }
        //            else
        //            {
        //                response = "error";
        //            }


        //        }

        //        //return RedirectToAction("UserList","Account",new { area="Auth"});
        //    }
        //    return Json(response);
        //}

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> UserList()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            List<ApplicationRoleViewModel> lstRole = new List<ApplicationRoleViewModel>();
            foreach (var data in roles)
            {
                ApplicationRoleViewModel modelr = new ApplicationRoleViewModel
                {
                    RoleId = data.Id,
                    RoleName = data.Name
                };
                if (data.Name == "admin")
                {

                }
                else
                {
                    lstRole.Add(modelr);
                }
            }

            UserListViewModel model = new UserListViewModel
            {
                //aspNetUsersViewModels = await _userInfoServices.GetUserInfo(),
                userRoles = lstRole,
            };
            return View(model);
        }

        //[Authorize(Roles = "Super Admin")]
        [HttpGet]
        public async Task<IActionResult> UserRoleCreate()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            List<ApplicationRoleViewModel> lstRole = new List<ApplicationRoleViewModel>();
            foreach (var data in roles)
            {
                ApplicationRoleViewModel model = new ApplicationRoleViewModel
                {
                    RoleId = data.Id,
                    RoleName = data.Name
                };
                lstRole.Add(model);
            }
            ApplicationRoleViewModel viewModel = new ApplicationRoleViewModel
            {
                roleViewModels = lstRole
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UserIdentityRoleCreate([FromForm] ApplicationRoleViewModel model)
        {
            var user = new ApplicationRole(model.RoleName);
            IdentityResult result = await _roleManager.CreateAsync(user);

            return RedirectToAction(nameof(UserRoleCreate));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EditRole([FromForm] ApplicationRoleViewModel model)
        {

            ApplicationUser user = await _userManager.FindByNameAsync(model.EuserName);
            //var oldRoleId = _userManager.GetUsersInRoleAsync(model.userName);
            //var oldRoleName = _roleManager.Roles.SingleOrDefault(r => r.Id == oldRoleId).Name;
            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles.ToArray());

            if (model.PreRoleId != null)
            {
                for (int i = 0; i < model.roleIdList.Length; i++)
                {
                    if (!await _userManager.IsInRoleAsync(user, model.roleIdList[i]))
                    {
                        await _userManager.AddToRoleAsync(user, model.roleIdList[i]);
                    }
                }
                //await _userManager.RemoveFromRoleAsync(user, model.PreRoleId);
            }

            return RedirectToAction(nameof(UserList));
        }

        public async Task<IActionResult> DeleteRoles(string? id)
        {
            try
            {
                //await _userInfoServices.DeleteRoleById(id);
                return Json("Success");
            }
            catch
            {
                return Json("Roles is Already Assigned Someone!!");
            }
        }

        public async Task<IActionResult> AssaignRoleToUser()
        {
            string? userName = User.Identity.Name;
            var roles = await _roleManager.Roles.Where(x => x.Name != "Supper Admin").ToListAsync();
            List<ApplicationRoleViewModel> lstRole = new List<ApplicationRoleViewModel>();
            foreach (var data in roles)
            {
                ApplicationRoleViewModel rolesModel = new ApplicationRoleViewModel
                {
                    RoleId = data.Id,
                    RoleName = data.Name
                };
                lstRole.Add(rolesModel);
            }

            ApplicationRoleViewModel model = new ApplicationRoleViewModel
            {
                roleViewModels = lstRole,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssaignRoleToUser([FromForm] ApplicationRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            //return Json(model);
            var user = await _userManager.FindByNameAsync(model.userName);
            //await _userInfoServices.DeleteUserRoleListByUserId(user.Id);
            if (model.roleIdList.Count() > 0)
            {
                for (int i = 0; i < model.roleIdList.Count(); i++)
                {
                    await _userManager.AddToRoleAsync(user, model.roleIdList[i]);
                }
            }

            return RedirectToAction(nameof(AssaignRoleToUser));
        }

        //[AllowAnonymous]
        //[Route("api/Account/GetUserInfoList/")]
        //[HttpGet]
        //public async Task<IActionResult> GetUserInfoList()
        //{
        //    //var result = await _userInfoServices.GetUserInfo();
        //    return Json(result);
        //}

        //[AllowAnonymous]
        //[Route("api/Account/getUserRoles/{userId}")]
        //[HttpGet]
        //public async Task<IActionResult> GetUserInfoList(string? userId)
        //{
        //    //var result = await _userInfoServices.GetUserRoleList(userId);
        //    return Json(result);
        //}
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }

        //[HttpGet]
        //public async Task<JsonResult> GetSectionByUnit(int unitId)
        //{
        //    var data = await _unitOfWork.Sections.FindAll(q=>q.specialBranchUnitId==unitId);
        //    return Json(data.ToList());
        //}




        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePsswordViewModel model)
        {
            try
            {
                string? message = "Fail To Update Password";
                if (ModelState.IsValid)
                {
                    var data = await _userManager.ChangePasswordAsync(await _userManager.FindByNameAsync(HttpContext.User.Identity.Name), model.OldPassword, model.Password);
                    message = data.ToString();
                }
                //return RedirectToAction(nameof(PortfolioDashboardController.Index), "Portfolio", new { Message = message });
                return RedirectToAction("Index", "PortfolioDashboard", new { Area = "Portfolio" });
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        [Route("api/[area]/[controller]/[action]")]
        public async Task<IActionResult> SendOTP([FromBody] JsonElement request)
        {
            try
            {
                string mobileNumber = request.GetProperty("mobileNumber").GetString();

                if (string.IsNullOrEmpty(mobileNumber))
                {
                    return BadRequest(new { success = false, message = "Invalid mobile number format" });
                }


                // Generate a 6-digit OTP
                byte[] randomBytes = new byte[4];  // Use 4 bytes for a 6-digit OTP
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(randomBytes); // Fill the byte array with random bytes
                }

                // Convert the byte array to a 32-bit unsigned integer
                uint otpValue = BitConverter.ToUInt32(randomBytes, 0);

                // Ensure the OTP is always 6 digits by using modulus
                string otp = (otpValue % 1000000).ToString("D6");

                // Call the service method to log OTP
                bool otpSent = await _mobilePhoneValidation.SendOTP(mobileNumber, otp);

                if (!otpSent)
                {
                    return StatusCode(500, new { success = false, message = "An error occurred while sending OTP" });
                }

                return Ok(new { success = true, message = "OTP sent successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending OTP: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred while sending OTP" });
            }
        }

        [HttpPost]
        [Route("/api/[area]/[controller]/[action]")]
        public async Task<IActionResult> VerifyOTP([FromBody] JsonElement request)
        {
            try
            {
                string mobileNumber = request.GetProperty("mobileNumber").GetString();
                string otp = request.GetProperty("otp").GetString();

                if (string.IsNullOrEmpty(mobileNumber))
                {
                    return BadRequest(new { success = false, message = "Invalid mobile number format" });
                }

                if (string.IsNullOrEmpty(otp))
                {
                    return BadRequest(new { success = false, message = "OTP is required" });
                }

                // Call the service method to verify OTP
                bool otpVerified = await _mobilePhoneValidation.VerifyOTP(mobileNumber, otp);

                if (!otpVerified)
                {
                    return BadRequest(new { success = false, message = "Invalid OTP or OTP has expired" });
                }

                //return Ok(new { success = true, message = "OTP verified successfully" });
                return Json(new { success = true, message = "OTP verified successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying OTP: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred while verifying OTP" });
            }
        }
    }
    
}

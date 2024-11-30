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

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("api/[area]/[controller]/[action]")]
        public async Task<IActionResult> Login([FromBody] MasterDataViewModel model)
        {
            var userInfos = await userInfoes.GetUserInfoByUser(model.mId);
            var lastOtp = await _mobilePhoneValidation.GetUserLastOtp(model.mId);
            var obj = new ReturnObject
            {

            };
            if (userInfos == null || userInfos.isActive != 1)
            {
                obj.status = "noUser";
                return Json(obj);
            }
            if (lastOtp == model.otp)
            {
                await _signInManager.SignInAsync(userInfos, true);
                obj.status = "success";
                obj.mobileMask = IdMasking.Encode(userInfos.UserName);
                obj.otpMask = IdMasking.Encode(lastOtp);
                return Json(obj);
            }
            else
            {
                obj.status = "lastOtpNotMatch";
                return Json(obj);
            }
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

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home", new { Area = "Home" });
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
                return Json(new { success = true, message = "OTP verified successfully",otpEn=IdMasking.Encode(otp) });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying OTP: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred while verifying OTP" });
            }
        }

        public class ReturnObject
        {
            public string status { get; set; }
            public string mobileMask { get; set; }
            public string otpMask { get; set; }
        }
    }
    
}

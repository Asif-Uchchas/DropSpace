using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace DropSpace.API
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        private const string FIXED_OTP = "123456";
        private static readonly Dictionary<string, string> _otpStorage = new Dictionary<string, string>();
        private static readonly Regex _mobileRegex = new Regex(@"^01[3-9]\d{8}$");


        [HttpPost]
        [Route("/api/sendOTP")]
        public IActionResult SendOTP([FromBody] JsonElement request)
        {
            try
            {
                string mobileNumber = request.GetProperty("mobileNumber").GetString();

                if (string.IsNullOrEmpty(mobileNumber) || !_mobileRegex.IsMatch(mobileNumber))
                {
                    return BadRequest(new { success = false, message = "Invalid mobile number format" });
                }

                // Store the fixed OTP for this mobile number
                _otpStorage[mobileNumber] = FIXED_OTP;

                // Log for debugging (remove in production)
                Console.WriteLine($"OTP sent to {mobileNumber}: {FIXED_OTP}");

                return Ok(new { success = true, message = "OTP sent successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending OTP: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred while sending OTP" });
            }
        }

        [HttpPost]
        [Route("/api/verifyOTP")]
        public IActionResult VerifyOTP([FromBody] JsonElement request)
        {
            try
            {
                string mobileNumber = request.GetProperty("mobileNumber").GetString();
                string otp = request.GetProperty("otp").GetString();

                if (string.IsNullOrEmpty(mobileNumber) || !_mobileRegex.IsMatch(mobileNumber))
                {
                    return BadRequest(new { success = false, message = "Invalid mobile number format" });
                }

                if (string.IsNullOrEmpty(otp))
                {
                    return BadRequest(new { success = false, message = "OTP is required" });
                }

                // For the dummy implementation, just check if the OTP matches our fixed value
                if (otp == FIXED_OTP)
                {
                    return Ok(new { success = true, message = "OTP verified successfully" });
                }

                return BadRequest(new { success = false, message = "Invalid OTP" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying OTP: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred while verifying OTP" });
            }
        }
    }
}
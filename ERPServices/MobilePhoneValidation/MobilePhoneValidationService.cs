using DropSpace.Context;
using DropSpace.Data.Entity.LogInfo;
using DropSpace.ERPServices.MobilePhoneValidation.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DropSpace.ERPServices.MobilePhoneValidation
{
    public class MobilePhoneValidationService : IMobilePhoneValidation
    {
        private readonly DropSpaceDbContext _context;

        public MobilePhoneValidationService(DropSpaceDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SendOTP(string mobileNumber, string otp)
        {
            try
            {
                // Validate mobile number format if necessary
                if (string.IsNullOrEmpty(mobileNumber) || mobileNumber.Length != 11) // Example validation for a 10-digit number
                {
                    return false;
                }

                // Set the OTP expiry time (for example, 5 minutes)
                DateTime otpExpire = DateTime.Now.AddMinutes(10);

                // Log OTP details in the database
                var otpLog = new OTPLogs
                {
                    userName = mobileNumber,
                    otp = otp,
                    otpExpire = otpExpire,
                    isVerified = false // Initially false
                };

                // Add OTP log to the database
                _context.oTPLogs.Add(otpLog);
                _context.SaveChanges();

                // Return success
                return true;
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }

        // Method to verify OTP
        public async Task<bool> VerifyOTP(string mobileNumber, string otp)
        {
            try
            {
                // Retrieve the OTP log from the database for the given mobile number
                var otpLog = _context.oTPLogs
                    .FirstOrDefault(log => log.userName == mobileNumber && log.otp == otp && log.isVerified==false && log.otpExpire > DateTime.Now);

                if (otpLog == null)
                {
                    return false; // OTP not found or expired
                }

                // OTP is valid, update verification status
                otpLog.isVerified = true;
                _context.SaveChanges();

                return true; // OTP successfully verified
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }
        public async Task<string> GetUserOtp(string mobileNumber)
        {
            var otp = await _context.oTPLogs.Where(x => x.userName == mobileNumber && x.isVerified == true && x.otpExpire.Value.Date == DateTime.Now.Date).OrderBy(x => x.Id).LastOrDefaultAsync();
            return otp.otp;
        }
        public async Task<string> GetUserLastOtp(string mobileNumber)
        {
            var otp = await _context.oTPLogs.Where(x => x.userName == mobileNumber).OrderBy(x => x.Id).LastOrDefaultAsync();
            return otp.otp;
        }
    }
}

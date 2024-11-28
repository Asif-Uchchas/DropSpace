namespace DropSpace.ERPServices.MobilePhoneValidation.Interfaces
{
    public interface IMobilePhoneValidation
    {
        Task<bool> SendOTP(string mobileNumber, string otp);
        Task<bool> VerifyOTP(string mobileNumber, string otp);
        Task<string> GetUserOtp(string mobileNumber);
    }
}

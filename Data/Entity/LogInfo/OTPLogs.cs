namespace DropSpace.Data.Entity.LogInfo
{
    public class OTPLogs:Base
    {
        public string? userName { get; set; }
        public string? otp { get; set; }
        public DateTime? otpExpire { get; set; }
        public bool isVerified { get; set; } = false;

    }
}

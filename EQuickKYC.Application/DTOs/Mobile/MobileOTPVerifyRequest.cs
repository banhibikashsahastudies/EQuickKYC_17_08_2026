namespace EQuickKYC.Application.DTOs.Mobile
{
    public record MobileOTPVerifyRequest
    {
        public string Mobile { get; set; }
        public string OTP { get; set; }
    }
}

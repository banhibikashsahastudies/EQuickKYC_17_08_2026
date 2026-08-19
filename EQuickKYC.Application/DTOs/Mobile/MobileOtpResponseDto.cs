namespace EQuickKYC.Application.DTOs.Mobile
{
    public record MobileOtpResponseDto
    {
        public string Mobile { get; set; }
        public string OTP { get; set; }
    }
}

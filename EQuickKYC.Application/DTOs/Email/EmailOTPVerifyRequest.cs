namespace EQuickKYC.Application.DTOs.Email
{
    public record EmailOTPVerifyRequest
    {
        public string Email { get; set; }
        public string OTP { get; set; }
    }
}

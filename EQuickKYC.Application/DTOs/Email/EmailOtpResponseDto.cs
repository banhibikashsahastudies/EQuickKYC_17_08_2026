namespace EQuickKYC.Application.DTOs.Email
{
    public record EmailOtpResponseDto
    {
        public string Email { get; set; }
        public string OTP { get; set; }
    }
}

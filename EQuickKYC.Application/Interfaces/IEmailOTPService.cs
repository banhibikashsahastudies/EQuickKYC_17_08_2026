using EQuickKYC.Application.DTOs.Email;

namespace EQuickKYC.Application.Interfaces
{
    public interface IEmailOTPService
    {
        Task<EmailOtpResponseDto> SendEmailOTPAsync(string email);
        Task<EmailOtpResponseDto> VerifyEmailOTPAsync(string email, string otp, Guid userMasterId);
    }
}

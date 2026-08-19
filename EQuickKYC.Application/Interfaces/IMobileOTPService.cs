using EQuickKYC.Application.DTOs.Mobile;

namespace EQuickKYC.Application.Interfaces
{
    public interface IMobileOTPService
    {
        Task<MobileOtpResponseDto> SendMobileOTPAsync(string mobile);
        Task<RegistrationResponseDto> VerifyMobileOTPAsync(string mobile, string otp);
    }
}

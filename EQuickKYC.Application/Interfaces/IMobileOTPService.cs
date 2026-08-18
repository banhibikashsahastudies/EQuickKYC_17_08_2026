namespace EQuickKYC.Application.Interfaces
{
    public interface IMobileOTPService
    {
        Task<bool> SendMobileOTPAsync(string mobile);
        Task<bool> VerifyMobileOTPAsync(string mobile, string otp);
    }
}

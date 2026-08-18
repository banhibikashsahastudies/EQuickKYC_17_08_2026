namespace EQuickKYC.Application.Interfaces
{
    public interface IEmailOTPService
    {
        Task<bool> SendEmailOTPAsync(string email, string otp);
        Task<bool> VerifyEmailOTPAsync(string email, string otp);
        Task<bool> SaveEmailOTPAsync();
    }
}

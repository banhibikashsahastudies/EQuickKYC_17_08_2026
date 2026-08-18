namespace EQuickKYC.Application.Interfaces
{
    public interface IEmailOTPService
    {
        Task<bool> SendEmailOTPAsync(string email);
        Task<bool> VerifyEmailOTPAsync(string email, string otp);
    }
}

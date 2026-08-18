using EQuickKYC.Application.Common;
using EQuickKYC.Application.DTOs.Email;
using EQuickKYC.Application.Interfaces;

namespace EQuickKYC.Application.Service
{
    public class EmailOTPServiceApplication
    {
        private readonly IEmailOTPService _emailOTPService;
        public EmailOTPServiceApplication(IEmailOTPService emailOTPService)
        {
            _emailOTPService = emailOTPService;
        }
        public async Task<Result<bool>> SendEmailOTP(EmailOTPRequestDto emailRequestdto)
        {
            if (string.IsNullOrEmpty(emailRequestdto.Email))
            {
                return Result<bool>.Fail("Email is required.");
            }
            var result = await _emailOTPService.SendEmailOTPAsync(emailRequestdto.Email);

            if (!result) return Result<bool>.Fail("Email OTP already verified.");

            return Result<bool>.Ok(true, "Email OTP sent successfully.");
        }
        public async Task<Result<bool>> VerifyEmailOTP(EmailOTPVerifyRequest emailVerifyDto)
        {
            if (!string.IsNullOrEmpty(emailVerifyDto.Email) && !string.IsNullOrEmpty(emailVerifyDto.OTP))
            {
                var result = await _emailOTPService.VerifyEmailOTPAsync(emailVerifyDto.Email, emailVerifyDto.OTP);
                if (!result)
                {
                    return Result<bool>.Fail("Email already verified.");
                }
                return Result<bool>.Ok(true, "Email OTP verified successfully.");
            }
            return Result<bool>.Fail("Invalid email or OTP.");
        }
    }
}

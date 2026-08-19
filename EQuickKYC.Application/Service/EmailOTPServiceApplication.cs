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
        public async Task<Result<EmailOtpResponseDto>> SendEmailOTP(EmailOTPRequestDto emailRequestdto)
        {
            if (string.IsNullOrEmpty(emailRequestdto.Email))
            {
                return Result<EmailOtpResponseDto>.Fail("Email is required.");
            }
            var result = await _emailOTPService.SendEmailOTPAsync(emailRequestdto.Email);

            if (result == null) return Result<EmailOtpResponseDto>.Fail("Email already verified.");

            return Result<EmailOtpResponseDto>.Ok(result, "Email OTP sent successfully.");
        }
        public async Task<Result<EmailOtpResponseDto>> VerifyEmailOTP(EmailOTPVerifyRequest emailVerifyDto)
        {
            if (!string.IsNullOrEmpty(emailVerifyDto.Email) && !string.IsNullOrEmpty(emailVerifyDto.OTP))
            {
                var result = await _emailOTPService.VerifyEmailOTPAsync(emailVerifyDto.Email, emailVerifyDto.OTP, emailVerifyDto.UserMasterId);
                if (result == null)
                {
                    return Result<EmailOtpResponseDto>.Fail("Email already verified.");
                }
                return Result<EmailOtpResponseDto>.Ok(result, "Email OTP verified successfully.");
            }
            return Result<EmailOtpResponseDto>.Fail("Invalid email or OTP.");
        }
    }
}

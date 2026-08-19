using EQuickKYC.Application.Common;
using EQuickKYC.Application.DTOs.Mobile;
using EQuickKYC.Application.Interfaces;

namespace EQuickKYC.Application.Service
{
    public class MobileOTPService
    {
        private readonly IMobileOTPService _mobileOTPService;

        public MobileOTPService(IMobileOTPService mobileOTPService)
        {
            _mobileOTPService = mobileOTPService;
        }
        public async Task<Result<MobileOtpResponseDto>> SendMobileOTP(MobileOTPRequestDto mobileRequestDto)
        {
            if (string.IsNullOrEmpty(mobileRequestDto.Mobile))
            {
                return Result<MobileOtpResponseDto>.Fail("Mobile number is required.");
            }
            var result = await _mobileOTPService.SendMobileOTPAsync(mobileRequestDto.Mobile);

            if (result == null) return Result<MobileOtpResponseDto>.Fail("Mobile OTP already verified.");

            return Result<MobileOtpResponseDto>.Ok(result, "Mobile OTP sent successfully.");
        }

        public async Task<Result<RegistrationResponseDto>> VerifyMobileOTP(MobileOTPVerifyRequest mobileVerifyDto)
        {
            if (!string.IsNullOrEmpty(mobileVerifyDto.Mobile) && !string.IsNullOrEmpty(mobileVerifyDto.OTP))
            {
                var result = await _mobileOTPService.VerifyMobileOTPAsync(mobileVerifyDto.Mobile, mobileVerifyDto.OTP);

                if (result == null)
                {
                    return Result<RegistrationResponseDto>.Fail("Mobile number or OTP did not match.");
                }
                return Result<RegistrationResponseDto>.Ok(result, "Mobile OTP verified successfully.");
            }
            return Result<RegistrationResponseDto>.Fail("Invalid mobile number or OTP.");
        }
    }
}

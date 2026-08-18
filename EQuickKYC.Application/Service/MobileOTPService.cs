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
        public async Task<Result<bool>> SendMobileOTP(MobileOTPRequestDto mobileRequestDto)
        {
            if (string.IsNullOrEmpty(mobileRequestDto.Mobile))
            {
                return Result<bool>.Fail("Mobile number is required.");
            }
            var result = await _mobileOTPService.SendMobileOTPAsync(mobileRequestDto.Mobile);

            if (!result) return Result<bool>.Fail("Mobile OTP already verified.");

            return Result<bool>.Ok(true, "Mobile OTP sent successfully.");
        }

        public async Task<Result<bool>> VerifyMobileOTP(MobileOTPVerifyRequest mobileVerifyDto
            )
        {
            if (!string.IsNullOrEmpty(mobileVerifyDto.Mobile) && !string.IsNullOrEmpty(mobileVerifyDto.OTP))
            {
                var result = await _mobileOTPService.VerifyMobileOTPAsync(mobileVerifyDto.Mobile, mobileVerifyDto.OTP);

                if (!result)
                {
                    return Result<bool>.Fail("Already verified.");
                }
                return Result<bool>.Ok(true, "Mobile OTP verified successfully.");
            }
            return Result<bool>.Fail("Invalid mobile number or OTP.");
        }
    }
}

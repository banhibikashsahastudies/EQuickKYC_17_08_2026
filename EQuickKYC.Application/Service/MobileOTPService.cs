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
        public async Task<Result<bool>> SendMobileOTP(MobileOTPRequestDto dto)
        {
            if (string.IsNullOrEmpty(dto.Mobile))
            {
                return Result<bool>.Fail("Mobile number is required.");
            }
            var result = await _mobileOTPService.SendMobileOTPAsync(dto.Mobile);

            if (!result) return Result<bool>.Fail("Mobile OTP already verified.");

            return Result<bool>.Ok(true, "Mobile OTP sent successfully.");
        }

        public async Task<Result<bool>> VerifyMobileOTP(MobileOTPVerifyRequest mobileOTP)
        {
            if (!string.IsNullOrEmpty(mobileOTP.Mobile) && !string.IsNullOrEmpty(mobileOTP.OTP))
            {
                var result = await _mobileOTPService.VerifyMobileOTPAsync(mobileOTP.Mobile, mobileOTP.OTP);
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

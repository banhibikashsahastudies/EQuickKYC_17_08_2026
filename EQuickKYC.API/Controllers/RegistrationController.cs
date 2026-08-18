using EQuickKYC.Application.DTOs.Mobile;
using EQuickKYC.Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace EQuickKYC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly MobileOTPService _mobileOTPService;

        public RegistrationController(MobileOTPService mobileOTPService)
        {
            _mobileOTPService = mobileOTPService;
        }
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendMobileOTP([FromBody] MobileOTPRequestDto dto)
        {
            if (dto.Mobile == null)
            {
                return BadRequest("Mobile number is required.");
            }

            var result = await _mobileOTPService.SendMobileOTP(dto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("verify")]
        public async Task<IActionResult> VerifyMobileOTP([FromBody] MobileOTPVerifyRequest dto)
        {
            if (dto.Mobile == null)
            {
                return BadRequest("Mobile number is required.");
            }

            var result = await _mobileOTPService.VerifyMobileOTP(dto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}

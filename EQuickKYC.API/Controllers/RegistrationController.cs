using EQuickKYC.Application.DTOs.Email;
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
        private readonly EmailOTPServiceApplication _emailOTPService;

        public RegistrationController(MobileOTPService mobileOTPService, EmailOTPServiceApplication emailOTPService)
        {
            _mobileOTPService = mobileOTPService;
            _emailOTPService = emailOTPService;
        }
        [HttpPost("send-mobile-otp")]
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
        [HttpPost("verify-mobile-otp")]
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

        [HttpPost("send-email-otp")]
        public async Task<IActionResult> SendEmailOTP([FromBody] EmailOTPRequestDto dto)
        {
            if (dto.Email == null)
            {
                return BadRequest("Email is required.");
            }

            var result = await _emailOTPService.SendEmailOTP(dto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("verify-email-otp")]
        public async Task<IActionResult> VerifyEmailOTP([FromBody] EmailOTPVerifyRequest dto)
        {
            if (dto.Email == null)
            {
                return BadRequest("Email is required.");
            }

            var result = await _emailOTPService.VerifyEmailOTP(dto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}

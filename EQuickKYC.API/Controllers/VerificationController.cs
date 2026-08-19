using EQuickKYC.Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace EQuickKYC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerificationController : ControllerBase
    {
        #region Services
        private readonly PanService _panService;

        public VerificationController(PanService panService)
        {
            _panService = panService;
        }
        #endregion

        #region PAN Verification for Admin
        [HttpGet("pan-verification")]
        public async Task<ActionResult> Get([FromQuery] string panNumber)
        {
            if (string.IsNullOrWhiteSpace(panNumber))
            {
                return BadRequest("PAN number is required.");
            }
            var result = await _panService.GetPanDetailsByPannumber(panNumber);
            return Ok(result);
        }
        #endregion
    }
}

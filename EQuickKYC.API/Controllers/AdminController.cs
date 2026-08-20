using EQuickKYC.Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace EQuickKYC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ClientAdminService _clientApiService;

        public AdminController(ClientAdminService clientApiService)
        {
            _clientApiService = clientApiService;
        }

        [HttpGet("get-api-error-log")]
        public async Task<ActionResult> Get()
        {
            var result = await _clientApiService.GetApiError();
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}


using EQuickKYC.Application.DTOs.Bank;
using EQuickKYC.Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace EQuickKYC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankMasterController : ControllerBase
    {
        private readonly AppBankService _bankService;

        public BankMasterController(AppBankService bankService)
        {
            _bankService = bankService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var result = await _bankService.GetAllBanks();
            if (result == null)
            {
                return NoContent();
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult> Post(AddBankRequestDto addBank)
        {
            if (addBank == null)
            {
                return BadRequest();
            }
            var id = await _bankService.AddBank(addBank);
            return Ok(id);
        }
    }
}

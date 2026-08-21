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

        [HttpGet("[action]")]
        public async Task<ActionResult> GetAllBanks()
        {
            var result = await _bankService.GetAllBanks();
            if (result == null)
            {
                return NoContent();
            }
            return Ok(result);
        }
        [HttpPost("[action]")]
        public async Task<ActionResult> AddBank(AddBankRequestDto addBank)
        {
            if (addBank == null)
            {
                return BadRequest();
            }
            var id = await _bankService.AddBank(addBank);
            return Ok(id);
        }
        [HttpGet("[action]")]
        public async Task<ActionResult> GetBankByBankId(Guid? bankId)
        {
            var bank = await _bankService.GetBankById(bankId);
            return Ok(bank);
        }
        [HttpDelete("[action]")]
        public async Task<ActionResult> DeleteBank(DeleteBankDto deleteBankDto)
        {
            var result = await _bankService.DeleteBank(deleteBankDto);

            return Ok(result);
        }
    }
}

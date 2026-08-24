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
        private readonly ExcelUploadService _excelUploadService;

        public BankMasterController(AppBankService bankService, ExcelUploadService excelUploadService)
        {
            _bankService = bankService;
            _excelUploadService = excelUploadService;
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
        [HttpGet("[action]")]
        public async Task<ActionResult> GetBankByBankId(Guid? bankId)
        {
            var bank = await _bankService.GetBankById(bankId);
            return Ok(bank);
        }
        [HttpGet("[action]")]
        public async Task<ActionResult> GetBankByParams(BankSearchDto bankSearchDto)
        {
            var response = await _bankService.GetBankByParams(bankSearchDto);
            return Ok(response);
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
        [HttpPatch("[action]")]
        public async Task<ActionResult> DeleteBank(DeleteBankDto deleteBankDto)
        {
            var result = await _bankService.DeleteBank(deleteBankDto);

            return Ok(result);
        }
        [HttpPut("[action]")]
        public async Task<ActionResult> UpdateBank(UpdateBankRequest updateBankRequest)
        { 
            if(updateBankRequest == null || updateBankRequest.Id == Guid.Empty) return BadRequest("Update request or Bank id null or empty");

            var result = await _bankService.UpdateBank(updateBankRequest);

            return Ok(result);
        }

        [HttpPost("sales-data-excel-upload")]
        public async Task<ActionResult> Post(IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Excel file is required.");
            }

            var result = await _excelUploadService.UploadExcel(file, cancellationToken);

            if (result == null) return NoContent();

            return Ok(result);

        }
    }
}

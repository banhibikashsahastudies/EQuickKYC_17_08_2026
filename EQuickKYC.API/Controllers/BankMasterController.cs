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


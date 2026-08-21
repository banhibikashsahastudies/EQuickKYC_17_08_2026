using EQuickKYC.Application.Common;
using EQuickKYC.Application.DTOs.Bank;
using EQuickKYC.Application.ExcelUpload;
using Microsoft.AspNetCore.Http;

namespace EQuickKYC.Application.Service
{
    public class ExcelUploadService
    {
        private readonly IExcelImportService _excelImportService;

        public ExcelUploadService(IExcelImportService excelImportService)
        {
            _excelImportService = excelImportService;
        }

        public async Task<Result<BankUploadResponseDto>> UploadExcel(IFormFile file, CancellationToken cancellationToken)
        {
            var extension = Path.GetExtension(file.FileName);

            var allowedExtensions = new[] { ".xlsx", ".csv" };

            if (!allowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                return Result<BankUploadResponseDto>.Fail("Only .xlsx and .csv files are supported.");
            }

            await using var stream = file.OpenReadStream();

            var data = await _excelImportService.ImportSalesAsync(stream, extension, cancellationToken);

            return Result<BankUploadResponseDto>.Ok(data, "Data successfully saved in the database");
        }
    }
}

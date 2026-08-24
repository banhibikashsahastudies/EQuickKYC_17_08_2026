using EQuickKYC.Application.DTOs.Bank;

namespace EQuickKYC.Application.ExcelUpload
{
    public interface IExcelImportService
    {
        Task<BankUploadResponseDto> ImportSalesAsync(Stream fileStream, string extension, CancellationToken cancellationToken = default);
    }
}

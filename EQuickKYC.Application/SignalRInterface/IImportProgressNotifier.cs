using EQuickKYC.Application.DTOs.Bank;

namespace EQuickKYC.Application.SignalRInterface
{
    public interface IImportProgressNotifier
    {
        Task SendProgressAsync(
        ExcelUploadRealTimeDto progress,
        CancellationToken cancellationToken = default);
    }
}

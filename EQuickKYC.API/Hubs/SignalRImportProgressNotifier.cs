using EQuickKYC.Application.DTOs.Bank;
using EQuickKYC.Application.SignalRInterface;
using Microsoft.AspNetCore.SignalR;

namespace EQuickKYC.API.Hubs
{
    public class SignalRImportProgressNotifier : IImportProgressNotifier
    {

        private readonly IHubContext<ImportProgressHub> _hubContext;

        public SignalRImportProgressNotifier(IHubContext<ImportProgressHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendProgressAsync(ExcelUploadRealTimeDto progress, CancellationToken cancellationToken = default)
        {
            await _hubContext.Clients.All.SendAsync("ImportProgress", progress, cancellationToken);
        }
    }
}

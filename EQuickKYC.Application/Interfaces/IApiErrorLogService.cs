using EQuickKYC.Domain.Entities;

namespace EQuickKYC.Application.Interfaces
{
    public interface IApiErrorLogService
    {
        Task LogAsync(ApiErrorLog errorLog);
        public Task<List<ApiErrorLog>> GetApiErorLogs();
    }
}

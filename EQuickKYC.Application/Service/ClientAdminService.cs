using EQuickKYC.Application.Common;
using EQuickKYC.Application.DTOs;
using EQuickKYC.Application.Interfaces;

namespace EQuickKYC.Application.Service
{
    public class ClientAdminService
    {
        private readonly IApiErrorLogService _apiErrorLogService;

        public ClientAdminService(IApiErrorLogService apiErrorLogService)
        {
            _apiErrorLogService = apiErrorLogService;
        }

        public async Task<Result<List<ApiErrorResponseDto>>> GetApiError()
        {
            var errorLogs = await _apiErrorLogService.GetApiErorLogs();

            if (errorLogs.Count == 0)
            {
                return Result<List<ApiErrorResponseDto>>.Fail(
                    "No error log was found.");
            }

            var response = errorLogs.Select(x => new ApiErrorResponseDto
            {
                Id = x.Id,
                CorrelationId = x.CorrelationId,
                ErrorCapturedAt = x.ErrorCapturedAt,
                ServiceName = x.ServiceName,
                Endpoint = x.Endpoint,
                ExternalApi = x.ExternalApi,
                HttpStatusCode = x.HttpStatusCode,
                ErrorType = x.ErrorType,
                ClientMessage = x.ClientMessage,
                Severity = x.Severity
            }).ToList();

            return Result<List<ApiErrorResponseDto>>.Ok(
                response,
                "Error logs retrieved successfully.");
        }
    }
}

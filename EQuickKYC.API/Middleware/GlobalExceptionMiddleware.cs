using EQuickKYC.Application.Exceptions;
using EQuickKYC.Application.Interfaces;
using EQuickKYC.Domain.Entities;

namespace eKyc.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IApiErrorLogService errorLogService)
    {
        try
        {
            await _next(context);
        }
        catch (ExternalApiException ex)
        {
            await HandleExternalApiExceptionAsync(context, ex, errorLogService);
        }
        catch (Exception ex)
        {
            await HandleUnhandledExceptionAsync(context, ex, errorLogService);
        }
    }

    private async Task HandleExternalApiExceptionAsync(HttpContext context, ExternalApiException ex, IApiErrorLogService errorLogService)
    {
        var correlationId = GetCorrelationId(context);

        // Technical log
        _logger.LogError(
            ex,
            "External API failure. " +
            "CorrelationId: {CorrelationId}, " +
            "ExternalApi: {ExternalApi}, " +
            "StatusCode: {StatusCode}",
            correlationId,
            ex.ExternalApi,
            ex.StatusCode);

        // Client-facing log
        await errorLogService.LogAsync(new ApiErrorLog
        {
            CorrelationId = correlationId,
            ErrorCapturedAt = DateTime.UtcNow,

            ServiceName = "eKYC.API",

            Endpoint = $"{context.Request.Method} {context.Request.Path}",

            ExternalApi = ex.ExternalApi,

            HttpStatusCode = ex.StatusCode,

            ErrorType = nameof(ExternalApiException),

            ErrorMessage = ex.Message,

            ClientMessage =
                "The external service is currently unavailable. " +
                "Please try again later.",

            Severity = "Error",

            RequestId = context.TraceIdentifier,

            ResponseBody = ex.ResponseBody,

            StackTrace = ex.StackTrace
        });

        context.Response.StatusCode = StatusCodes.Status502BadGateway;

        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new
        {
            success = false,

            message =
                "The external service is currently unavailable. " +
                "Please try again later.",

            correlationId
        });
    }

    private async Task HandleUnhandledExceptionAsync(HttpContext context, Exception ex, IApiErrorLogService errorLogService)
    {
        var correlationId = GetCorrelationId(context);

        // Technical log
        _logger.LogError(
            ex,
            "Unhandled exception. CorrelationId: {CorrelationId}",
            correlationId);

        // Client-facing log
        await errorLogService.LogAsync(new ApiErrorLog
        {
            CorrelationId = correlationId,
            ErrorCapturedAt = DateTime.UtcNow,

            ServiceName = "eKYC.API",

            Endpoint = $"{context.Request.Method} {context.Request.Path}",

            ErrorType = ex.GetType().Name,

            ErrorMessage = ex.Message,

            ClientMessage =
                "An unexpected error occurred. " +
                "Please try again later.",

            Severity = "Critical",

            RequestId = context.TraceIdentifier,

            StackTrace = ex.StackTrace
        });

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new
        {
            success = false,

            message =
                "An unexpected error occurred. " +
                "Please try again later.",

            correlationId
        });
    }

    private static Guid GetCorrelationId(HttpContext context)
    {
        if (context.Items["CorrelationId"] is Guid correlationId)
        {
            return correlationId;
        }

        var newCorrelationId = Guid.NewGuid();

        context.Items["CorrelationId"] = newCorrelationId;

        return newCorrelationId;
    }
}
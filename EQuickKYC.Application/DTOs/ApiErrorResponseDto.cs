namespace EQuickKYC.Application.DTOs
{
    public record ApiErrorResponseDto
    {
        public int Id { get; set; }

        public Guid CorrelationId { get; set; }

        public DateTime ErrorCapturedAt { get; set; }

        public string? ServiceName { get; set; }

        public string? Endpoint { get; set; }

        public string? ExternalApi { get; set; }

        public int? HttpStatusCode { get; set; }

        public string? ErrorType { get; set; }

        public string? ErrorMessage { get; set; }

        public string? ClientMessage { get; set; }

        public long? DurationMs { get; set; }

        public string? Severity { get; set; }

        public string? RequestId { get; set; }

        public string? StackTrace { get; set; }

        public string? ResponseBody { get; set; }
        public int? RetryCount { get; set; }
    }
}

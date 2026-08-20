namespace EQuickKYC.Application.Exceptions
{
    public class ExternalApiException : Exception
    {
        public string ExternalApi { get; }
        public int? StatusCode { get; }
        public string? ResponseBody { get; }

        public ExternalApiException(string externalApi, string message, int? statusCode = null, string? responseBody = null) : base(message)
        {
            ExternalApi = externalApi;
            StatusCode = statusCode;
            ResponseBody = responseBody;
        }
    }
}


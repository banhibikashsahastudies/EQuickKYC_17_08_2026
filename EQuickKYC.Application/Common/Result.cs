namespace EQuickKYC.Application.Common
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int? TotalCount { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public static Result<T> Ok(T data, string? message, int? totalCount)
        {
            return new Result<T>
            {
                Success = true,
                Message = message,
                TotalCount = totalCount,
                Data = data
            };

        }
        public static Result<T> Ok(T data, string? message)
        {
            return new Result<T>
            {
                Success = true,
                Message = message,
                Data = data
            };

        }
        public static Result<T> Ok(string? message)
        {
            return new Result<T>
            {
                Success = true,
                Message = message
            };

        }
        public static Result<T> Fail(string? message, List<string>? errors)
        {
            return new Result<T>
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }

        public static Result<T> Fail(string message, string error)
        {
            return new Result<T>
            {
                Success = false,
                Message = message,
                Errors = new List<string> { error }
            };
        }
        public static Result<T> Fail(string message)
        {
            return new Result<T>
            {
                Success = false,
                Message = message,
            };
        }
    }
}

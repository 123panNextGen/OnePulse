namespace OnePulse.Pan123.Api.Models
{
    public enum ApiResult
    {
        Success,
        Failed,
        AuthFailed,
        TimeOut,
        AlreadyFinished,
        NotEnoughQualifications,
    }

    public class ApiReturn<T>
    {
        public readonly ApiResult Result;
        public string Message { get; set; } = "";

        public T? Data;

        public ApiReturn(ApiResult result)
        {
            Result = result;
            Data = default;
        }

        public ApiReturn(ApiResult result, string message)
        {
            Result = result;
            Message = message;
            Data = default;
        }
    }
}

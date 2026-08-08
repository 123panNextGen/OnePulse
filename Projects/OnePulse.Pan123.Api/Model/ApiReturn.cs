using System.Diagnostics.CodeAnalysis;

namespace OnePulse.Pan123.Api.Model
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
        ApiResult Result;
        string Message { get; set; } = "";

        [MaybeNull]
        T? Data;

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

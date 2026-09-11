namespace OnePulse.Pan123.Api.Models.Sessions
{
    public abstract record LoginDataResult(string? Token = null);

    public record LoginResult(
        int? Code = null,
        string? Message = null,
        LoginDataResult? Data = null
    );
}

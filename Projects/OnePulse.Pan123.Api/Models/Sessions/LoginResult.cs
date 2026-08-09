namespace OnePulse.Pan123.Api.Models.Sessions
{
    public record class LoginDataResult(string? Token = null);

    public record class LoginResult(
        int? Code = null,
        string? Message = null,
        LoginDataResult? Data = null
    );
}

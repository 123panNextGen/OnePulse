namespace OnePulse.Pan123.Api.Model.Session
{
    public record class LoginDataResult(string? Token = null);

    public record class LoginResult(
        int? Code = null,
        string? Message = null,
        LoginDataResult? Data = null);
}

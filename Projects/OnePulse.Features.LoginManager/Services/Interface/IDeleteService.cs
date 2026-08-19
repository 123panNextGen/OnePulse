using OnePulse.Pan123.Api.Models;

namespace OnePulse.Features.LoginManager.Services.Interface
{
    public partial interface IDeleteService
    {
        public ApiReturn<string> DeleteUser(string userName);
    }
}

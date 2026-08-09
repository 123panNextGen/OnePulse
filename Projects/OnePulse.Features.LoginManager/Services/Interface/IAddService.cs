using OnePulse.Pan123.Api.Models;
using OnePulse.Pan123.Api.Models.UserInfo;

namespace OnePulse.Features.LoginManager.Services.Interface
{
    public partial interface IAddService
    {
        public ApiReturn<string> AddUserInfo(UserInfo info);
    }
}
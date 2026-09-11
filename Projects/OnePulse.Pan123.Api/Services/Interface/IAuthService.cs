using OnePulse.Pan123.Api.Models;
using OnePulse.Pan123.Api.Models.UserInfo;

namespace OnePulse.Pan123.Api.Services.Interface
{
    public interface IAuthService
    {
        public Task<ApiReturn<string>> LoginByUserInfoAsync(UserInfo userInfo);

    }
}

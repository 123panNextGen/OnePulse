using OnePulse.Pan123.Api.Models;
using OnePulse.Pan123.Api.Models.UserInfo;

namespace OnePulse.Features.LoginManager.Services
{
    public partial class LoginManager
    {
        public class AddService(LoginManager session)
        {
            private readonly LoginManager _session = session;

            public ApiReturn<string> AddUserInfo(UserInfo info)
            {
                ArgumentNullException.ThrowIfNull(_session.UserInfoCollections);

                if (_session.UserInfoCollections.FindOne(i => i.UserName == info.UserName) != null)
                    return new ApiReturn<string>(ApiResult.AlreadyFinished, "已存在");

                _session.UserInfoCollections.Insert(info);

                return new ApiReturn<string>(ApiResult.Success);
            }
        }
    }
}

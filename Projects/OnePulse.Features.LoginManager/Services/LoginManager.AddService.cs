using OnePulse.Pan123.Api.Models;
using OnePulse.Pan123.Api.Models.UserInfo;
using OnePulse.Features.LoginManager.Services.Interface;
using OnePulse.Features.LoginManager.Services.SecureCrypto;

namespace OnePulse.Features.LoginManager.Services
{
    public partial class LoginManager
    {
        public class AddService : IAddService
        {
            private readonly LoginManager _session;

            internal AddService(LoginManager session)
            {
                _session = session;
            }

            public ApiReturn<string> AddUserInfo(UserInfo info)
            {
                ArgumentNullException.ThrowIfNull(_session.UserInfoCollections);

                // 重名检查依赖 UserName 明文：加密后无法做相等匹配
                if (_session.UserInfoCollections.FindOne(i => i.UserName == info.UserName) != null)
                    return new ApiReturn<string>(ApiResult.AlreadyFinished, "已存在");

                // 落库前加密敏感字段；原对象保留明文供后续登录流程使用
                _session.UserInfoCollections.Insert(UserInfoProtector.Encrypt(info));

                return new ApiReturn<string>(ApiResult.Success);
            }
        }
    }
}
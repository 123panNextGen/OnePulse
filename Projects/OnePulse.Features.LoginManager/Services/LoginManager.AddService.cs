using OnePulse.Features.LoginManager.Models;
using OnePulse.Features.LoginManager.Services.Interface;
using OnePulse.Features.LoginManager.Services.SecureCrypto;
using OnePulse.Pan123.Api.Models;
using OnePulse.Pan123.Api.Models.UserInfo;

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
                ArgumentNullException.ThrowIfNull(_session.UserCollections);

                // 重名检查依赖 StorageUser.UserName 明文：加密后无法做相等匹配
                if (_session.UserCollections.FindOne(s => s.UserName == info.UserName) != null)
                    return new ApiReturn<string>(ApiResult.AlreadyFinished, "已存在");

                // 落库前加密敏感字段；外层仅存列表展示字段（明文），内层为密文
                _session.UserCollections.Insert(new StorageUser
                {
                    UserId = info.OpenInfo?.Uid.ToString() ?? "",
                    UserName = info.UserName,
                    HeadImageUrl = info.OpenInfo?.HeadImage,
                    UserInfo = UserInfoProtector.Encrypt(info),
                });

                return new ApiReturn<string>(ApiResult.Success);
            }
        }
    }
}

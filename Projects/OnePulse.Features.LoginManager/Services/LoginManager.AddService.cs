using OnePulse.Features.LoginManager.Models;
using OnePulse.Features.LoginManager.Services.Interface;
using OnePulse.Pan123.Api.Models;

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

            public ApiReturn<string> AddUser(StorageUser user)
            {
                ArgumentNullException.ThrowIfNull(_session.UserCollections);

                // 重名检查依赖 StorageUser.UserName 明文：加密后无法做相等匹配
                if (_session.UserCollections.FindOne(s => s.UserName == user.UserName) != null)
                    return new ApiReturn<string>(ApiResult.AlreadyFinished, "已存在");

                // 落库前敏感字段必须已是密文（由 UserInfoConverter.ToStorageUser 转换而来）
                _session.UserCollections.Insert(user);

                return new ApiReturn<string>(ApiResult.Success);
            }
        }
    }
}

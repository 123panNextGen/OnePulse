using OnePulse.Features.LoginManager.Models;
using OnePulse.Features.LoginManager.Services.Interface;
using OnePulse.Features.LoginManager.Services.SecureCrypto;

namespace OnePulse.Features.LoginManager.Services
{
    public partial class LoginManager
    {
        public class GetService : IGetService
        {
            private readonly LoginManager _session;

            internal GetService(LoginManager session)
            {
                _session = session;
            }

            public List<StorageUser> GetUsers()
            {
                ArgumentNullException.ThrowIfNull(_session.UserCollections);

                var users = _session.UserCollections.FindAll().ToList();

                // 入库时凭据为密文，读取后还原明文供展示与登录使用
                foreach (var user in users)
                    user.UserInfo = UserInfoProtector.Decrypt(user.UserInfo);

                return users;
            }
        }
    }
}

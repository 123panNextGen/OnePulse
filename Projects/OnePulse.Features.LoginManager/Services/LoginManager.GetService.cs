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

                // 入库时凭据为密文，读取后还原明文供展示与登录使用；
                // 返回全新 StorageUser 副本：明文凭据只存在于与 LiteDB 追踪实体解耦的对象上，
                // 任何后续 Update/Upsert 都不可能把明文写回库中
                return _session.UserCollections
                    .FindAll()
                    .Select(stored => new StorageUser
                    {
                        Uuid = stored.Uuid,
                        UserId = stored.UserId,
                        UserName = stored.UserName,
                        HeadImageUrl = stored.HeadImageUrl,
                        // Decrypt 返回全新 UserInfo（含新反序列化的 OpenInfo），不共享追踪实体上的子对象
                        UserInfo = UserInfoProtector.Decrypt(stored.UserInfo),
                    })
                    .ToList();
            }
        }
    }
}

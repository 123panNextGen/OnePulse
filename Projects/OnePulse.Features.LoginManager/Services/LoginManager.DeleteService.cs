using OnePulse.Features.LoginManager.Services.Interface;
using OnePulse.Pan123.Api.Models;

namespace OnePulse.Features.LoginManager.Services
{
    public partial class LoginManager
    {
        public class DeleteService : IDeleteService
        {
            private readonly LoginManager _session;

            internal DeleteService(LoginManager session)
            {
                _session = session;
            }

            public ApiReturn<string> DeleteUser(string uuid)
            {
                ArgumentNullException.ThrowIfNull(_session.UserCollections);

                // 以 StorageUser.Uuid（转换时生成的 Guid 字符串）匹配删除，
                // 与 UserInfo.Uuid（设备令牌）无关；遗留旧记录无 Uuid 将匹配不到
                var deleted = _session.UserCollections.DeleteMany(s => s.Uuid == uuid);

                return deleted > 0
                    ? new ApiReturn<string>(ApiResult.Success)
                    : new ApiReturn<string>(ApiResult.Failed, "用户不存在");
            }
        }
    }
}

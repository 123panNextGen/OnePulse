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

            public ApiReturn<string> DeleteUser(string userName)
            {
                ArgumentNullException.ThrowIfNull(_session.UserCollections);

                // 以明文 UserName 匹配删除（与 AddService 重名检查同一键）
                var deleted = _session.UserCollections.DeleteMany(s => s.UserName == userName);

                return deleted > 0
                    ? new ApiReturn<string>(ApiResult.Success)
                    : new ApiReturn<string>(ApiResult.Failed, "用户不存在");
            }
        }
    }
}

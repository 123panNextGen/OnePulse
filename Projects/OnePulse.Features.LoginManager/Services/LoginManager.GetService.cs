using OnePulse.Features.LoginManager.Services.Interface;
using OnePulse.Pan123.Api.Models.UserInfo;

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

            public List<UserInfo> GetUsers()
            {
                ArgumentNullException.ThrowIfNull(_session.UserInfoCollections);

                var query = _session.UserInfoCollections.FindAll();

                List<UserInfo> allUsers = [.. query];

                return allUsers;
            }
        }
    }
}

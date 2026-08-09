using OnePulse.Pan123.Api.Models.UserInfo;

namespace OnePulse.Features.LoginManager.Services.Interface
{
    public partial interface IGetService
    {
        public List<UserInfo> GetUsers();
    }
}

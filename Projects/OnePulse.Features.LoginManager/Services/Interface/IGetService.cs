using OnePulse.Features.LoginManager.Models;

namespace OnePulse.Features.LoginManager.Services.Interface
{
    public partial interface IGetService
    {
        public List<StorageUser> GetUsers();
    }
}

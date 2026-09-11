using OnePulse.Features.LoginManager.Models;

namespace OnePulse.Features.LoginManager.Services.Interface
{
    public interface IGetService
    {
        public List<StorageUser> GetUsers();
    }
}

using OnePulse.Pan123.Api.Models;

namespace OnePulse.Features.LoginManager.Services.Interface
{
    public partial interface IDeleteService
    {
        // 为什么：删除以 StorageUser.Uuid（转换时生成的唯一记录键）定位，
        // 而非可变的 UserName —— 用户名可改、可重复，不能作为可靠删除键
        public ApiReturn<string> DeleteUser(string uuid);
    }
}

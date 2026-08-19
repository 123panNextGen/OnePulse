using OnePulse.Features.LoginManager.Models;
using OnePulse.Pan123.Api.Models;

namespace OnePulse.Features.LoginManager.Services.Interface
{
    public partial interface IAddService
    {
        // 为什么：写操作统一接收已转换的 StorageUser（敏感字段已加密），
        // 明文 UserInfo 需先经 UserInfoConverter.ToStorageUser 转换
        public ApiReturn<string> AddUser(StorageUser user);
    }
}
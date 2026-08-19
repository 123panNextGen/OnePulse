using OnePulse.Features.LoginManager.Models;
using OnePulse.Pan123.Api.Models.UserInfo;

namespace OnePulse.Features.LoginManager.Services.Interface
{
    public partial interface IUserInfoConverter
    {
        // UserInfo → StorageUser 转换契约
        // 为什么：写操作（Add）统一接收已转换的 StorageUser，
        // 转换（含敏感字段加密）集中在单一服务，保证"凡落库必已加密"
        public StorageUser ToStorageUser(UserInfo info);
    }
}

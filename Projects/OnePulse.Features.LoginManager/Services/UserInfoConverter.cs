using OnePulse.Features.LoginManager.Models;
using OnePulse.Features.LoginManager.Services.Interface;
using OnePulse.Features.LoginManager.Services.SecureCrypto;
using OnePulse.Pan123.Api.Models.UserInfo;

namespace OnePulse.Features.LoginManager.Services
{
    // UserInfo → StorageUser 转换服务
    // 为什么：转换逻辑原本散落在 AddService 内部，调用方无法复用，
    // 且"落库前必须加密"的安全约束与持久化耦合。抽成独立服务后：
    //   1. 单一职责 —— 转换/加密与数据库读写分离，各自独立演进；
    //   2. 安全兜底 —— 转换即加密，调用方拿到的 StorageUser 敏感字段必为密文；
    //   3. 便于扩展 —— 后续新增登录方式（如二维码登录）只需构造 UserInfo，
    //      转换逻辑无需变动。
    public class UserInfoConverter : IUserInfoConverter
    {
        public StorageUser ToStorageUser(UserInfo info)
        {
            ArgumentNullException.ThrowIfNull(info);

            return new StorageUser
            {
                // 转换时生成唯一记录键：一次生成、永久稳定，删除/定位不再依赖可变的 UserName；
                // 与 UserInfo.Uuid（设备绑定令牌）无关，两者职责分离
                Uuid = Guid.NewGuid().ToString(),
                // 外层字段保持明文，仅供列表展示与重名查询
                UserId = info.OpenInfo?.Uid.ToString() ?? "",
                UserName = info.UserName,
                HeadImageUrl = info.OpenInfo?.HeadImage,
                // 内层敏感字段在此完成加密：Encrypt 返回新对象，不改动原 UserInfo
                UserInfo = UserInfoProtector.Encrypt(info),
            };
        }
    }
}

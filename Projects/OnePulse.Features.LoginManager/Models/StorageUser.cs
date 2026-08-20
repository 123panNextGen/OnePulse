using OnePulse.Pan123.Api.Models.UserInfo;

namespace OnePulse.Features.LoginManager.Models
{
    // LiteDB 存储模型：外层承载列表展示与查询字段（明文），
    // UserInfo 为加密后的凭据（密文），读取时由 GetService 解密还原
    public class StorageUser
    {
        // 存储记录的唯一稳定键（Guid 字符串），由 UserInfoConverter 转换时生成。
        // 为什么：UserName 可变且可能重名，不能作为可靠的删除/定位键；
        // 默认空串用于识别旧库中尚无 Uuid 的遗留记录
        public string Uuid { get; set; } = "";

        public string UserId { get; set; } = "";

        public string? UserName { get; set; }

        public string? HeadImageUrl { get; set; }

        public UserInfo UserInfo { get; set; } = new();
    }
}

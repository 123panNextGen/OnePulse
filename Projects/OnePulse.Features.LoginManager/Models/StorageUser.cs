using OnePulse.Pan123.Api.Models.UserInfo;

namespace OnePulse.Features.LoginManager.Models
{
    // LiteDB 存储模型：外层承载列表展示与查询字段（明文），
    // UserInfo 为加密后的凭据（密文），读取时由 GetService 解密还原
    public class StorageUser
    {
        public int? Version { get; init; } = 1;

        public string Uuid { get; init; } = "";

        public string UserId { get; init; } = "";

        public string? UserName { get; init; }

        public string? HeadImageUrl { get; init; }

        public UserInfo UserInfo { get; init; } = new();
    }
}

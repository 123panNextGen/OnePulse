using OnePulse.Pan123.Api.Models.UserInfo;

namespace OnePulse.Features.LoginManager.Models
{
    // LiteDB 存储模型：外层承载列表展示与查询字段（明文），
    // UserInfo 为加密后的凭据（密文），读取时由 GetService 解密还原
    public class StorageUser
    {
        public string UserId { get; set; } = "";

        public string? UserName { get; set; }

        public string? HeadImageUrl { get; set; }

        public UserInfo UserInfo { get; set; } = new();
    }
}

using System.Security.Cryptography;
using System.Text;

namespace OnePulse.Features.LoginManager.Services.SecureCrypto
{
    // DPAPI 加解密：密钥由当前 Windows 用户账户派生，
    // 代码中不再出现任何硬编码密钥，换机器后也无法解密（数据只属于本机用户）
    // 仅用于 WinUI 桌面端，无需支持跨平台，故关闭 CA1416（API 仅 Windows 可用）
#pragma warning disable CA1416
    public static class SecureCryptoService
    {
        // 加密字符串 → Base64 密文
        public static string Protect(string plainText)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encrypted = ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encrypted);
        }

        // Base64 密文 → 明文
        public static string Unprotect(string cipherText)
        {
            byte[] encrypted = Convert.FromBase64String(cipherText);
            byte[] bytes = ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(bytes);
        }

        // 生成随机密钥（十六进制）
        // LiteDB 密码仅允许字母数字，hex 编码可避开特殊字符限制
        public static string GenerateRandomKey(int length = 32)
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(length));
        }
    }
#pragma warning restore CA1416
}
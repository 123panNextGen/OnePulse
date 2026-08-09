using System.Text.Json;
using OnePulse.Pan123.Api.Models.UserInfo;

namespace OnePulse.Features.LoginManager.Services.SecureCrypto
{
    // UserInfo 的敏感字段在入库前加密，读取后还原
    // UserName 必须保持明文：重名检查（FindOne 查询）依赖它，加密后无法按用户名检索
    public static class UserInfoProtector
    {
        // 写入前调用：加密敏感字段，返回新对象
        public static UserInfo Encrypt(UserInfo source)
        {
            return new UserInfo
            {
                LoginMethod = source.LoginMethod,
                UserName = source.UserName,
                Password = Secure(source.Password),
                Authorization = Secure(source.Authorization),
                Uuid = Secure(source.Uuid),
                DeviceInfo = source.DeviceInfo,
                // OpenInfo 是对象，序列化后再加密
                OpenInfoCipher = SecureInfo(source.OpenInfo),
            };
        }

        // 读取后调用：还原明文敏感字段
        public static UserInfo Decrypt(UserInfo stored)
        {
            return new UserInfo
            {
                LoginMethod = stored.LoginMethod,
                UserName = stored.UserName,
                Password = Unsecure(stored.Password),
                Authorization = Unsecure(stored.Authorization),
                Uuid = Unsecure(stored.Uuid),
                DeviceInfo = stored.DeviceInfo,
                OpenInfo = UnsecureInfo(stored.OpenInfoCipher),
            };
        }

        static string? Secure(string? value)
        {
            return string.IsNullOrEmpty(value) ? value : SecureCryptoService.Protect(value);
        }

        static string? Unsecure(string? value)
        {
            return string.IsNullOrEmpty(value) ? value : SecureCryptoService.Unprotect(value);
        }

        static string? SecureInfo(OpenUserInfo? info)
        {
            return info == null ? null : SecureCryptoService.Protect(JsonSerializer.Serialize(info));
        }

        static OpenUserInfo? UnsecureInfo(string? cipher)
        {
            return string.IsNullOrEmpty(cipher)
                ? null
                : JsonSerializer.Deserialize<OpenUserInfo>(SecureCryptoService.Unprotect(cipher));
        }
    }
}
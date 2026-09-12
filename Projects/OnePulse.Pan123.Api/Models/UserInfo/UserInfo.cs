namespace OnePulse.Pan123.Api.Models.UserInfo
{
    public enum LoginMethod
    {
        PasswordLogin,
        QRCodeLogin,
    }

    public class DeviceInfo
    {
        public string Os { get; private set; } = string.Empty;
        public string Type { get; private set; } = string.Empty;

        public DeviceInfo(string os, string type)
        {
            Os = os;
            Type = type;
        }

        private DeviceInfo() { }

        public static DeviceInfo DeviceInfoFromString(string device)
        {
            string[] deviceParts = device.Split([':'], 2);

            return new DeviceInfo
            {
                Os = deviceParts.Length > 0 ? deviceParts[0] : string.Empty,
                Type = deviceParts.Length > 1 ? deviceParts[1] : string.Empty,
            };
        }
    }

    public class UserInfo
    {
        // 登录方式
        public LoginMethod LoginMethod { get; init; } = LoginMethod.PasswordLogin;

        // 基本信息
        public string? UserName { get; init; } = "";
        public string? Password { get; init; } = "";

        // Token
        public string? Authorization { get; set; }
        public string? Uuid { get; init; }

        // 登录信息
        public DeviceInfo? DeviceInfo { get; init; }

        public OpenUserInfo? OpenInfo { get; init; }

        // 对象无法直接加密，序列化为 JSON 后存密文
        public string? OpenInfoCipher { get; init; }

        public string GetUserName()
        {
            if (OpenInfo != null)
                return (OpenInfo.Nickname ?? OpenInfo.Passport ?? OpenInfo.Mail ?? UserName) ?? string.Empty;
            return UserName ?? string.Empty;
        }

        public UserInfo() { }

        public UserInfo(string username, string password)
        {
            UserName = username;
            Password = password;
        }
    }
}
namespace OnePulse.Pan123.Api.Models.UserInfo
{
    public enum LoginMethod
    {
        PasswordLogin,
        QRCodeLogin,
    }

    public class DeviceInfo
    {
        public string OS { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;

        public DeviceInfo(string os, string type)
        {
            OS = os;
            Type = type;
        }

        public DeviceInfo() { }

        public static DeviceInfo NewDeviceInfo(string device)
        {
            string[] deviceParts = device.Split([':'], 2);

            return new DeviceInfo
            {
                OS = deviceParts.Length > 0 ? deviceParts[0] : string.Empty,
                Type = deviceParts.Length > 1 ? deviceParts[1] : string.Empty,
            };
        }
    }

    public class UserInfo
    {
        // 登录方式
        public LoginMethod LoginMethod { get; set; } = LoginMethod.PasswordLogin;

        // 基本信息
        public string? UserName { get; set; } = "";
        public string? Password { get; set; } = "";

        // Token
        public string? Authorization { get; set; }
        public string? Uuid { get; set; }

        // 登录信息
        public DeviceInfo? DeviceInfo { get; set; }

        public OpenUserInfo? OpenInfo { get; set; }

        // 对象无法直接加密，序列化为 JSON 后存密文
        public string? OpenInfoCipher { get; set; }

        public UserInfo() { }

        public UserInfo(string username, string password)
        {
            UserName = username;
            Password = password;
        }
    }
}

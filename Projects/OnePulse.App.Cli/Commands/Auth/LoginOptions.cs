using System.CommandLine;

namespace OnePulse.App.Gui.Commands.Auth
{
    // 登录选项集中于此，与 LoginCommand 同目录：选项只被 login 命令使用，
    // 与命令放在一起保证改选项和改命令不用跨目录
    internal class LoginOptions
    {
        public Option<string> UserNameOption = new(name: "--user-name")
        {
            Description = "The user name for login.",
            Required = true,
        };
        public Option<string> PasswordOption = new(name: "--password")
        {
            Description = "The password for login.",
            Required = true,
        };
        public Option<string> UuidOption = new(name: "--uuid")
        {
            Description = "The UUID for login.",
            Required = false,
            // 每次解析自动生成新 UUID，保证设备指纹唯一
            DefaultValueFactory = (_) => Guid.NewGuid().ToString(),
        };
        public Option<string> DeviceOption = new(name: "--device")
        {
            Description = "The Device for login. (e.g., Xiaomi:17)",
            Required = false,
        };

        public Option<bool> ReplaceOption = new(name: "--replace")
        {
            Description = "Replace the existing token.",
            Required = false,
            DefaultValueFactory = (_) => false,
        };
    }
}
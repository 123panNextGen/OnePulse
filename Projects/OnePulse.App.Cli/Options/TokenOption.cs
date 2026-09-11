using System.CommandLine;

namespace OnePulse.App.Cli.Options
{
    // 根级共享选项：--token 会被 auth 各子命令（login/logout 等）读取，
    // 独立成类便于各命令引用同一个实例，避免多处重复定义
    internal class TokenOption
    {
        public Option<string> Option { get; } = new(name: "--token", ["-T"])
        {
            Description = "The token for authentication. (Without Bearer prefix)",
            Required = false,
        };
    }
}
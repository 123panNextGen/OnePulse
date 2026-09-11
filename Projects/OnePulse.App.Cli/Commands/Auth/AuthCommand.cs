using System.CommandLine;

namespace OnePulse.App.Cli.Commands.Auth
{
    // 父命令工厂：新增 auth 子命令（logout 等）只需在这里追加一行，
    // Program.cs 作为入口组装点无需随之改动
    internal class AuthCommand
    {
        public Command Command { get; } = new("auth", "Authentication commands");

        public AuthCommand(Option<string> tokenOption)
        {
            Command.Subcommands.Add(new LoginCommand(tokenOption).Command);
        }
    }
}
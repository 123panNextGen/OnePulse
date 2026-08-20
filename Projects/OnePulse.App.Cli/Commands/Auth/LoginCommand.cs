using System.CommandLine;
using System.CommandLine.Invocation;

namespace OnePulse.App.Gui.Commands.Auth
{
    // 命令定义与动作逻辑分离：本类只负责 System.CommandLine 接线，
    // 实际登录行为在 LoginAction 中，便于单独测试与复用
    internal class LoginCommand
    {
        public Command Command { get; } = new("login", "Login command");

        public LoginCommand(Option<string> tokenOption)
        {
            var options = new LoginOptions();

            Command.Options.Add(options.UserNameOption);
            Command.Options.Add(options.PasswordOption);
            Command.Options.Add(options.UuidOption);
            Command.Options.Add(options.DeviceOption);
            Command.Options.Add(options.ReplaceOption);

            // token 定义在根命令上，但子命令的解析结果中同样可取到
            Command.SetAction(async parseResult =>
            {
                var result = await new LoginAction().LoginActionAsync(
                    userName: parseResult.GetValue(options.UserNameOption),
                    password: parseResult.GetValue(options.PasswordOption),
                    uuid: parseResult.GetValue(options.UuidOption),
                    device: parseResult.GetValue(options.DeviceOption),
                    token: parseResult.GetValue(tokenOption),
                    replaceToken: parseResult.GetValue(options.ReplaceOption)
                );
                Console.WriteLine(result);
            });
        }
    }
}

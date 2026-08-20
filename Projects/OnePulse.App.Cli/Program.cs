using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using OnePulse.App.Cli.Commands;
using OnePulse.App.Cli.Options;

namespace OnePulse.App.Cli;

internal class Program
{
    internal static int Main(string[] args)
    {
        Option<string> tokenOption = new(name: "--token", ["-T"])
        {
            Description = "The token for authentication.",
            Required = false,
        };

        var authOptions = new Options.Auth.LoginOptions();

        RootCommand rootCommand = new("OnePulse CommandLine Tool");
        rootCommand.Options.Add(tokenOption);

        Command authCommand = new("auth", "Authentication commands");
        rootCommand.Subcommands.Add(authCommand);

        Command loginCommand = new("login", "Login command");
        loginCommand.Options.Add(authOptions.UserNameOption);
        loginCommand.Options.Add(authOptions.PasswordOption);
        loginCommand.Options.Add(authOptions.UuidOption);
        loginCommand.Options.Add(authOptions.DeviceOption);
        loginCommand.Options.Add(authOptions.ReplaceOption);
        loginCommand.SetAction(async parseResult => await new LoginCommand().LoginActionAsync(
            userName: parseResult.GetValue(authOptions.UserNameOption),
            password: parseResult.GetValue(authOptions.PasswordOption),
            uuid: parseResult.GetValue(authOptions.UuidOption),
            device: parseResult.GetValue(authOptions.DeviceOption),
            token: parseResult.GetValue(tokenOption),
            replaceToken: parseResult.GetValue(authOptions.ReplaceOption)
        ));
        authCommand.Subcommands.Add(loginCommand);

        ParseResult parseResult = rootCommand.Parse(args);
        return parseResult.Invoke();
    }
}

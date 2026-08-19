using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using OnePulse.App.Cli.Options;

namespace OnePulse.App.Cli;

internal class Program
{
    public static int Main(string[] args)
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
        authCommand.Subcommands.Add(loginCommand);

        ParseResult parseResult = rootCommand.Parse(args);
        return parseResult.Invoke();
    }
}

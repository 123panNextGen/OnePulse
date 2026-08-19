using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;

namespace OnePulse.App.Cli;

internal static class Program
{
    static int Main(string[] args)
    {
        Option<string> tokenOption = new(name: "--token")
        {
            Description = "The token for authentication.",
            Required = false,
        };

        Option<string> loginUserNameOption = new(name: "--user-name")
        {
            Description = "The user name for login.",
            Required = true,
        };
        Option<string> loginPasswordOption = new(name: "--password")
        {
            Description = "The password for login.",
            Required = true,
        };
        Option<string> loginUuidOption = new(name: "--uuid")
        {
            Description = "The UUID for login.",
            Required = false,
            DefaultValueFactory = (_) => Guid.NewGuid().ToString(),
        };
        Option<string> loginDeviceOption = new(name: "--device")
        {
            Description = "The Device for login.",
            Required = false,
            DefaultValueFactory = (_) => "Xiaomi:17",
        };

        RootCommand rootCommand = new("OnePulse CommandLine Tool");
        rootCommand.Options.Add(tokenOption);


        Command authCommand = new("auth", "Authentication commands");
        rootCommand.Subcommands.Add(authCommand);

        Command loginCommand = new("login", "Login command");
        loginCommand.Options.Add(loginUserNameOption);
        loginCommand.Options.Add(loginPasswordOption);
        loginCommand.Options.Add(loginUuidOption);
        loginCommand.Options.Add(loginDeviceOption);
        authCommand.Subcommands.Add(loginCommand);


        ParseResult parseResult = rootCommand.Parse(args);
        return parseResult.Invoke();
    }
}
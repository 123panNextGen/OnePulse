using System.CommandLine;
using System.CommandLine.Parsing;
using OnePulse.App.Gui.Commands.Auth;
using OnePulse.App.Gui.Options;

namespace OnePulse.App.Gui;

internal static class Program
{
    internal static int Main(string[] args)
    {
        // 根级共享选项，auth 各子命令通过构造器引用同一实例
        var tokenOption = new TokenOption().Option;

        RootCommand rootCommand = new("OnePulse CommandLine Tool");
        rootCommand.Options.Add(tokenOption);
        rootCommand.Subcommands.Add(new AuthCommand(tokenOption).Command);

        ParseResult parseResult = rootCommand.Parse(args);
        return parseResult.Invoke();
    }
}
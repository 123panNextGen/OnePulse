using System.CommandLine;

namespace OnePulse.App.Cli.Commands.Auth
{
    internal class ManagerCommand
    {
        public Command Command { get; } = new("manager", "Login Manager command");

        public ManagerCommand(Option<string> tokenOption)
        {
            
        }
    }
}
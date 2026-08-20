using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text;

namespace OnePulse.App.Cli.Options.Auth
{
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

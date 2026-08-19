using OnePulse.Pan123.Api.Models;
using OnePulse.Pan123.Api.Models.UserInfo;
using OnePulse.Pan123.Api.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnePulse.App.Cli.Commands
{
    internal class LoginCommand
    {
        internal NetSession Manager { get; private set; } = NetSession.Instance;

        internal async Task<string> LoginByPassword(UserInfo userInfo)
        {
            var result = await Manager.Auth.LoginByUserInfoAsync(userInfo);
            
            if (result.Result == ApiResult.Success && result.Data != null)
                return result.Data;
            
            throw new InvalidOperationException($"Failed to login. Msg: {result.Message}");
        }

        internal async Task<string> Login(UserInfo userInfo, bool replaceToken = false)
        {
            if (userInfo.Authorization != null && !replaceToken)
                return userInfo.Authorization;

            if (userInfo.LoginMethod == LoginMethod.PasswordLogin)
                return await LoginByPassword(userInfo);

            throw new NotImplementedException("Login method not implemented.");
        }
    }
}

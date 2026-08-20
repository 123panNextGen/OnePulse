using System;
using System.Collections.Generic;
using System.Text;
using OnePulse.Pan123.Api.Models;
using OnePulse.Pan123.Api.Models.UserInfo;
using OnePulse.Pan123.Api.Services;

namespace OnePulse.App.Cli.Commands
{
    internal class LoginCommand
    {
        internal NetSession Manager { get; private set; } = NetSession.Instance;

        internal async Task<string> LoginByPasswordAsync(UserInfo userInfo)
        {
            var result = await Manager.Auth.LoginByUserInfoAsync(userInfo);

            if (result.Result == ApiResult.Success && result.Data != null)
                return result.Data;

            throw new InvalidOperationException($"Failed to login. Msg: {result.Message}");
        }

        internal async Task<string> LoginAsync(UserInfo userInfo, bool replaceToken = false)
        {
            if (userInfo.Authorization != null && !replaceToken)
                return userInfo.Authorization;

            if (userInfo.LoginMethod == LoginMethod.PasswordLogin)
                return await LoginByPasswordAsync(userInfo);

            throw new NotImplementedException("Login method not implemented.");
        }

        internal async Task<string> LoginActionAsync(
            string? userName,
            string? password,
            string? uuid,
            string? device,
            string? token,
            bool replaceToken = false
        )
        {
            if (token != null && !replaceToken)
                return token;

            device ??= "Xiaomi:17"; // 防止 device 没有提供

            string[] deviceParts = device.Split([':'], 2);

            var deviceInfo = new DeviceInfo
            {
                OS = deviceParts.Length > 0 ? deviceParts[0] : string.Empty,
                Type = deviceParts.Length > 1 ? deviceParts[1] : string.Empty
            };

            var userInfo = new UserInfo
            {
                UserName = userName,
                Password = password,
                Uuid = uuid,
                DeviceInfo = deviceInfo,
                LoginMethod = LoginMethod.PasswordLogin,
            };

            return await LoginAsync(userInfo, replaceToken);
        }
    }
}

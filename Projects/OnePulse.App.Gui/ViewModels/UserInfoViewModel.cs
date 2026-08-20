using OnePulse.Pan123.Api.Models;
using OnePulse.Pan123.Api.Models.UserInfo;
using OnePulse.Pan123.Api.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnePulse.App.Gui.ViewModels
{
    internal class UserInfoViewModel
    {
        // 单例会话，复用全局登录状态（Authorization/Uuid 等）
        internal NetSession Manager { get; private set; } = NetSession.Instance;

        internal async Task<string> LoginByPasswordAsync(UserInfo userInfo)
        {
            var result = await Manager.Auth.LoginByUserInfoAsync(userInfo);

            if (result.Result == ApiResult.Success && result.Data != null)
                return result.Data;

            // 抛异常让上层捕获，携带服务端返回的错误消息便于排查
            throw new InvalidOperationException($"Failed to login. Msg: {result.Message}");
        }

        public async Task<UserInfo> LoginAsync(UserInfo userInfo, bool replaceToken = false)
        {
            // 已有令牌且未要求替换时直接复用，避免每次启动都重新登录
            if (userInfo.Authorization != null && !replaceToken)
                return userInfo;

            switch (userInfo.LoginMethod)
            {
                case LoginMethod.PasswordLogin:
                    var token = await LoginByPasswordAsync(userInfo);
                    userInfo.Authorization = token;
                    return userInfo;
                default:
                    throw new NotImplementedException("Login method not implemented.");
            }
        }
    }
}

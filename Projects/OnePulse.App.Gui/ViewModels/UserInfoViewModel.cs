using System;
using System.Threading.Tasks;
using OnePulse.Pan123.Api.Models;
using OnePulse.Pan123.Api.Models.UserInfo;
using OnePulse.Pan123.Api.Services;

namespace OnePulse.App.Gui.ViewModels
{
    internal class UserInfoViewModel
    {
        // 单例会话，复用全局登录状态（Authorization/Uuid 等）
        private NetSession Manager { get; } = NetSession.Instance;

        private async Task LoginByPasswordAsync(UserInfo userInfo)
        {
            ApiReturn<string> result = await Manager.Auth.LoginByUserInfoAsync(userInfo);

            if (result is not { Result: ApiResult.Success, Data: not null })
                // 抛异常让上层捕获，携带服务端返回的错误消息便于排查
                throw new InvalidOperationException($"Failed to login. Msg: {result.Message}");
        }

        public async Task<UserInfo?> LoginAsync(UserInfo userInfo, bool replaceToken = false)
        {
            // 已有令牌且未要求替换时直接复用，避免每次启动都重新登录
            if (userInfo.Authorization != null && !replaceToken)
                return userInfo;

            // 判断 DeviceInfo 是否为 null, 如果是就随机
            userInfo.DeviceInfo ??= Manager.Utils.GetRandomDeviceInfo();

            switch (userInfo.LoginMethod)
            {
                case LoginMethod.PasswordLogin:
                    await LoginByPasswordAsync(userInfo);
                    return Manager.UserInfo;
                case LoginMethod.QRCodeLogin:
                default:
                    throw new NotImplementedException("Login method not implemented.");
            }
        }
    }
}

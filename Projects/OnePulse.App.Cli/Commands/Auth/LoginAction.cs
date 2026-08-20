using OnePulse.Pan123.Api.Models;
using OnePulse.Pan123.Api.Models.UserInfo;
using OnePulse.Pan123.Api.Services;

namespace OnePulse.App.Gui.Commands.Auth
{
    // 登录动作与命令定义分离：LoginAction 不依赖 System.CommandLine，
    // 可被单元测试直接调用，也便于将来被其他入口（如 GUI）复用
    internal class LoginAction
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

        internal async Task<string> LoginAsync(UserInfo userInfo, bool replaceToken = false)
        {
            // 已有令牌且未要求替换时直接复用，避免每次启动都重新登录
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
            // 命令行已提供 token 且未要求替换时直接使用，跳过网络登录
            if (token != null && !replaceToken)
                return token;

            // 设备串格式为 "OS:类型"，未提供时兜底为 123pan 客户端常见取值
            device ??= "Xiaomi:17";

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
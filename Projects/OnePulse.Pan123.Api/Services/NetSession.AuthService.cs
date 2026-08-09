using OnePulse.Pan123.Api.Models;
using OnePulse.Pan123.Api.Models.Sessions;
using OnePulse.Pan123.Api.Models.UserInfo;
using OnePulse.Pan123.Api.Services.Interface;
using System.Net.Http.Json;
using System.Text.Json;

namespace OnePulse.Pan123.Api.Services
{
    public partial class NetSession
    {
        public class AuthService : IAuthService
        {
            private readonly NetSession _session;

            internal AuthService(NetSession session)
            {
                _session = session;
            }

            // 登录
            public async Task<ApiReturn<string>> LoginByUserInfoAsync(UserInfo userInfo)
            {
                ArgumentNullException.ThrowIfNull(userInfo, nameof(userInfo));
                ArgumentNullException.ThrowIfNull(userInfo.UserName, nameof(userInfo.UserName));
                ArgumentNullException.ThrowIfNull(userInfo.Password, nameof(userInfo.Password));
                ArgumentNullException.ThrowIfNull(userInfo.DeviceInfo, nameof(userInfo.DeviceInfo));
                ArgumentNullException.ThrowIfNull(userInfo.Uuid, nameof(userInfo.Uuid));

                try
                {
                    // 请求
                    using var response = await NetSession.sharedClient.PostAsJsonAsync(
                        "/b/api/user/sign_in",
                        new
                        {
                            type = 1,
                            passport = userInfo.UserName,
                            password = userInfo.Password,
                        }
                    );

                    // 判断
                    if (!response.IsSuccessStatusCode)
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        return new ApiReturn<string>(
                            ApiResult.Failed,
                            $"请求失败 ({(int)response.StatusCode}): {errorContent}"
                        );
                    }

                    // 转换
                    var result = await response.Content.ReadFromJsonAsync<LoginResult>();

                    // 判断
                    if (
                        result == null
                        || result.Code != 200
                        || result.Data == null
                        || result.Data.Token is { Length: <= 0 }
                    )
                        return new ApiReturn<string>(
                            ApiResult.Failed,
                            result?.Message ?? "登录失败"
                        );

                    // 应用
                    _session.UserInfo = userInfo;
                    _session.UserInfo.Authorization = result.Data.Token;

                    _session.Utils.UpdateHeaders();

                    return new ApiReturn<string>(ApiResult.Success, result.Message ?? "登录成功")
                    {
                        Data = result.Data.Token,
                    };
                }
                catch (HttpRequestException ex)
                {
                    return new ApiReturn<string>(ApiResult.Failed, $"网络请求异常: {ex.Message}");
                }
                catch (JsonException ex)
                {
                    return new ApiReturn<string>(
                        ApiResult.Failed,
                        $"响应数据解析异常: {ex.Message}"
                    );
                }
                catch (TaskCanceledException)
                {
                    return new ApiReturn<string>(ApiResult.Failed, "请求超时或已取消");
                }
                catch (Exception ex)
                {
                    return new ApiReturn<string>(ApiResult.Failed, $"未知错误: {ex.Message}");
                }
            }
        }
    }
}

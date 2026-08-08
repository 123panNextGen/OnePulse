using OnePulse.Pan123.Api.Model;
using OnePulse.Pan123.Api.Model.Session;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace OnePulse.Pan123.Api.Service
{
    public partial class NetSession
    {
        public class AuthService
        {
            private readonly NetSession _session;

            internal AuthService(NetSession session)
            {
                _session = session;
            }

            // 登录
            public async Task<ApiReturn<string>> LoginByUserInfoAsync(UserInfo userInfo)
            {
                ArgumentNullException.ThrowIfNull(userInfo.UserName);
                ArgumentNullException.ThrowIfNull(userInfo.Password);
                ArgumentNullException.ThrowIfNull(userInfo.DeviceInfo);
                ArgumentNullException.ThrowIfNull(userInfo.Uuid);

                using StringContent jsonContent = new(
                    JsonSerializer.Serialize(new
                    {
                        type = 1,
                        passport = userInfo.UserName,
                        password = userInfo.Password,
                    }),
                    Encoding.UTF8,
                    "application/json");

                using HttpResponseMessage response =
                    await NetSession.sharedClient.PostAsync("/b/api/user/sign_in", jsonContent);

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<LoginResult>();

                if (result == null || result.Code != 200 || result.Data == null)
                    return new ApiReturn<string>(ApiResult.Failed, result?.Message ?? "登录失败");

                _session.UserInfo = userInfo;
                _session.UserInfo.Authorization = result.Data.Token;

                _session.Utils.UpdateHeaders();

                return new ApiReturn<string>(ApiResult.Success, result.Message ?? "登录成功");
            }
        }
    }
}
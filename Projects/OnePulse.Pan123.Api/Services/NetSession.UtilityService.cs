using OnePulse.Pan123.Api.Models;
using OnePulse.Pan123.Api.Models.UserInfo;
using System.Net.Http.Headers;

namespace OnePulse.Pan123.Api.Services
{
    public partial class NetSession
    {
        public class UtilityService
        {
            private readonly NetSession _session;

            internal UtilityService(NetSession session)
            {
                _session = session;
            }

            internal ApiReturn<string> UpdateHeaders()
            {
                if (_session.UserInfo == null)
                    return new ApiReturn<string>(ApiResult.NotEnoughQualifications);

                Dictionary<string, string> headers = BuildHeadersByUserInfo(_session.UserInfo);

                HttpRequestHeaders defaultHeaders = SharedClient.DefaultRequestHeaders;
                defaultHeaders.Clear();
                foreach (KeyValuePair<string, string> kv in headers)
                    defaultHeaders.TryAddWithoutValidation(kv.Key, kv.Value);

                return new ApiReturn<string>(ApiResult.Success);
            }

            private static Dictionary<string, string> BuildHeadersByUserInfo(UserInfo userInfo)
            {
                ArgumentNullException.ThrowIfNull(userInfo.DeviceInfo);
                ArgumentNullException.ThrowIfNull(userInfo.Uuid);

                Dictionary<string, string> headers = new()
                {
                    ["user-agent"] = $"123pan/v2.4.0({userInfo.DeviceInfo.OS};Xiaomi)",
                    ["accept-encoding"] = "gzip",
                    ["content-type"] = "application/json",
                    ["osversion"] = userInfo.DeviceInfo.OS,
                    ["loginuuid"] = userInfo.Uuid,
                    ["platform"] = "android",
                    ["devicetype"] = userInfo.DeviceInfo.Type,
                    ["devicename"] = "Xiaomi",
                    ["app-version"] = "61",
                    ["x-app-version"] = "2.4.0",
                };

                if (userInfo.Authorization is { Length: > 0 } auth)
                    headers.Add("Authorization", auth);

                return headers;
            }
        }
    }
}

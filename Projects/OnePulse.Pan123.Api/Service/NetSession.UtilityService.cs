using OnePulse.Pan123.Api.Model;

namespace OnePulse.Pan123.Api.Service
{
    public partial class NetSession
    {
        internal class UtilityService
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

                var headers = BuildHeadersByUserInfo(_session.UserInfo);

                var defaultHeaders = NetSession.sharedClient.DefaultRequestHeaders;
                defaultHeaders.Clear();
                foreach (var kv in headers)
                    defaultHeaders.TryAddWithoutValidation(kv.Key, kv.Value);

                return new ApiReturn<string>(ApiResult.Success);
            }

            internal Dictionary<string, string> BuildHeadersByUserInfo(UserInfo userInfo)
            {
                ArgumentNullException.ThrowIfNull(userInfo.DeviceInfo);
                ArgumentNullException.ThrowIfNull(userInfo.Uuid);

                var headers = new Dictionary<string, string>
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

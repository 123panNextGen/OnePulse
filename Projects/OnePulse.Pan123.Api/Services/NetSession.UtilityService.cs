using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using OnePulse.Pan123.Api.Data;
using OnePulse.Pan123.Api.Models;
using OnePulse.Pan123.Api.Models.Data;
using OnePulse.Pan123.Api.Models.UserInfo;

namespace OnePulse.Pan123.Api.Services
{
    public partial class NetSession
    {
        public class UtilityService
        {
            private readonly NetSession _session;
            private readonly Random _random = new();

            public DataInfo? DataInfo { get; private set; }
            private DeviceData? DeviceData { get; set; }
            public bool IsDataVerified { get; private set; }

            internal UtilityService(NetSession session)
            {
                _session = session;
            }

            public ApiReturn<string> LoadDataFiles(CancellationToken cancellationToken = default)
            {
                // 读取 DataInfo.json
                string dataInfoPath = Constants.Data.DataInfoPath;
                if (!File.Exists(dataInfoPath))
                    return new ApiReturn<string>(ApiResult.Failed, $"找不到文件: {dataInfoPath}");
                string dataInfoJson = File.ReadAllText(dataInfoPath, Encoding.UTF8);
                DataInfo? info;
                try
                {
                    info = JsonConvert.DeserializeObject<DataInfo>(dataInfoJson, JsonSettings);
                }
                catch (JsonException ex)
                {
                    return new ApiReturn<string>(
                        ApiResult.Failed,
                        $"DataInfo.json 解析失败: {ex.Message}"
                    );
                }
                if (info == null)
                    return new ApiReturn<string>(ApiResult.Failed, "DataInfo.json 内容为空");
                DataInfo = info;
                // 2. 遍历 dataContent，校验并加载每个数据文件
                foreach (DataContent content in info.DataContent)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return new ApiReturn<string>(ApiResult.Failed, "操作已取消");
                    string fullPath = Path.Combine(AppContext.BaseDirectory, content.Path);
                    if (!File.Exists(fullPath))
                        return new ApiReturn<string>(
                            ApiResult.Failed,
                            $"找不到数据文件: {fullPath}"
                        );
                    // 2a. SHA256 校验
                    string actualHash = ComputeSha256(fullPath);
                    if (!actualHash.Equals(content.Sha256, StringComparison.OrdinalIgnoreCase))
                    {
                        IsDataVerified = false;
                        return new ApiReturn<string>(
                            ApiResult.Failed,
                            $"文件 {content.Path} SHA256 校验失败\n期望: {content.Sha256}\n实际: {actualHash}"
                        );
                    }
                    // 按名称加载对应模型
                    string fileJson = File.ReadAllText(fullPath, Encoding.UTF8);
                    switch (content.Name)
                    {
                        case "DeviceData":
                            try
                            {
                                DeviceData = JsonConvert.DeserializeObject<DeviceData>(
                                    fileJson,
                                    JsonSettings
                                );
                            }
                            catch (JsonException ex)
                            {
                                return new ApiReturn<string>(
                                    ApiResult.Failed,
                                    $"DeviceData.json 解析失败: {ex.Message}"
                                );
                            }
                            break;
                    }
                }
                IsDataVerified = true;
                return new ApiReturn<string>(ApiResult.Success, "数据文件加载完成");
            }

            private static string ComputeSha256(string filePath)
            {
                using FileStream stream = File.OpenRead(filePath);
                using SHA256 sha256 = SHA256.Create();
                byte[] hash = sha256.ComputeHash(stream);
                return Convert.ToHexString(hash);
            }

            /// <summary>
            /// 随机获取一个设备类型 (DeviceData.type)
            /// </summary>
            public string? GetRandomDeviceType()
            {
                return DeviceData?.Types is not { Count: > 0 } list
                    ? null
                    : list[_random.Next(list.Count)];
            }

            /// <summary>
            /// 随机获取一个操作系统版本 (DeviceData.os)
            /// </summary>
            public string? GetRandomOs()
            {
                return DeviceData?.Os is not { Count: > 0 } list
                    ? null
                    : list[_random.Next(list.Count)];
            }

            public DeviceInfo GetRandomDeviceInfo()
            {
                return new DeviceInfo(
                    type: GetRandomDeviceType() ?? "Xiaomi17",
                    os: GetRandomOs() ?? "Android"
                );
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
                    ["user-agent"] = $"123pan/v2.4.0({userInfo.DeviceInfo.Os};Xiaomi)",
                    ["accept-encoding"] = "gzip",
                    ["content-type"] = "application/json",
                    ["osversion"] = userInfo.DeviceInfo.Os,
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

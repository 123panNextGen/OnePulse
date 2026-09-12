using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace OnePulse.Pan123.Api.Services
{
    public partial class NetSession
    {
        /// <summary>
        /// Newtonsoft.Json 全局序列化设置
        /// </summary>
        private static JsonSerializerSettings JsonSettings { get; } =
            new JsonSerializerSettings
            {
                // 保持属性名原样（不做驼峰/下划线转换）
                ContractResolver = new DefaultContractResolver(),
                // 忽略 null 值属性
                NullValueHandling = NullValueHandling.Ignore,
                // 不缩进输出
                Formatting = Formatting.None,
                // 枚举序列化为字符串
                Converters = { new StringEnumConverter() },
            };
    }
}

using System.Text.Json;
using System.Text.Json.Serialization;

namespace OnePulse.Pan123.Api.Services
{
    public partial class NetSession
    {
        public static JsonSerializerOptions Options { get; } =
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = null,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = false,
                Converters = { new JsonStringEnumConverter() },
            };
    }
}

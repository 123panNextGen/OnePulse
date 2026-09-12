using Newtonsoft.Json;

namespace OnePulse.Pan123.Api.Models.Data
{
    public class DeviceData
    {
        [JsonProperty("type")]
        public List<string> Types { get; set; } = [];

        [JsonProperty("os")]
        public List<string> Os { get; set; } = [];
    }
}
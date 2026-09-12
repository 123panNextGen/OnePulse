using Newtonsoft.Json;

namespace OnePulse.Pan123.Api.Models.Data
{
    public class DataInfo
    {
        [JsonProperty("metainfo")]
        public MetaInfo MetaInfo { get; set; } = new();

        [JsonProperty("dataContent")]
        public List<DataContent> DataContent { get; set; } = [];
    }

    public class MetaInfo
    {
        [JsonProperty("version")]
        public int Version { get; set; }
    }

    public class DataContent
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; } = string.Empty;

        [JsonProperty("sha256")]
        public string Sha256 { get; set; } = string.Empty;
    }
}
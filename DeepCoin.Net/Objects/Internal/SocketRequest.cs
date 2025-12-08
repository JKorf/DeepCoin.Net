using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Internal
{
    internal class SocketRequest
    {
        [JsonPropertyName("Action")]
        public string Action { get; set; } = string.Empty;
        [JsonPropertyName("FilterValue")]
        public string Topic { get; set; } = string.Empty;
        [JsonPropertyName("LocalNo")]
        public int RequestId { get; set; }
        [JsonPropertyName("ResumeNo")]
        public int ResumeNumber { get; set; }
        [JsonPropertyName("TopicID")]
        public string TopicId { get; set; } = string.Empty;
        [JsonPropertyName("BusinessNo"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int? BusinessNo { get; set; }
    }
}

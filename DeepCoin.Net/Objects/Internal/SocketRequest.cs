using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Internal
{
    internal class SocketRequest
    {
        [JsonPropertyName("Action")]
        public string Action { get; set; }
        [JsonPropertyName("FilterValue")]
        public string Topic { get; set; }
        [JsonPropertyName("LocalNo")]
        public int RequestId { get; set; }
        [JsonPropertyName("ResumeNo")]
        public int ResumeNumber { get; set; }
        [JsonPropertyName("TopicID")]
        public string TopicId { get; set; }
        [JsonPropertyName("BusinessNo"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int? BusinessNo { get; set; }
    }
}

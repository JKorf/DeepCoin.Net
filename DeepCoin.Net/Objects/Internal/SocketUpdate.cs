using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Internal
{
    internal record SocketUpdate<T>
    {
        [JsonPropertyName("action")]
        public string Action { get; set; }
        [JsonPropertyName("index")]
        public string Index { get; set; }
        [JsonPropertyName("bNo")]
        public long BusinessNumber { get; set; }
        [JsonPropertyName("changeType")]
        public string ChangeType { get; set; }
        [JsonPropertyName("result")]
        public IEnumerable<TableData<T>> Result { get; set; }
    }
}

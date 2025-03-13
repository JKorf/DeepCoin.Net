using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Internal
{
    [SerializationModel]
    internal record SocketUpdate<T>
    {
        [JsonPropertyName("action")]
        public string Action { get; set; } = string.Empty;
        [JsonPropertyName("index")]
        public string Index { get; set; } = string.Empty;
        [JsonPropertyName("bNo")]
        public long BusinessNumber { get; set; }
        [JsonPropertyName("changeType")]
        public string ChangeType { get; set; } = string.Empty;
        [JsonPropertyName("result")]
        public TableData<T>[] Result { get; set; } = [];
    }
}

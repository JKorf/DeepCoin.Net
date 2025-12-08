using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Internal
{
    [SerializationModel]
    internal record TableData<T>
    {
        [JsonPropertyName("table")]
        public string Table { get; set; } = string.Empty;
        [JsonPropertyName("data")]
        public T Data { get; set; } = default!;
    }
}

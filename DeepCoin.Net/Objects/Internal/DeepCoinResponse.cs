using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Internal
{
    /// <summary>
    /// DeepCoin response
    /// </summary>
    [SerializationModel]
    internal record DeepCoinResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("msg")]
        public string? Message { get; set; }
    }

    [SerializationModel]
    internal record DeepCoinResponse<T> : DeepCoinResponse
    {
        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }
}

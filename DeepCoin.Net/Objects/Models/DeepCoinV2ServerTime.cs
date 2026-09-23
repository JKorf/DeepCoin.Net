using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// V2 exchange server time.
    /// </summary>
    [SerializationModel]
    public record DeepCoinV2ServerTime
    {
        /// <summary>
        /// ["<c>ts</c>"] Current server time, expressed as Unix milliseconds on the wire.
        /// </summary>
        [JsonPropertyName("ts")]
        public DateTime Timestamp { get; set; }
    }
}

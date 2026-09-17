using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// V2 close-position result object inside the standard API response envelope.
    /// </summary>
    [SerializationModel]
    public record DeepCoinV2ClosePositionResponse
    {
        /// <summary>
        /// ["<c>data</c>"] Per-position results. A successful close can return an empty list; missing or null data is not an acknowledgement.
        /// </summary>
        [JsonPropertyName("data")]
        public DeepCoinV2ClosePositionResult[]? Results { get; set; }
    }
}

using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// V2 per-order lookup result.
    /// </summary>
    [SerializationModel]
    public record DeepCoinV2OrderQueryResult
    {
        /// <summary>
        /// ["<c>ordId</c>"] Order id.
        /// </summary>
        [JsonPropertyName("ordId")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>code</c>"] Result code; zero means success.
        /// </summary>
        [JsonPropertyName("code")]
        public int? ResultCode { get; set; }
        /// <summary>
        /// ["<c>msg</c>"] Result message.
        /// </summary>
        [JsonPropertyName("msg")]
        public string? ResultMessage { get; set; }
        /// <summary>
        /// ["<c>data</c>"] Order details when the lookup succeeds.
        /// </summary>
        [JsonPropertyName("data")]
        public DeepCoinOrder? Order { get; set; }
    }
}

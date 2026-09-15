using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net.Enums;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// V2 per-position closing result.
    /// </summary>
    [SerializationModel]
    public record DeepCoinV2ClosePositionResult
    {
        /// <summary>
        /// ["<c>instId</c>"] Product identifier.
        /// </summary>
        [JsonPropertyName("instId")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>posId</c>"] Position identifier.
        /// </summary>
        [JsonPropertyName("posId")]
        public string PositionId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>posSide</c>"] Position side.
        /// </summary>
        [JsonPropertyName("posSide")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// ["<c>memberId</c>"] User identifier.
        /// </summary>
        [JsonPropertyName("memberId")]
        public string UserId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>accountId</c>"] Account identifier.
        /// </summary>
        [JsonPropertyName("accountId")]
        public string AccountId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>tradeUnitId</c>"] Trading unit identifier.
        /// </summary>
        [JsonPropertyName("tradeUnitId")]
        public string TradeUnitId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>sCode</c>"] Result code; zero means success.
        /// </summary>
        [JsonPropertyName("sCode")]
        public int? ResultCode { get; set; }
        /// <summary>
        /// ["<c>sMsg</c>"] Result message.
        /// </summary>
        [JsonPropertyName("sMsg")]
        public string? ResultMessage { get; set; }
    }
}

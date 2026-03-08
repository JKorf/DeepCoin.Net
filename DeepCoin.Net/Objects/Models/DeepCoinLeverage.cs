using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net.Enums;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Leverage setting
    /// </summary>
    [SerializationModel]
    public record DeepCoinLeverage
    {
        /// <summary>
        /// ["<c>instId</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("instId")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>lever</c>"] Leverage
        /// </summary>
        [JsonPropertyName("lever")]
        public decimal Leverage { get; set; }
        /// <summary>
        /// ["<c>mgnMode</c>"] Margin mode
        /// </summary>
        [JsonPropertyName("mgnMode")]
        public TradeMode TradeMode { get; set; }
        /// <summary>
        /// ["<c>mrgPosition</c>"] Position type
        /// </summary>
        [JsonPropertyName("mrgPosition")]
        public PositionType PositionType { get; set; }
        /// <summary>
        /// ["<c>sCode</c>"] Result code
        /// </summary>
        [JsonPropertyName("sCode")]
        public int ResultCode { get; set; }
        /// <summary>
        /// ["<c>sMsg</c>"] Result message
        /// </summary>
        [JsonPropertyName("sMsg")]
        public string? ResultMessage { get; set; }
    } 
}

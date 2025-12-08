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
        /// Symbol name
        /// </summary>
        [JsonPropertyName("instId")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Leverage
        /// </summary>
        [JsonPropertyName("lever")]
        public decimal Leverage { get; set; }
        /// <summary>
        /// Margin mode
        /// </summary>
        [JsonPropertyName("mgnMode")]
        public TradeMode TradeMode { get; set; }
        /// <summary>
        /// Position type
        /// </summary>
        [JsonPropertyName("mrgPosition")]
        public PositionType PositionType { get; set; }
        /// <summary>
        /// Result code
        /// </summary>
        [JsonPropertyName("sCode")]
        public int ResultCode { get; set; }
        /// <summary>
        /// Result message
        /// </summary>
        [JsonPropertyName("sMsg")]
        public string? ResultMessage { get; set; }
    } 
}

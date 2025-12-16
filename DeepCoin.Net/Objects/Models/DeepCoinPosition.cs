using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Position info
    /// </summary>
    [SerializationModel]
    public record DeepCoinPosition
    {
        /// <summary>
        /// Symbol type
        /// </summary>
        [JsonPropertyName("instType")]
        public SymbolType SymbolType { get; set; }
        /// <summary>
        /// Margin mode
        /// </summary>
        [JsonPropertyName("mgnMode")]
        public TradeMode TradeMode { get; set; }
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("instId")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Position id
        /// </summary>
        [JsonPropertyName("posId")]
        public string PositionId { get; set; } = string.Empty;
        /// <summary>
        /// Position side
        /// </summary>
        [JsonPropertyName("posSide")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// Position size
        /// </summary>
        [JsonPropertyName("pos")]
        public decimal Size { get; set; }
        /// <summary>
        /// Average price
        /// </summary>
        [JsonPropertyName("avgPx")]
        public decimal AveragePrice { get; set; }
        /// <summary>
        /// Leverage
        /// </summary>
        [JsonPropertyName("lever")]
        public decimal Leverage { get; set; }
        /// <summary>
        /// Liquidation price
        /// </summary>
        [JsonPropertyName("liqPx")]
        public decimal LiquidationPrice { get; set; }
        /// <summary>
        /// Used margin
        /// </summary>
        [JsonPropertyName("useMargin")]
        public decimal UsedMargin { get; set; }
        /// <summary>
        /// Position type
        /// </summary>
        [JsonPropertyName("mrgPosition")]
        public PositionType PositionType { get; set; }
        /// <summary>
        /// Margin asset
        /// </summary>
        [JsonPropertyName("ccy")]
        public string MarginAsset { get; set; } = string.Empty;
        /// <summary>
        /// Update time
        /// </summary>
        [JsonPropertyName("uTime")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// Create time
        /// </summary>
        [JsonPropertyName("cTime")]
        public DateTime CreateTime { get; set; }
    }


}

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
        /// ["<c>instType</c>"] Symbol type
        /// </summary>
        [JsonPropertyName("instType")]
        public SymbolType SymbolType { get; set; }
        /// <summary>
        /// ["<c>mgnMode</c>"] Margin mode
        /// </summary>
        [JsonPropertyName("mgnMode")]
        public TradeMode TradeMode { get; set; }
        /// <summary>
        /// ["<c>instId</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("instId")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>posId</c>"] Position id
        /// </summary>
        [JsonPropertyName("posId")]
        public string PositionId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>posSide</c>"] Position side
        /// </summary>
        [JsonPropertyName("posSide")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// ["<c>pos</c>"] Position size
        /// </summary>
        [JsonPropertyName("pos")]
        public decimal Size { get; set; }
        /// <summary>
        /// ["<c>avgPx</c>"] Average price
        /// </summary>
        [JsonPropertyName("avgPx")]
        public decimal AveragePrice { get; set; }
        /// <summary>
        /// ["<c>lever</c>"] Leverage
        /// </summary>
        [JsonPropertyName("lever")]
        public decimal Leverage { get; set; }
        /// <summary>
        /// ["<c>liqPx</c>"] Liquidation price
        /// </summary>
        [JsonPropertyName("liqPx")]
        public decimal LiquidationPrice { get; set; }
        /// <summary>
        /// ["<c>useMargin</c>"] Used margin
        /// </summary>
        [JsonPropertyName("useMargin")]
        public decimal UsedMargin { get; set; }
        /// <summary>
        /// ["<c>mrgPosition</c>"] Position type
        /// </summary>
        [JsonPropertyName("mrgPosition")]
        public PositionType PositionType { get; set; }
        /// <summary>
        /// ["<c>ccy</c>"] Margin asset
        /// </summary>
        [JsonPropertyName("ccy")]
        public string MarginAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>uTime</c>"] Update time
        /// </summary>
        [JsonPropertyName("uTime")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// ["<c>cTime</c>"] Create time
        /// </summary>
        [JsonPropertyName("cTime")]
        public DateTime CreateTime { get; set; }
    }


}

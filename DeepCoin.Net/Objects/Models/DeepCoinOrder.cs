using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Order info
    /// </summary>
    [SerializationModel]
    public record DeepCoinOrder
    {
        /// <summary>
        /// Symbol type
        /// </summary>
        [JsonPropertyName("instType")]
        public SymbolType SymbolType { get; set; }
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("instId")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Type of order quantity
        /// </summary>
        [JsonPropertyName("tgtCcy")]
        public QuantityType? QuantityType { get; set; }
        /// <summary>
        /// Margin asset
        /// </summary>
        [JsonPropertyName("ccy")]
        public string? MarginAsset { get; set; }
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("ordId")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("clOrdId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Tag
        /// </summary>
        [JsonPropertyName("tag")]
        public string? Tag { get; set; }
        /// <summary>
        /// Order price
        /// </summary>
        [JsonPropertyName("px")]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("sz")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Profit and loss
        /// </summary>
        [JsonPropertyName("pnl")]
        public decimal Pnl { get; set; }
        /// <summary>
        /// Order type
        /// </summary>
        [JsonPropertyName("ordType")]
        public OrderType OrderType { get; set; }
        /// <summary>
        /// Order side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide OrderSide { get; set; }
        /// <summary>
        /// Position side
        /// </summary>
        [JsonPropertyName("posSide")]
        public PositionSide? PositionSide { get; set; }
        /// <summary>
        /// Margin mode
        /// </summary>
        [JsonPropertyName("tdMode")]
        public TradeMode TradeMode { get; set; }
        /// <summary>
        /// Quantity filled
        /// </summary>
        [JsonPropertyName("accFillSz")]
        public decimal QuantityFilled { get; set; }
        /// <summary>
        /// Last trade price
        /// </summary>
        [JsonPropertyName("fillPx")]
        public decimal? LastTradePrice { get; set; }
        /// <summary>
        /// Last trade id
        /// </summary>
        [JsonPropertyName("tradeId")]
        public string? LastTradeId { get; set; }
        /// <summary>
        /// Last trade fill quantity
        /// </summary>
        [JsonPropertyName("fillSz")]
        public decimal? LastTradeQuantity { get; set; }
        /// <summary>
        /// Last trade timestamp
        /// </summary>
        [JsonPropertyName("fillTime")]
        public DateTime? LastTradeTime { get; set; }
        /// <summary>
        /// Average fill price
        /// </summary>
        [JsonPropertyName("avgPx")]
        public decimal? AverageFillPrice { get; set; }
        /// <summary>
        /// Order status
        /// </summary>
        [JsonPropertyName("state")]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// Leverage
        /// </summary>
        [JsonPropertyName("lever")]
        public decimal? Leverage { get; set; }
        /// <summary>
        /// Take profit trigger price
        /// </summary>
        [JsonPropertyName("tpTriggerPx")]
        public decimal? TpTriggerPrice { get; set; }
        /// <summary>
        /// Take profit trigger price type
        /// </summary>
        [JsonPropertyName("tpTriggerPxType")]
        public PriceType? TpTriggerPriceType { get; set; }
        /// <summary>
        /// Take profit order price
        /// </summary>
        [JsonPropertyName("tpOrdPx")]
        public decimal? TpOrderPrice { get; set; }
        /// <summary>
        /// Stop loss trigger price
        /// </summary>
        [JsonPropertyName("slTriggerPx")]
        public decimal? SlTriggerPrice { get; set; }
        /// <summary>
        /// Stop loss trigger price type
        /// </summary>
        [JsonPropertyName("slTriggerPxType")]
        public PriceType? SlTriggerPriceType { get; set; }
        /// <summary>
        /// Stop loss order price
        /// </summary>
        [JsonPropertyName("slOrdPx")]
        public decimal? SlOrderPrice { get; set; }
        /// <summary>
        /// Fee asset
        /// </summary>
        [JsonPropertyName("feeCcy")]
        public string FeeAsset { get; set; } = string.Empty;
        /// <summary>
        /// Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// Rebate asset
        /// </summary>
        [JsonPropertyName("rebateCcy")]
        public string? RebateAsset { get; set; }
        /// <summary>
        /// Source
        /// </summary>
        [JsonPropertyName("source")]
        public string? Source { get; set; }
        /// <summary>
        /// Rebate quantity
        /// </summary>
        [JsonPropertyName("rebate")]
        public decimal? Rebate { get; set; }
        /// <summary>
        /// Category
        /// </summary>
        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;
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

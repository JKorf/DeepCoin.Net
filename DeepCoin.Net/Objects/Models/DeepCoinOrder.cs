using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net.Enums;
using System;
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
        /// ["<c>instType</c>"] Symbol type
        /// </summary>
        [JsonPropertyName("instType")]
        public SymbolType SymbolType { get; set; }
        /// <summary>
        /// ["<c>instId</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("instId")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>tgtCcy</c>"] Type of order quantity
        /// </summary>
        [JsonPropertyName("tgtCcy")]
        public QuantityType? QuantityType { get; set; }
        /// <summary>
        /// ["<c>ccy</c>"] Margin asset
        /// </summary>
        [JsonPropertyName("ccy")]
        public string? MarginAsset { get; set; }
        /// <summary>
        /// ["<c>ordId</c>"] Order id
        /// </summary>
        [JsonPropertyName("ordId")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>clOrdId</c>"] Client order id
        /// </summary>
        [JsonPropertyName("clOrdId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// ["<c>tag</c>"] Tag
        /// </summary>
        [JsonPropertyName("tag")]
        public string? Tag { get; set; }
        /// <summary>
        /// ["<c>px</c>"] Order price
        /// </summary>
        [JsonPropertyName("px")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>sz</c>"] Quantity
        /// </summary>
        [JsonPropertyName("sz")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>pnl</c>"] Profit and loss
        /// </summary>
        [JsonPropertyName("pnl")]
        public decimal Pnl { get; set; }
        /// <summary>
        /// ["<c>ordType</c>"] Order type
        /// </summary>
        [JsonPropertyName("ordType")]
        public OrderType OrderType { get; set; }
        /// <summary>
        /// ["<c>side</c>"] Order side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide OrderSide { get; set; }
        /// <summary>
        /// ["<c>posSide</c>"] Position side
        /// </summary>
        [JsonPropertyName("posSide")]
        public PositionSide? PositionSide { get; set; }
        /// <summary>
        /// ["<c>tdMode</c>"] Margin mode
        /// </summary>
        [JsonPropertyName("tdMode")]
        public TradeMode TradeMode { get; set; }
        /// <summary>
        /// ["<c>accFillSz</c>"] Quantity filled
        /// </summary>
        [JsonPropertyName("accFillSz")]
        public decimal QuantityFilled { get; set; }
        /// <summary>
        /// ["<c>fillPx</c>"] Last trade price
        /// </summary>
        [JsonPropertyName("fillPx")]
        public decimal? LastTradePrice { get; set; }
        /// <summary>
        /// ["<c>tradeId</c>"] Last trade id
        /// </summary>
        [JsonPropertyName("tradeId")]
        public string? LastTradeId { get; set; }
        /// <summary>
        /// ["<c>fillSz</c>"] Last trade fill quantity
        /// </summary>
        [JsonPropertyName("fillSz")]
        public decimal? LastTradeQuantity { get; set; }
        /// <summary>
        /// ["<c>fillTime</c>"] Last trade timestamp
        /// </summary>
        [JsonPropertyName("fillTime")]
        public DateTime? LastTradeTime { get; set; }
        /// <summary>
        /// ["<c>avgPx</c>"] Average fill price
        /// </summary>
        [JsonPropertyName("avgPx")]
        public decimal? AverageFillPrice { get; set; }
        /// <summary>
        /// ["<c>state</c>"] Order status
        /// </summary>
        [JsonPropertyName("state")]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// ["<c>lever</c>"] Leverage
        /// </summary>
        [JsonPropertyName("lever")]
        public decimal? Leverage { get; set; }
        /// <summary>
        /// ["<c>tpTriggerPx</c>"] Take profit trigger price
        /// </summary>
        [JsonPropertyName("tpTriggerPx")]
        public decimal? TpTriggerPrice { get; set; }
        /// <summary>
        /// ["<c>tpTriggerPxType</c>"] Take profit trigger price type
        /// </summary>
        [JsonPropertyName("tpTriggerPxType")]
        public PriceType? TpTriggerPriceType { get; set; }
        /// <summary>
        /// ["<c>tpOrdPx</c>"] Take profit order price
        /// </summary>
        [JsonPropertyName("tpOrdPx")]
        public decimal? TpOrderPrice { get; set; }
        /// <summary>
        /// ["<c>slTriggerPx</c>"] Stop loss trigger price
        /// </summary>
        [JsonPropertyName("slTriggerPx")]
        public decimal? SlTriggerPrice { get; set; }
        /// <summary>
        /// ["<c>slTriggerPxType</c>"] Stop loss trigger price type
        /// </summary>
        [JsonPropertyName("slTriggerPxType")]
        public PriceType? SlTriggerPriceType { get; set; }
        /// <summary>
        /// ["<c>slOrdPx</c>"] Stop loss order price
        /// </summary>
        [JsonPropertyName("slOrdPx")]
        public decimal? SlOrderPrice { get; set; }
        /// <summary>
        /// ["<c>feeCcy</c>"] Fee asset
        /// </summary>
        [JsonPropertyName("feeCcy")]
        public string FeeAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>fee</c>"] Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// ["<c>rebateCcy</c>"] Rebate asset
        /// </summary>
        [JsonPropertyName("rebateCcy")]
        public string? RebateAsset { get; set; }
        /// <summary>
        /// ["<c>source</c>"] Source
        /// </summary>
        [JsonPropertyName("source")]
        public string? Source { get; set; }
        /// <summary>
        /// ["<c>rebate</c>"] Rebate quantity
        /// </summary>
        [JsonPropertyName("rebate")]
        public decimal? Rebate { get; set; }
        /// <summary>
        /// ["<c>category</c>"] Category
        /// </summary>
        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;
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

using DeepCoin.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// User trade info
    /// </summary>
    public record DeepCoinUserTrade
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
        /// Trade id
        /// </summary>
        [JsonPropertyName("tradeId")]
        public string TradeId { get; set; } = string.Empty;
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
        /// Bill id
        /// </summary>
        [JsonPropertyName("billId")]
        public string BillId { get; set; } = string.Empty;
        /// <summary>
        /// Tag
        /// </summary>
        [JsonPropertyName("tag")]
        public string? Tag { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("fillPx")]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("fillSz")]
        public decimal Quantity { get; set; }
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
        /// Trade role
        /// </summary>
        [JsonPropertyName("execType")]
        public TradeRole? Role { get; set; }
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
        /// Timestamp
        /// </summary>
        [JsonPropertyName("ts")]
        public DateTime Timestamp { get; set; }
    }


}

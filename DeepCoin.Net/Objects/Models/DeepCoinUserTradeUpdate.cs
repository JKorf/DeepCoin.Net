using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// User trade update
    /// </summary>
    [SerializationModel]
    public record DeepCoinUserTradeUpdate
    {
        /// <summary>
        /// Account id
        /// </summary>
        [JsonPropertyName("A")]
        public string AccountId { get; set; } = string.Empty;
        /// <summary>
        /// Clearing asset
        /// </summary>
        [JsonPropertyName("CC")]
        public string ClearingAsset { get; set; } = string.Empty;
        /// <summary>
        /// Side
        /// </summary>
        [JsonPropertyName("D")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Fee quantity
        /// </summary>
        [JsonPropertyName("F")]
        public decimal Fee { get; set; }
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("I")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Create time
        /// </summary>
        [JsonPropertyName("IT")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// User id
        /// </summary>
        [JsonPropertyName("M")]
        public string UserId { get; set; } = string.Empty;
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("OS")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Trade price
        /// </summary>
        [JsonPropertyName("P")]
        public decimal Price { get; set; }
        /// <summary>
        /// Turnover
        /// </summary>
        [JsonPropertyName("T")]
        public decimal Turnover { get; set; }
        /// <summary>
        /// Trade id
        /// </summary>
        [JsonPropertyName("TI")]
        public string TradeId { get; set; } = string.Empty;
        /// <summary>
        /// Trade timestamp
        /// </summary>
        [JsonPropertyName("TT")]
        public DateTime TradeTime { get; set; }
        /// <summary>
        /// Trade quantity
        /// </summary>
        [JsonPropertyName("V")]
        public decimal Quantity { get; set; }
        ///// <summary>
        ///// C
        ///// </summary>
        //[JsonPropertyName("c")]
        //public decimal C { get; set; }
        /// <summary>
        /// Fee asset
        /// </summary>
        [JsonPropertyName("f")]
        public string FeeAsset { get; set; } = string.Empty;
        /// <summary>
        /// Leverage
        /// </summary>
        [JsonPropertyName("l")]
        public decimal Leverage { get; set; }
        /// <summary>
        /// Trade role
        /// </summary>
        [JsonPropertyName("m")]
        public TradeRole TradeRole { get; set; }
        /// <summary>
        /// OffsetFlag
        /// </summary>
        [JsonPropertyName("o")]
        public string OffsetFlag { get; set; } = string.Empty;
    }


}

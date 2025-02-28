using DeepCoin.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Order update
    /// </summary>
    public record DeepCoinOrderUpdate
    {
        /// <summary>
        /// Order side
        /// </summary>
        [JsonPropertyName("D")]
        public OrderSide OrderSide { get; set; }
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
        /// LocalId
        /// </summary>
        [JsonPropertyName("L")]
        public string LocalId { get; set; } = string.Empty;
        ///// <summary>
        ///// O
        ///// </summary>
        //[JsonPropertyName("O")]
        //public decimal O { get; set; }
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("OS")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Order type
        /// </summary>
        [JsonPropertyName("OT")]
        public OrderType OrderType { get; set; }
        /// <summary>
        /// Order status
        /// </summary>
        [JsonPropertyName("Or")]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// Order price
        /// </summary>
        [JsonPropertyName("P")]
        public decimal? OrderPrice { get; set; }
        /// <summary>
        /// Turnover
        /// </summary>
        [JsonPropertyName("T")]
        public decimal Turnover { get; set; }
        /// <summary>
        /// UpdateTime
        /// </summary>
        [JsonPropertyName("U")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// Order quantity
        /// </summary>
        [JsonPropertyName("V")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Is cross margin
        /// </summary>
        [JsonPropertyName("i")]
        public bool IsCrossMargin { get; set; }
        /// <summary>
        /// Leverage
        /// </summary>
        [JsonPropertyName("l")]
        public decimal Leverage { get; set; }
        /// <summary>
        /// OffsetFlag
        /// </summary>
        [JsonPropertyName("o")]
        public string OffsetFlag { get; set; } = string.Empty;
        /// <summary>
        /// Position side
        /// </summary>
        [JsonPropertyName("p")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// Average fill price
        /// </summary>
        [JsonPropertyName("t")]
        public decimal? AverageFillPrice { get; set; }
        /// <summary>
        /// Quantity filled
        /// </summary>
        [JsonPropertyName("v")]
        public decimal QuantityFilled { get; set; }
    }


}

using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Order update
    /// </summary>
    [SerializationModel]
    public record DeepCoinOrderUpdate
    {
        /// <summary>
        /// ["<c>D</c>"] Order side
        /// </summary>
        [JsonPropertyName("D")]
        public OrderSide OrderSide { get; set; }
        /// <summary>
        /// ["<c>I</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("I")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>IT</c>"] Create time
        /// </summary>
        [JsonPropertyName("IT")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>L</c>"] LocalId
        /// </summary>
        [JsonPropertyName("L")]
        public string LocalId { get; set; } = string.Empty;
        ///// <summary>
        ///// O
        ///// </summary>
        //[JsonPropertyName("O")]
        //public decimal O { get; set; }
        /// <summary>
        /// ["<c>OS</c>"] Order id
        /// </summary>
        [JsonPropertyName("OS")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>OT</c>"] Order type
        /// </summary>
        [JsonPropertyName("OT")]
        public OrderType OrderType { get; set; }
        /// <summary>
        /// ["<c>Or</c>"] Order status
        /// </summary>
        [JsonPropertyName("Or")]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// ["<c>P</c>"] Order price
        /// </summary>
        [JsonPropertyName("P")]
        public decimal? OrderPrice { get; set; }
        /// <summary>
        /// ["<c>T</c>"] Turnover
        /// </summary>
        [JsonPropertyName("T")]
        public decimal Turnover { get; set; }
        /// <summary>
        /// ["<c>U</c>"] UpdateTime
        /// </summary>
        [JsonPropertyName("U")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// ["<c>V</c>"] Order quantity
        /// </summary>
        [JsonPropertyName("V")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>i</c>"] Is cross margin
        /// </summary>
        [JsonPropertyName("i")]
        public bool IsCrossMargin { get; set; }
        /// <summary>
        /// ["<c>l</c>"] Leverage
        /// </summary>
        [JsonPropertyName("l")]
        public decimal Leverage { get; set; }
        /// <summary>
        /// ["<c>o</c>"] OffsetFlag
        /// </summary>
        [JsonPropertyName("o")]
        public string OffsetFlag { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>p</c>"] Position side
        /// </summary>
        [JsonPropertyName("p")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// ["<c>t</c>"] Average fill price
        /// </summary>
        [JsonPropertyName("t")]
        public decimal? AverageFillPrice { get; set; }
        /// <summary>
        /// ["<c>VT</c>"] Quantity filled
        /// </summary>
        [JsonPropertyName("VT")]
        public decimal QuantityFilled { get; set; }
    }


}

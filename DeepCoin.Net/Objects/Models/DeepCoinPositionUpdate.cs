using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Position update
    /// </summary>
    [SerializationModel]
    public record DeepCoinPositionUpdate
    {
        /// <summary>
        /// Account id
        /// </summary>
        [JsonPropertyName("A")]
        public string AccountId { get; set; } = string.Empty;
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("I")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// User id
        /// </summary>
        [JsonPropertyName("M")]
        public string UserId { get; set; } = string.Empty;
        /// <summary>
        /// Open price
        /// </summary>
        [JsonPropertyName("OP")]
        public decimal? OpenPrice { get; set; }
        /// <summary>
        /// Position size
        /// </summary>
        [JsonPropertyName("Po")]
        public decimal PositionSize { get; set; }
        /// <summary>
        /// Update time
        /// </summary>
        [JsonPropertyName("U")]
        public DateTime UpdateTime { get; set; }
        ///// <summary>
        ///// C
        ///// </summary>
        //[JsonPropertyName("c")]
        //public decimal C { get; set; }
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
        /// PositionSide
        /// </summary>
        [JsonPropertyName("p")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// Margin used
        /// </summary>
        [JsonPropertyName("u")]
        public decimal MarginUsed { get; set; }
    }


}

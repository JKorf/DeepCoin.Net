using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net.Enums;
using System;
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
        /// ["<c>A</c>"] Account id
        /// </summary>
        [JsonPropertyName("A")]
        public string AccountId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>I</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("I")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>M</c>"] User id
        /// </summary>
        [JsonPropertyName("M")]
        public string UserId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>OP</c>"] Open price
        /// </summary>
        [JsonPropertyName("OP")]
        public decimal? OpenPrice { get; set; }
        /// <summary>
        /// ["<c>Po</c>"] Position size
        /// </summary>
        [JsonPropertyName("Po")]
        public decimal PositionSize { get; set; }
        /// <summary>
        /// ["<c>U</c>"] Update time
        /// </summary>
        [JsonPropertyName("U")]
        public DateTime UpdateTime { get; set; }
        ///// <summary>
        ///// C
        ///// </summary>
        //[JsonPropertyName("c")]
        //public decimal C { get; set; }
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
        /// ["<c>p</c>"] PositionSide
        /// </summary>
        [JsonPropertyName("p")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// ["<c>u</c>"] Margin used
        /// </summary>
        [JsonPropertyName("u")]
        public decimal MarginUsed { get; set; }
    }


}

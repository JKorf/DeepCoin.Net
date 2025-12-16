using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Trigger order update
    /// </summary>
    [SerializationModel]
    public record DeepCoinTriggerOrderUpdate
    {
        /// <summary>
        /// Account id
        /// </summary>
        [JsonPropertyName("A")]
        public string AccountId { get; set; } = string.Empty;
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
        /// User id
        /// </summary>
        [JsonPropertyName("M")]
        public string UserId { get; set; } = string.Empty;
        /// <summary>
        /// O
        /// </summary>
        [JsonPropertyName("O")]
        public decimal O { get; set; }
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
        /// Trigger order type
        /// </summary>
        [JsonPropertyName("TO")]
        public string TriggerOrderType { get; set; } = string.Empty;
        /// <summary>
        /// Take profit price
        /// </summary>
        [JsonPropertyName("TP")]
        public decimal? TpPrice { get; set; }
        /// <summary>
        /// Take profit trigger price
        /// </summary>
        [JsonPropertyName("TPT")]
        public decimal? TpTriggerPrice { get; set; }
        /// <summary>
        /// Trigger status
        /// </summary>
        [JsonPropertyName("TS")]
        public string TriggerStatus { get; set; } = string.Empty;
        /// <summary>
        /// Position id
        /// </summary>
        [JsonPropertyName("TU")]
        public long PositionId { get; set; }
        /// <summary>
        /// Take profit price type
        /// </summary>
        [JsonPropertyName("Tr")]
        public string TpPriceType { get; set; } = string.Empty;
        /// <summary>
        /// Update time
        /// </summary>
        [JsonPropertyName("U")]
        public DateTime? UpdateTime { get; set; }
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
    }


}

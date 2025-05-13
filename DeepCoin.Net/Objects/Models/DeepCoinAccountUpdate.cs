using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Account update
    /// </summary>
    [SerializationModel]
    public record DeepCoinAccountUpdate
    {
        /// <summary>
        /// Account id
        /// </summary>
        [JsonPropertyName("A")]
        public string AccountId { get; set; } = string.Empty;
        /// <summary>
        /// Account detail id
        /// </summary>
        [JsonPropertyName("AD")]
        public string AccountDetailId { get; set; } = string.Empty;
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("Am")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Static balance
        /// </summary>
        [JsonPropertyName("B")]
        public decimal Balance { get; set; }
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("C")]
        public string Asset { get; set; } = string.Empty;
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
        public decimal UserId { get; set; }
        /// <summary>
        /// Pre-change balance
        /// </summary>
        [JsonPropertyName("PB")]
        public decimal PreBalance { get; set; }
        /// <summary>
        /// Remark
        /// </summary>
        [JsonPropertyName("R")]
        public string? Remark { get; set; }
        /// <summary>
        /// Transaction type
        /// </summary>
        [JsonPropertyName("S")]
        public string TransactionType { get; set; } = string.Empty;
        /// <summary>
        /// Related change id
        /// </summary>
        [JsonPropertyName("r")]
        public string RelatedId { get; set; } = string.Empty;
    }


}

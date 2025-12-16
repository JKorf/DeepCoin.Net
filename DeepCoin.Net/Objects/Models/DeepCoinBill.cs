using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Bill info
    /// </summary>
    [SerializationModel]
    public record DeepCoinBill
    {
        /// <summary>
        /// Bill id
        /// </summary>
        [JsonPropertyName("billId")]
        public string BillId { get; set; } = string.Empty;
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("ccy")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Client id
        /// </summary>
        [JsonPropertyName("clientId")]
        public string? ClientId { get; set; }
        /// <summary>
        /// Balance change
        /// </summary>
        [JsonPropertyName("balChg")]
        public decimal BalanceChange { get; set; }
        /// <summary>
        /// Balance
        /// </summary>
        [JsonPropertyName("bal")]
        public decimal Balance { get; set; }
        /// <summary>
        /// Bill type
        /// </summary>
        [JsonPropertyName("type")]
        public BillType Type { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("ts")]
        public DateTime Timestamp { get; set; }
    }
}

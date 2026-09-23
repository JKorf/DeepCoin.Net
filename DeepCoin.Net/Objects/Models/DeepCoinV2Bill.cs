using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// V2 bill info
    /// </summary>
    [SerializationModel]
    public record DeepCoinV2Bill
    {
        /// <summary>
        /// ["<c>billId</c>"] Bill id
        /// </summary>
        [JsonPropertyName("billId")]
        public string BillId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>ccy</c>"] Asset
        /// </summary>
        [JsonPropertyName("ccy")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>clientId</c>"] Client id
        /// </summary>
        [JsonPropertyName("clientId")]
        public string? ClientId { get; set; }
        /// <summary>
        /// ["<c>balChg</c>"] Balance change
        /// </summary>
        [JsonPropertyName("balChg")]
        public decimal BalanceChange { get; set; }
        /// <summary>
        /// ["<c>bal</c>"] Balance
        /// </summary>
        [JsonPropertyName("bal")]
        public decimal Balance { get; set; }
        /// <summary>
        /// ["<c>type</c>"] Native bill source. Response sources differ from the request's BillType filter categories.
        /// </summary>
        [JsonPropertyName("type")]
        public V2BillType Type { get; set; }
        /// <summary>
        /// ["<c>ts</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("ts")]
        public DateTime Timestamp { get; set; }
    }
}

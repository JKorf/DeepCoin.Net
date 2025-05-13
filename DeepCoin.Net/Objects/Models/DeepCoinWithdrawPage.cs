using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Withdrawal history page
    /// </summary>
    [SerializationModel]
    public record DeepCoinWithdrawPage
    {
        /// <summary>
        /// Data
        /// </summary>
        [JsonPropertyName("data")]
        public DeepCoinWithdrawal[] Data { get; set; } = Array.Empty<DeepCoinWithdrawal>();
        /// <summary>
        /// Total count
        /// </summary>
        [JsonPropertyName("count")]
        public int Total { get; set; }
        /// <summary>
        /// Page
        /// </summary>
        [JsonPropertyName("page")]
        public int Page { get; set; }
        /// <summary>
        /// Page size
        /// </summary>
        [JsonPropertyName("size")]
        public int PageSize { get; set; }
    }

    /// <summary>
    /// Withdraw info
    /// </summary>
    [SerializationModel]
    public record DeepCoinWithdrawal
    {
        /// <summary>
        /// Create time
        /// </summary>
        [JsonPropertyName("createTime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Transaction hash
        /// </summary>
        [JsonPropertyName("txHash")]
        public string TransactionHash { get; set; } = string.Empty;
        /// <summary>
        /// Address
        /// </summary>
        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("coin")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Withdraw status
        /// </summary>
        [JsonPropertyName("status")]
        public WithdrawStatus DepositStatus { get; set; }
    }
}

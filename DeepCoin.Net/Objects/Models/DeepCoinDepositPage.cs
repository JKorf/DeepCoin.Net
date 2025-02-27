using DeepCoin.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Deposit history page
    /// </summary>
    public record DeepCoinDepositPage
    {
        /// <summary>
        /// Data
        /// </summary>
        [JsonPropertyName("data")]
        public IEnumerable<DeepCoinDeposit> Data { get; set; } = Array.Empty<DeepCoinDeposit>();
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
    /// Deposit info
    /// </summary>
    public record DeepCoinDeposit
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
        /// Network name
        /// </summary>
        [JsonPropertyName("chainName")]
        public string NetworkName { get; set; } = string.Empty;
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
        /// Deposit status
        /// </summary>
        [JsonPropertyName("status")]
        public DepositStatus DepositStatus { get; set; }
    }


}

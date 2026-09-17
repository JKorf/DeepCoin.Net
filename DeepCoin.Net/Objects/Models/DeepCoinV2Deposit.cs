using System;
using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net.Enums;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// V2 deposit record.
    /// </summary>
    [SerializationModel]
    public record DeepCoinV2Deposit
    {
        /// <summary>
        /// ["<c>createTime</c>"] On-chain confirmation time.
        /// </summary>
        [JsonPropertyName("createTime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>txHash</c>"] Transaction hash.
        /// </summary>
        [JsonPropertyName("txHash")]
        public string TransactionHash { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>chainName</c>"] Chain name.
        /// </summary>
        [JsonPropertyName("chainName")]
        public string NetworkName { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>amount</c>"] Deposited quantity.
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>ccy</c>"] Currency.
        /// </summary>
        [JsonPropertyName("ccy")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>status</c>"] Deposit status.
        /// </summary>
        [JsonPropertyName("status")]
        public DepositStatus Status { get; set; }
    }
}

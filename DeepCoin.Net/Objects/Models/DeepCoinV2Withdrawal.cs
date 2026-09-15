using System;
using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net.Enums;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// V2 withdrawal record.
    /// </summary>
    [SerializationModel]
    public record DeepCoinV2Withdrawal
    {
        /// <summary>
        /// ["<c>wdId</c>"] Withdrawal identifier.
        /// </summary>
        [JsonPropertyName("wdId")]
        public string WithdrawalId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>clientId</c>"] Client request identifier.
        /// </summary>
        [JsonPropertyName("clientId")]
        public string? ClientId { get; set; }
        /// <summary>
        /// ["<c>ccy</c>"] Currency.
        /// </summary>
        [JsonPropertyName("ccy")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>chain</c>"] Chain identifier.
        /// </summary>
        [JsonPropertyName("chain")]
        public string Network { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>amt</c>"] Withdrawal quantity.
        /// </summary>
        [JsonPropertyName("amt")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>fee</c>"] Withdrawal fee.
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// ["<c>toAddr</c>"] Withdrawal address.
        /// </summary>
        [JsonPropertyName("toAddr")]
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>memo</c>"] Memo or tag.
        /// </summary>
        [JsonPropertyName("memo")]
        public string? Memo { get; set; }
        /// <summary>
        /// ["<c>txId</c>"] Transaction hash.
        /// </summary>
        [JsonPropertyName("txId")]
        public string TransactionHash { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>state</c>"] Normalized withdrawal status. An omitted state remains undefined.
        /// </summary>
        [JsonPropertyName("state")]
        public V2WithdrawStatus Status { get; set; } = (V2WithdrawStatus)(-9);
        /// <summary>
        /// ["<c>canCancel</c>"] Whether cancellation is currently available.
        /// </summary>
        [JsonPropertyName("canCancel")]
        public bool CanCancel { get; set; }
        /// <summary>
        /// ["<c>cTime</c>"] Creation time in milliseconds.
        /// </summary>
        [JsonPropertyName("cTime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>uTime</c>"] Last update time in milliseconds.
        /// </summary>
        [JsonPropertyName("uTime")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// ["<c>createTime</c>"] Compatibility creation time.
        /// </summary>
        [JsonPropertyName("createTime")]
        public DateTime? LegacyCreateTime { get; set; }
        /// <summary>
        /// ["<c>txHash</c>"] Compatibility transaction hash.
        /// </summary>
        [JsonPropertyName("txHash")]
        public string? LegacyTransactionHash { get; set; }
        /// <summary>
        /// ["<c>address</c>"] Compatibility withdrawal address.
        /// </summary>
        [JsonPropertyName("address")]
        public string? LegacyAddress { get; set; }
        /// <summary>
        /// ["<c>amount</c>"] Compatibility quantity.
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal? LegacyQuantity { get; set; }
        /// <summary>
        /// ["<c>coin</c>"] Compatibility currency.
        /// </summary>
        [JsonPropertyName("coin")]
        public string? LegacyAsset { get; set; }
        /// <summary>
        /// ["<c>status</c>"] Compatibility status.
        /// </summary>
        [JsonPropertyName("status")]
        public V2WithdrawStatus? LegacyStatus { get; set; }
    }
}

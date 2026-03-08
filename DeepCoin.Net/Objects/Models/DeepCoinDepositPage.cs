using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Deposit history page
    /// </summary>
    [SerializationModel]
    public record DeepCoinDepositPage
    {
        /// <summary>
        /// ["<c>data</c>"] Data
        /// </summary>
        [JsonPropertyName("data")]
        public DeepCoinDeposit[] Data { get; set; } = Array.Empty<DeepCoinDeposit>();
        /// <summary>
        /// ["<c>count</c>"] Total count
        /// </summary>
        [JsonPropertyName("count")]
        public int Total { get; set; }
        /// <summary>
        /// ["<c>page</c>"] Page
        /// </summary>
        [JsonPropertyName("page")]
        public int Page { get; set; }
        /// <summary>
        /// ["<c>size</c>"] Page size
        /// </summary>
        [JsonPropertyName("size")]
        public int PageSize { get; set; }
    }

    /// <summary>
    /// Deposit info
    /// </summary>
    [SerializationModel]
    public record DeepCoinDeposit
    {
        /// <summary>
        /// ["<c>createTime</c>"] Create time
        /// </summary>
        [JsonPropertyName("createTime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>txHash</c>"] Transaction hash
        /// </summary>
        [JsonPropertyName("txHash")]
        public string TransactionHash { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>chainName</c>"] Network name
        /// </summary>
        [JsonPropertyName("chainName")]
        public string NetworkName { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>amount</c>"] Quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>coin</c>"] Asset
        /// </summary>
        [JsonPropertyName("coin")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>status</c>"] Deposit status
        /// </summary>
        [JsonPropertyName("status")]
        public DepositStatus DepositStatus { get; set; }
    }


}

using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Transfer page
    /// </summary>
    [SerializationModel]
    public record DeepCoinTransferPage
    {
        /// <summary>
        /// ["<c>data</c>"] Data
        /// </summary>
        [JsonPropertyName("data")]
        public DeepCoinTransfer[] Data { get; set; } = Array.Empty<DeepCoinTransfer>();
        /// <summary>
        /// ["<c>count</c>"] Total results
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
    /// Transfer info
    /// </summary>
    [SerializationModel]
    public record DeepCoinTransfer
    {
        /// <summary>
        /// ["<c>orderId</c>"] Order id
        /// </summary>
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>receiverAccountType</c>"] Receiver account type
        /// </summary>
        [JsonPropertyName("receiverAccountType")]
        public AccountType ReceiverAccountType { get; set; }
        /// <summary>
        /// ["<c>receiverAccount</c>"] Receiver account
        /// </summary>
        [JsonPropertyName("receiverAccount")]
        public string? ReceiverAccount { get; set; }
        /// <summary>
        /// ["<c>coin</c>"] Asset
        /// </summary>
        [JsonPropertyName("coin")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>amount</c>"] Quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>transferWay</c>"] Transfer way
        /// </summary>
        [JsonPropertyName("transferWay")]
        public string TransferWay { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>status</c>"] Status
        /// </summary>
        [JsonPropertyName("status")]
        public TransferStatus Status { get; set; }
        /// <summary>
        /// ["<c>receiverUID</c>"] Receiver id
        /// </summary>
        [JsonPropertyName("receiverUID")]
        public decimal ReceiverID { get; set; }
        /// <summary>
        /// ["<c>createTime</c>"] Create time
        /// </summary>
        [JsonPropertyName("createTime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>updateTime</c>"] Update time
        /// </summary>
        [JsonPropertyName("updateTime")]
        public DateTime? UpdateTime { get; set; }
    }


}

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
        /// Data
        /// </summary>
        [JsonPropertyName("data")]
        public DeepCoinTransfer[] Data { get; set; } = Array.Empty<DeepCoinTransfer>();
        /// <summary>
        /// Total results
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
    /// Transfer info
    /// </summary>
    [SerializationModel]
    public record DeepCoinTransfer
    {
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Receiver account type
        /// </summary>
        [JsonPropertyName("receiverAccountType")]
        public AccountType ReceiverAccountType { get; set; }
        /// <summary>
        /// Receiver account
        /// </summary>
        [JsonPropertyName("receiverAccount")]
        public string? ReceiverAccount { get; set; }
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("coin")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Transfer way
        /// </summary>
        [JsonPropertyName("transferWay")]
        public string TransferWay { get; set; } = string.Empty;
        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("status")]
        public TransferStatus Status { get; set; }
        /// <summary>
        /// Receiver id
        /// </summary>
        [JsonPropertyName("receiverUID")]
        public decimal ReceiverID { get; set; }
        /// <summary>
        /// Create time
        /// </summary>
        [JsonPropertyName("createTime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Update time
        /// </summary>
        [JsonPropertyName("updateTime")]
        public DateTime? UpdateTime { get; set; }
    }


}

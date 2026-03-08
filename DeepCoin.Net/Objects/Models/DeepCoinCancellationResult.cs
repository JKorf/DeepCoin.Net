using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Cancellation result
    /// </summary>
    [SerializationModel]
    public record DeepCoinCancellationResult
    {
        /// <summary>
        /// ["<c>errorList</c>"] Orders failed to cancel
        /// </summary>
        [JsonPropertyName("errorList")]
        public DeepCoinCancellationResultEntry[] ErrorList { get; set; } = Array.Empty<DeepCoinCancellationResultEntry>();
    }

    /// <summary>
    /// Cancellation
    /// </summary>
    [SerializationModel]
    public record DeepCoinCancellationResultEntry
    {
        /// <summary>
        /// ["<c>memberId</c>"] User id
        /// </summary>
        [JsonPropertyName("memberId")]
        public string UserId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>accountId</c>"] Account id
        /// </summary>
        [JsonPropertyName("accountId")]
        public string AccountId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>orderSysId</c>"] Order id
        /// </summary>
        [JsonPropertyName("orderSysId")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>errorCode</c>"] Error code
        /// </summary>
        [JsonPropertyName("errorCode")]
        public int ErrorCode { get; set; }
        /// <summary>
        /// ["<c>errorMsg</c>"] Error message
        /// </summary>
        [JsonPropertyName("errorMsg")]
        public string ErrorMessage { get; set; } = string.Empty;

    }
}

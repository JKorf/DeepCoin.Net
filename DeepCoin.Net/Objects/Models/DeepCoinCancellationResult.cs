using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text;
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
        /// Orders failed to cancel
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
        /// User id
        /// </summary>
        [JsonPropertyName("memberId")]
        public string UserId { get; set; } = string.Empty;
        /// <summary>
        /// Account id
        /// </summary>
        [JsonPropertyName("accountId")]
        public string AccountId { get; set; } = string.Empty;
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("orderSysId")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Error code
        /// </summary>
        [JsonPropertyName("errorCode")]
        public int ErrorCode { get; set; }
        /// <summary>
        /// Error message
        /// </summary>
        [JsonPropertyName("errorMsg")]
        public string ErrorMessage { get; set; } = string.Empty;

    }
}

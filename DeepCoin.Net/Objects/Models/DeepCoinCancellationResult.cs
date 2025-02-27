﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Cancellation result
    /// </summary>
    public record DeepCoinCancellationResult
    {
        /// <summary>
        /// Orders failed to cancel
        /// </summary>
        [JsonPropertyName("errorList")]
        public IEnumerable<DeepCoinCancellationResultEntry> ErrorList { get; set; } = Array.Empty<DeepCoinCancellationResultEntry>();
    }

    /// <summary>
    /// Cancellation
    /// </summary>
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

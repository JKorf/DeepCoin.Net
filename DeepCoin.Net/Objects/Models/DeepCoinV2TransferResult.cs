using System;
using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// V2 unified transfer acknowledgement.
    /// </summary>
    [SerializationModel]
    public record DeepCoinV2TransferResult
    {
        /// <summary>
        /// ["<c>transferId</c>"] Transfer identifier; empty for observed internal transfers.
        /// </summary>
        [JsonPropertyName("transferId")]
        public string TransferId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>clientId</c>"] Client idempotency identifier.
        /// </summary>
        [JsonPropertyName("clientId")]
        public string ClientId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>status</c>"] Native transfer status; success confirms completion.
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>cTime</c>"] Transfer creation time; unavailable for observed internal transfers.
        /// </summary>
        [JsonPropertyName("cTime")]
        public DateTime? CreateTime { get; set; }
    }
}

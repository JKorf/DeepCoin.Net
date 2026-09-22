using System;
using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net.Enums;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// V2 withdrawal history page.
    /// </summary>
    [SerializationModel]
    public record DeepCoinV2WithdrawPage
    {
        /// <summary>
        /// ["<c>data</c>"] Withdrawal records.
        /// </summary>
        [JsonPropertyName("data")]
        public DeepCoinV2Withdrawal[] Data { get; set; } = Array.Empty<DeepCoinV2Withdrawal>();
        /// <summary>
        /// ["<c>count</c>"] Total record count.
        /// </summary>
        [JsonPropertyName("count")]
        public int Total { get; set; }
        /// <summary>
        /// ["<c>page</c>"] Current page.
        /// </summary>
        [JsonPropertyName("page")]
        public int Page { get; set; }
        /// <summary>
        /// ["<c>size</c>"] Page size.
        /// </summary>
        [JsonPropertyName("size")]
        public int PageSize { get; set; }
    }
}


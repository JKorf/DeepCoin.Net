using System;
using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net.Enums;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// V2 deposit history page.
    /// </summary>
    [SerializationModel]
    public record DeepCoinV2DepositPage
    {
        /// <summary>
        /// ["<c>data</c>"] Deposit records.
        /// </summary>
        [JsonPropertyName("data")]
        public DeepCoinV2Deposit[] Data { get; set; } = Array.Empty<DeepCoinV2Deposit>();
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
        /// ["<c>limit</c>"] Page size.
        /// </summary>
        [JsonPropertyName("limit")]
        public int PageSize { get; set; }
    }
}


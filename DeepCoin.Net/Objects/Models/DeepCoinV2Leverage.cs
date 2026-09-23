using System;
using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net.Enums;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// V2 leverage configuration result.
    /// </summary>
    [SerializationModel]
    public record DeepCoinV2Leverage
    {
        /// <summary>
        /// ["<c>instId</c>"] Product identifier.
        /// </summary>
        [JsonPropertyName("instId")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>lever</c>"] Leverage.
        /// </summary>
        [JsonPropertyName("lever")]
        public decimal Leverage { get; set; }
        /// <summary>
        /// ["<c>mgnMode</c>"] Margin mode.
        /// </summary>
        [JsonPropertyName("mgnMode")]
        public TradeMode TradeMode { get; set; }
        /// <summary>
        /// ["<c>mrgPosition</c>"] Position aggregation mode.
        /// </summary>
        [JsonPropertyName("mrgPosition")]
        public PositionType PositionType { get; set; }
        /// <summary>
        /// ["<c>code</c>"] Execution result code; zero means success.
        /// </summary>
        [JsonPropertyName("code")]
        public int? ResultCode { get; set; }
        /// <summary>
        /// ["<c>msg</c>"] Execution result message.
        /// </summary>
        [JsonPropertyName("msg")]
        public string? ResultMessage { get; set; }
    }
}


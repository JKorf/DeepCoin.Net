using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Ticker price info
    /// </summary>
    [SerializationModel]
    public record DeepCoinTicker
    {
        /// <summary>
        /// ["<c>instType</c>"] Symbol type
        /// </summary>
        [JsonPropertyName("instType")]
        public SymbolType SymbolType { get; set; }
        /// <summary>
        /// ["<c>instId</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("instId")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>last</c>"] Last trade price
        /// </summary>
        [JsonPropertyName("last")]
        public decimal? LastPrice { get; set; }
        /// <summary>
        /// ["<c>askPx</c>"] Best ask price in the order book
        /// </summary>
        [JsonPropertyName("askPx")]
        public decimal? BestAskPrice { get; set; }
        /// <summary>
        /// ["<c>askSz</c>"] Best ask quantity in the order book
        /// </summary>
        [JsonPropertyName("askSz")]
        public decimal? BestAskQuantity { get; set; }
        /// <summary>
        /// ["<c>bidPx</c>"] Best bid price in the order book
        /// </summary>
        [JsonPropertyName("bidPx")]
        public decimal? BestBidPrice { get; set; }
        /// <summary>
        /// ["<c>bidSz</c>"] Best bid quantity in the order book
        /// </summary>
        [JsonPropertyName("bidSz")]
        public decimal? BestBidQuantity { get; set; }
        /// <summary>
        /// ["<c>open24h</c>"] Price 24h ago
        /// </summary>
        [JsonPropertyName("open24h")]
        public decimal? OpenPrice { get; set; }
        /// <summary>
        /// ["<c>high24h</c>"] Highest price in the last 24h
        /// </summary>
        [JsonPropertyName("high24h")]
        public decimal? HighPrice { get; set; }
        /// <summary>
        /// ["<c>low24h</c>"] Lowest price in the last 24h
        /// </summary>
        [JsonPropertyName("low24h")]
        public decimal? LowPrice { get; set; }
        /// <summary>
        /// ["<c>vol24h</c>"] Volume in base asset
        /// </summary>
        [JsonPropertyName("vol24h")]
        public decimal Volume { get; set; }
        /// <summary>
        /// ["<c>volCcy24h</c>"] Volume in quote asset
        /// </summary>
        [JsonPropertyName("volCcy24h")]
        public decimal QuoteVolume { get; set; }
        /// <summary>
        /// ["<c>ts</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("ts")]
        public DateTime Timestamp { get; set; }
    }
}

using DeepCoin.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Ticker price info
    /// </summary>
    public record DeepCoinTicker
    {
        /// <summary>
        /// Symbol type
        /// </summary>
        [JsonPropertyName("instType")]
        public SymbolType SymbolType { get; set; }
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("instId")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Last trade price
        /// </summary>
        [JsonPropertyName("last")]
        public decimal? LastPrice { get; set; }
        /// <summary>
        /// Best ask price in the order book
        /// </summary>
        [JsonPropertyName("askPx")]
        public decimal? BestAskPrice { get; set; }
        /// <summary>
        /// Best ask quantity in the order book
        /// </summary>
        [JsonPropertyName("askSz")]
        public decimal? BestAskQuantity { get; set; }
        /// <summary>
        /// Best bid price in the order book
        /// </summary>
        [JsonPropertyName("bidPx")]
        public decimal? BestBidPrice { get; set; }
        /// <summary>
        /// Best bid quantity in the order book
        /// </summary>
        [JsonPropertyName("bidSz")]
        public decimal? BestBidQuantity { get; set; }
        /// <summary>
        /// Price 24h ago
        /// </summary>
        [JsonPropertyName("open24h")]
        public decimal? OpenPrice { get; set; }
        /// <summary>
        /// Highest price in the last 24h
        /// </summary>
        [JsonPropertyName("high24h")]
        public decimal? HighPrice { get; set; }
        /// <summary>
        /// Lowest price in the last 24h
        /// </summary>
        [JsonPropertyName("low24h")]
        public decimal? LowPrice { get; set; }
        /// <summary>
        /// Volume in base asset
        /// </summary>
        [JsonPropertyName("volCcy24h")]
        public decimal Volume { get; set; }
        /// <summary>
        /// Volume in quote asset
        /// </summary>
        [JsonPropertyName("vol24h")]
        public decimal QuoteVolume { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("ts")]
        public DateTime Timestamp { get; set; }
    }
}

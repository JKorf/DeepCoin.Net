using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Kline update
    /// </summary>
    [SerializationModel]
    public record DeepCoinKlineUpdate
    {
        /// <summary>
        /// ["<c>ExchangeID</c>"] Exchange id
        /// </summary>
        [JsonPropertyName("ExchangeID")]
        public string ExchangeId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>InstrumentID</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("InstrumentID")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>PeriodID</c>"] Interval
        /// </summary>
        [JsonPropertyName("PeriodID")]
        public KlineInterval Interval { get; set; }
        /// <summary>
        /// ["<c>BeginTime</c>"] Open timestamp
        /// </summary>
        [JsonPropertyName("BeginTime")]
        public DateTime OpenTime { get; set; }
        /// <summary>
        /// ["<c>OpenPrice</c>"] Open price
        /// </summary>
        [JsonPropertyName("OpenPrice")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// ["<c>ClosePrice</c>"] Close price
        /// </summary>
        [JsonPropertyName("ClosePrice")]
        public decimal ClosePrice { get; set; }
        /// <summary>
        /// ["<c>HighestPrice</c>"] High price
        /// </summary>
        [JsonPropertyName("HighestPrice")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// ["<c>LowestPrice</c>"] Low price
        /// </summary>
        [JsonPropertyName("LowestPrice")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// ["<c>Volume</c>"] Volume in base asset
        /// </summary>
        [JsonPropertyName("Volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// ["<c>Turnover</c>"] Turnover
        /// </summary>
        [JsonPropertyName("Turnover")]
        public decimal Turnover { get; set; }
        /// <summary>
        /// ["<c>TimeZone</c>"] TimeZone
        /// </summary>
        [JsonPropertyName("TimeZone")]
        public int TimeZone { get; set; }
        /// <summary>
        /// ["<c>UpdateTime</c>"] Update timestamp
        /// </summary>
        [JsonPropertyName("UpdateTime")]
        public DateTime UpdateTime { get; set; }
    }
}

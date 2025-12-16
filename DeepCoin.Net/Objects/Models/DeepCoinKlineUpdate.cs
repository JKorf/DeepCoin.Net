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
        /// Exchange id
        /// </summary>
        [JsonPropertyName("ExchangeID")]
        public string ExchangeId { get; set; } = string.Empty;
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("InstrumentID")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Interval
        /// </summary>
        [JsonPropertyName("PeriodID")]
        public KlineInterval Interval { get; set; }
        /// <summary>
        /// Open timestamp
        /// </summary>
        [JsonPropertyName("BeginTime")]
        public DateTime OpenTime { get; set; }
        /// <summary>
        /// Open price
        /// </summary>
        [JsonPropertyName("OpenPrice")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// Close price
        /// </summary>
        [JsonPropertyName("ClosePrice")]
        public decimal ClosePrice { get; set; }
        /// <summary>
        /// High price
        /// </summary>
        [JsonPropertyName("HighestPrice")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// Low price
        /// </summary>
        [JsonPropertyName("LowestPrice")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// Volume in base asset
        /// </summary>
        [JsonPropertyName("Volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// Turnover
        /// </summary>
        [JsonPropertyName("Turnover")]
        public decimal Turnover { get; set; }
        /// <summary>
        /// TimeZone
        /// </summary>
        [JsonPropertyName("TimeZone")]
        public int TimeZone { get; set; }
        /// <summary>
        /// Update timestamp
        /// </summary>
        [JsonPropertyName("UpdateTime")]
        public DateTime UpdateTime { get; set; }
    }
}

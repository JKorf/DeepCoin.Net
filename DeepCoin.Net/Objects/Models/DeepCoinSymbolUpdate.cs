using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Symbol update
    /// </summary>
    [SerializationModel]
    public record DeepCoinSymbolUpdate
    {
        /// <summary>
        /// Exchange Id
        /// </summary>
        [JsonPropertyName("ExchangeID")]
        public string ExchangeId { get; set; } = string.Empty;
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("InstrumentID")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Product group
        /// </summary>
        [JsonPropertyName("ProductGroup")]
        public ProductGroup ProductGroup { get; set; }
        /// <summary>
        /// Update time
        /// </summary>
        [JsonPropertyName("UpdateTime")]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// Update millisecond
        /// </summary>
        [JsonPropertyName("UpdateMilliSecond")]
        public int UpdateMilliSecond { get; set; }
        /// <summary>
        /// Upper limit price
        /// </summary>
        [JsonPropertyName("UpperLimitPrice")]
        public decimal UpperLimitPrice { get; set; }
        /// <summary>
        /// Lower limit price
        /// </summary>
        [JsonPropertyName("LowerLimitPrice")]
        public decimal LowerLimitPrice { get; set; }
        /// <summary>
        /// Underlying price
        /// </summary>
        [JsonPropertyName("UnderlyingPrice")]
        public decimal UnderlyingPrice { get; set; }
        /// <summary>
        /// Marked price
        /// </summary>
        [JsonPropertyName("MarkedPrice")]
        public decimal? MarkedPrice { get; set; }
        /// <summary>
        /// Position fee rate
        /// </summary>
        [JsonPropertyName("PositionFeeRate")]
        public decimal PositionFeeRate { get; set; }
        /// <summary>
        /// High price
        /// </summary>
        [JsonPropertyName("HighestPrice")]
        public decimal? HighPrice { get; set; }
        /// <summary>
        /// Low price
        /// </summary>
        [JsonPropertyName("LowestPrice")]
        public decimal? LowPrice { get; set; }
        /// <summary>
        /// Last price
        /// </summary>
        [JsonPropertyName("LastPrice")]
        public decimal? LastPrice { get; set; }
        /// <summary>
        /// Volume
        /// </summary>
        [JsonPropertyName("Volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// Turnover
        /// </summary>
        [JsonPropertyName("Turnover")]
        public decimal Turnover { get; set; }
        /// <summary>
        /// Open interest
        /// </summary>
        [JsonPropertyName("OpenInterest")]
        public decimal OpenInterest { get; set; }
        /// <summary>
        /// Open price
        /// </summary>
        [JsonPropertyName("OpenPrice")]
        public decimal? OpenPrice { get; set; }
        /// <summary>
        /// Symbol status
        /// </summary>
        [JsonPropertyName("InstrumentStatus")]
        public SymbolStatus Status { get; set; }
        /// <summary>
        /// Pre position fee rate
        /// </summary>
        [JsonPropertyName("PrePositionFeeRate")]
        public decimal PrePositionFeeRate { get; set; }
    }


}

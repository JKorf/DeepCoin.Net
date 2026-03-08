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
        /// ["<c>ExchangeID</c>"] Exchange Id
        /// </summary>
        [JsonPropertyName("ExchangeID")]
        public string ExchangeId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>InstrumentID</c>"] Symbol
        /// </summary>
        [JsonPropertyName("InstrumentID")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>ProductGroup</c>"] Product group
        /// </summary>
        [JsonPropertyName("ProductGroup")]
        public ProductGroup ProductGroup { get; set; }
        /// <summary>
        /// ["<c>UpdateTime</c>"] Update time
        /// </summary>
        [JsonPropertyName("UpdateTime")]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// ["<c>UpdateMilliSecond</c>"] Update millisecond
        /// </summary>
        [JsonPropertyName("UpdateMilliSecond")]
        public int UpdateMilliSecond { get; set; }
        /// <summary>
        /// ["<c>UpperLimitPrice</c>"] Upper limit price
        /// </summary>
        [JsonPropertyName("UpperLimitPrice")]
        public decimal UpperLimitPrice { get; set; }
        /// <summary>
        /// ["<c>LowerLimitPrice</c>"] Lower limit price
        /// </summary>
        [JsonPropertyName("LowerLimitPrice")]
        public decimal LowerLimitPrice { get; set; }
        /// <summary>
        /// ["<c>UnderlyingPrice</c>"] Underlying price
        /// </summary>
        [JsonPropertyName("UnderlyingPrice")]
        public decimal UnderlyingPrice { get; set; }
        /// <summary>
        /// ["<c>MarkedPrice</c>"] Marked price
        /// </summary>
        [JsonPropertyName("MarkedPrice")]
        public decimal? MarkedPrice { get; set; }
        /// <summary>
        /// ["<c>PositionFeeRate</c>"] Position fee rate
        /// </summary>
        [JsonPropertyName("PositionFeeRate")]
        public decimal PositionFeeRate { get; set; }
        /// <summary>
        /// ["<c>HighestPrice</c>"] High price
        /// </summary>
        [JsonPropertyName("HighestPrice")]
        public decimal? HighPrice { get; set; }
        /// <summary>
        /// ["<c>LowestPrice</c>"] Low price
        /// </summary>
        [JsonPropertyName("LowestPrice")]
        public decimal? LowPrice { get; set; }
        /// <summary>
        /// ["<c>LastPrice</c>"] Last price
        /// </summary>
        [JsonPropertyName("LastPrice")]
        public decimal? LastPrice { get; set; }
        /// <summary>
        /// ["<c>Volume</c>"] Volume
        /// </summary>
        [JsonPropertyName("Volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// ["<c>Turnover</c>"] Turnover
        /// </summary>
        [JsonPropertyName("Turnover")]
        public decimal Turnover { get; set; }
        /// <summary>
        /// ["<c>OpenInterest</c>"] Open interest
        /// </summary>
        [JsonPropertyName("OpenInterest")]
        public decimal OpenInterest { get; set; }
        /// <summary>
        /// ["<c>OpenPrice</c>"] Open price
        /// </summary>
        [JsonPropertyName("OpenPrice")]
        public decimal? OpenPrice { get; set; }
        /// <summary>
        /// ["<c>InstrumentStatus</c>"] Symbol status
        /// </summary>
        [JsonPropertyName("InstrumentStatus")]
        public SymbolStatus Status { get; set; }
        /// <summary>
        /// ["<c>PrePositionFeeRate</c>"] Pre position fee rate
        /// </summary>
        [JsonPropertyName("PrePositionFeeRate")]
        public decimal PrePositionFeeRate { get; set; }
    }


}

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

        /// <summary>
        /// ["<c>PositionFeeTime</c>"] Funding settlement time, Unix seconds on the wire; zero when unavailable.
        /// </summary>
        [JsonPropertyName("PositionFeeTime")]
        public DateTime? PositionFeeTime { get; set; }

        /// <summary>
        /// ["<c>BidPrice1</c>"] Best bid price.
        /// </summary>
        [JsonPropertyName("BidPrice1")]
        public decimal? BestBidPrice { get; set; }

        /// <summary>
        /// ["<c>AskPrice1</c>"] Best ask price.
        /// </summary>
        [JsonPropertyName("AskPrice1")]
        public decimal? BestAskPrice { get; set; }

        /// <summary>
        /// ["<c>Volume24</c>"] Rolling 24-hour quantity, in contracts for futures and base currency for spot.
        /// </summary>
        [JsonPropertyName("Volume24")]
        public decimal? Volume24Hrs { get; set; }

        /// <summary>
        /// ["<c>Turnover24</c>"] Rolling 24-hour turnover, in quote currency for linear contracts and base currency for inverse contracts.
        /// </summary>
        [JsonPropertyName("Turnover24")]
        public decimal? Turnover24Hrs { get; set; }

        /// <summary>
        /// ["<c>V2</c>"] Raw additional V2 ticker volume. The reporting window is unspecified; this is not the REST 24-hour volume.
        /// </summary>
        [JsonPropertyName("V2")]
        public decimal? RawV2Volume { get; set; }

        /// <summary>
        /// ["<c>T2</c>"] Raw additional V2 ticker turnover. The reporting window is unspecified; this is not the REST 24-hour turnover.
        /// </summary>
        [JsonPropertyName("T2")]
        public decimal? RawV2Turnover { get; set; }

    }


}

using System.Text.Json.Serialization;
using System;

namespace DeepCoin.Net.Objects.Models;

/// <summary>
/// V2 ticker payload.
/// </summary>
public sealed class DeepCoinV2TickerData
{
    /// <summary>
    /// [<c>I</c>] Instrument identifier.
    /// </summary>
    [JsonPropertyName("I")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// [<c>U</c>] Latest update time, Unix milliseconds.
    /// </summary>
    [JsonPropertyName("U")]
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// [<c>PF</c>] Funding settlement time, Unix seconds; zero when unavailable.
    /// </summary>
    [JsonPropertyName("PF")]
    public DateTime? PositionFeeTime { get; set; }

    /// <summary>
    /// [<c>E</c>] Previous settlement's funding rate.
    /// </summary>
    [JsonPropertyName("E")]
    public decimal? PrePositionFeeRate { get; set; }

    /// <summary>
    /// [<c>C</c>] Upper limit price.
    /// </summary>
    [JsonPropertyName("C")]
    public decimal? UpperLimitPrice { get; set; }

    /// <summary>
    /// [<c>F</c>] Lower limit price.
    /// </summary>
    [JsonPropertyName("F")]
    public decimal? LowerLimitPrice { get; set; }

    /// <summary>
    /// [<c>D</c>] Underlying price.
    /// </summary>
    [JsonPropertyName("D")]
    public decimal? UnderlyingPrice { get; set; }

    /// <summary>
    /// [<c>M</c>] Mark price.
    /// </summary>
    [JsonPropertyName("M")]
    public decimal? MarkedPrice { get; set; }

    /// <summary>
    /// [<c>H</c>] Highest price.
    /// </summary>
    [JsonPropertyName("H")]
    public decimal? HighPrice { get; set; }

    /// <summary>
    /// [<c>L</c>] Lowest price.
    /// </summary>
    [JsonPropertyName("L")]
    public decimal? LowPrice { get; set; }

    /// <summary>
    /// [<c>N</c>] Latest price.
    /// </summary>
    [JsonPropertyName("N")]
    public decimal? LastPrice { get; set; }

    /// <summary>
    /// [<c>V</c>] Volume; live REST comparisons identify this as the rolling 24-hour quantity.
    /// </summary>
    [JsonPropertyName("V")]
    public decimal? Volume { get; set; }

    /// <summary>
    /// [<c>T</c>] Turnover; live REST comparisons identify this as the rolling 24-hour turnover.
    /// </summary>
    [JsonPropertyName("T")]
    public decimal? Turnover { get; set; }

    /// <summary>
    /// [<c>O</c>] Opening price.
    /// </summary>
    [JsonPropertyName("O")]
    public decimal? OpenPrice { get; set; }

    /// <summary>
    /// [<c>V2</c>] Additional volume with an unspecified reporting window.
    /// </summary>
    [JsonPropertyName("V2")]
    public decimal? RawV2Volume { get; set; }

    /// <summary>
    /// [<c>T2</c>] Additional turnover with an unspecified reporting window.
    /// </summary>
    [JsonPropertyName("T2")]
    public decimal? RawV2Turnover { get; set; }

    /// <summary>
    /// [<c>BP1</c>] Best bid price.
    /// </summary>
    [JsonPropertyName("BP1")]
    public decimal? BestBidPrice { get; set; }

    /// <summary>
    /// [<c>AP1</c>] Best ask price.
    /// </summary>
    [JsonPropertyName("AP1")]
    public decimal? BestAskPrice { get; set; }
}

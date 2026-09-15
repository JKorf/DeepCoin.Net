using System.Text.Json;
using System.Text.Json.Serialization;
using DeepCoin.Net.Enums;

namespace DeepCoin.Net.Objects.Internal;

/// <summary>
/// V2 public stream envelope; the private stream retains the original protocol.
/// </summary>
internal sealed class DeepCoinV2SocketMessage
{
    /// <summary>
    /// [<c>I</c>] Symbol alias observed on live V2 candle frames.
    /// </summary>
    [JsonInclude, JsonPropertyName("I")]
    internal string CandleSymbolAlias { get => Symbol; set => Symbol = value; }

    /// <summary>
    /// [<c>P</c>] Period alias observed on live V2 candle frames.
    /// </summary>
    [JsonInclude, JsonPropertyName("P")]
    internal KlineInterval? CandlePeriodAlias { get => Period; set => Period = value; }

    /// <summary>
    /// [<c>a</c>] Message action identifying the topic or subscription acknowledgment.
    /// </summary>
    [JsonPropertyName("a")]
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// [<c>m</c>] Subscription acknowledgment result text.
    /// </summary>
    [JsonPropertyName("m")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// [<c>i</c>] Native stream symbol: compact for futures or slash-separated for spot.
    /// </summary>
    [JsonPropertyName("i")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// [<c>p</c>] Candle interval used to route candle updates.
    /// </summary>
    [JsonPropertyName("p")]
    public KlineInterval? Period { get; set; }

    /// <summary>
    /// [<c>t</c>] Book update type: f for a full snapshot or i for an incremental update.
    /// </summary>
    [JsonPropertyName("t")]
    public V2BookUpdateType? UpdateType { get; set; }

    /// <summary>
    /// [<c>tt</c>] Trade timestamp in Unix milliseconds.
    /// </summary>
    [JsonPropertyName("tt")]
    public long TradeTime { get; set; }

    /// <summary>
    /// [<c>mt</c>] Market timestamp in Unix milliseconds.
    /// </summary>
    [JsonPropertyName("mt")]
    public long MarketTime { get; set; }

    /// <summary>
    /// [<c>pt</c>] Publication timestamp in Unix milliseconds.
    /// </summary>
    [JsonPropertyName("pt")]
    public long PublishTime { get; set; }

    /// <summary>
    /// [<c>d</c>] Topic payload: ticker or book object or array, or trade or candle array.
    /// </summary>
    [JsonPropertyName("d")]
    public JsonElement Data { get; set; }
}

using System.Text.Json.Serialization;
using DeepCoin.Net.Enums;

namespace DeepCoin.Net.Objects.Internal;

/// <summary>
/// Native V2 public subscription request.
/// </summary>
internal sealed class DeepCoinV2SocketRequest
{
    /// <summary>
    /// [<c>Action</c>] Action code: 1 to subscribe or 0 to unsubscribe using the original LocalNo.
    /// </summary>
    [JsonPropertyName("Action")]
    public V2SubscriptionAction Action { get; set; } = V2SubscriptionAction.Subscribe;

    /// <summary>
    /// [<c>Symbol</c>] Native stream symbol: compact for futures or slash-separated for spot.
    /// </summary>
    [JsonPropertyName("Symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// [<c>LocalNo</c>] Positive subscription identifier, echoed in acknowledgments and reused when unsubscribing. New subscriptions use a unique value.
    /// </summary>
    [JsonPropertyName("LocalNo")]
    public int RequestId { get; set; }

    /// <summary>
    /// [<c>Topic</c>] Requested public topic.
    /// </summary>
    [JsonPropertyName("Topic")]
    public string Topic { get; set; } = string.Empty;

    /// <summary>
    /// [<c>ResumeNo</c>] Resume position; -1 starts the current stream.
    /// </summary>
    [JsonPropertyName("ResumeNo")]
    public int ResumeNumber { get; set; } = -1;

    /// <summary>
    /// [<c>PeriodID</c>] Candle interval; omitted for other topics.
    /// </summary>
    [JsonPropertyName("PeriodID"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public KlineInterval? Period { get; set; }

    /// <summary>
    /// [<c>Count</c>] Historical candle count, at most 100; the live endpoint rejects zero.
    /// </summary>
    [JsonPropertyName("Count"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Count { get; set; }
}

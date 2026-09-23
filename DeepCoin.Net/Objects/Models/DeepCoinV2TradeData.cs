using System.Text.Json.Serialization;
using System;
using DeepCoin.Net.Enums;

namespace DeepCoin.Net.Objects.Models;

/// <summary>
/// V2 trade payload.
/// </summary>
public sealed class DeepCoinV2TradeData
{
    /// <summary>
    /// [<c>TradeID</c>] Trade identifier.
    /// </summary>
    [JsonPropertyName("TradeID")]
    public string TradeId { get; set; } = string.Empty;

    /// <summary>
    /// [<c>D</c>] Trade direction: 0 for buy, 1 for sell.
    /// </summary>
    [JsonPropertyName("D")]
    public OrderSide Side { get; set; }

    /// <summary>
    /// [<c>P</c>] Trade price.
    /// </summary>
    [JsonPropertyName("P")]
    public decimal Price { get; set; }

    /// <summary>
    /// [<c>V</c>] Trade quantity.
    /// </summary>
    [JsonPropertyName("V")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// [<c>T</c>] Trade time, Unix seconds.
    /// </summary>
    [JsonPropertyName("T")]
    public DateTime Timestamp { get; set; }
}

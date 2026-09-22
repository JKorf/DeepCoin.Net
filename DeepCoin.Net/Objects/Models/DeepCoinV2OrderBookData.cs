using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models;

/// <summary>
/// V2 order book payload.
/// </summary>
public sealed class DeepCoinV2OrderBookData
{
    /// <summary>
    /// [<c>a</c>] Ask price and quantity levels.
    /// </summary>
    [JsonPropertyName("a")]
    public DeepCoinOrderBookEntry[] Asks { get; set; } = [];

    /// <summary>
    /// [<c>b</c>] Bid price and quantity levels.
    /// </summary>
    [JsonPropertyName("b")]
    public DeepCoinOrderBookEntry[] Bids { get; set; } = [];
}

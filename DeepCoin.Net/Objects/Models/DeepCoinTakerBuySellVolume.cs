using System;
using System.Text.Json.Serialization;
using DeepCoin.Net.Enums;

namespace DeepCoin.Net.Objects.Models;

/// <summary>
/// Taker buy/sell volume
/// </summary>
public record DeepCoinTakerBuySellVolume
{
    /// <summary>
    /// ["<c>ts</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("ts")]
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// ["<c>buyVol</c>"] Buy volume
    /// </summary>
    [JsonPropertyName("buyVol")]
    public decimal BuyVolume { get; set; }
    /// <summary>
    /// ["<c>sellVol</c>"] Sell volume
    /// </summary>
    [JsonPropertyName("sellVol")]
    public decimal SellVolume { get; set; }
}


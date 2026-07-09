using System;
using System.Text.Json.Serialization;
using DeepCoin.Net.Enums;

namespace DeepCoin.Net.Objects.Models;

/// <summary>
/// Open interest
/// </summary>
public record DeepCoinOpenInterest
{
    /// <summary>
    /// ["<c>ts</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("ts")]
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// ["<c>oi</c>"] Open interest
    /// </summary>
    [JsonPropertyName("oi")]
    public decimal OpenInterest { get; set; }
    /// <summary>
    /// ["<c>vol</c>"] Volume
    /// </summary>
    [JsonPropertyName("vol")]
    public decimal Volume { get; set; }
}


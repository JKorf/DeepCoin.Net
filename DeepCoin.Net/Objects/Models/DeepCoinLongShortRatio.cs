using System;
using System.Text.Json.Serialization;
using DeepCoin.Net.Enums;

namespace DeepCoin.Net.Objects.Models;

/// <summary>
/// Long short ratio
/// </summary>
public record DeepCoinLongShortRatio
{
    /// <summary>
    /// ["<c>ts</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("ts")]
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// ["<c>longRatio</c>"] Long ratio
    /// </summary>
    [JsonPropertyName("longRatio")]
    public decimal LongRatio { get; set; }
    /// <summary>
    /// ["<c>shortRatio</c>"] Short ratio
    /// </summary>
    [JsonPropertyName("shortRatio")]
    public decimal ShortRatio { get; set; }
    /// <summary>
    /// ["<c>longShortRatio</c>"] Long short ratio
    /// </summary>
    [JsonPropertyName("longShortRatio")]
    public decimal LongShortRatio { get; set; }
}


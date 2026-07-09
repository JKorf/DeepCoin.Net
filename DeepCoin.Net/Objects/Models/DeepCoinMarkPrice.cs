using System;
using System.Text.Json.Serialization;
using DeepCoin.Net.Enums;

namespace DeepCoin.Net.Objects.Models;

/// <summary>
/// Mark price info
/// </summary>
public record DeepCoinMarkPrice
{
    /// <summary>
    /// ["<c>instType</c>"] Symbol type
    /// </summary>
    [JsonPropertyName("instType")]
    public SymbolType? SymbolType { get; set; }
    /// <summary>
    /// ["<c>instId</c>"] Symbol name
    /// </summary>
    [JsonPropertyName("instId")]
    public string Symbol { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>markPx</c>"] Mark price
    /// </summary>
    [JsonPropertyName("markPx")]
    public decimal MarkPrice { get; set; }
    /// <summary>
    /// ["<c>ts</c>"] Timestamp
    /// </summary>
    [JsonPropertyName("ts")]
    public DateTime Timestamp { get; set; }
}


using System;
using System.Text.Json.Serialization;
using DeepCoin.Net.Enums;

namespace DeepCoin.Net.Objects.Models;

/// <summary>
/// Fee rate
/// </summary>
public record DeepCoinFeeRate
{
    /// <summary>
    /// ["<c>level</c>"] Level
    /// </summary>
    [JsonPropertyName("level")]
    public string Level { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>feeGroup</c>"] Fee group
    /// </summary>
    [JsonPropertyName("feeGroup")]
    public string FeeGroup { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>category</c>"] Category
    /// </summary>
    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>instType</c>"] Symbol type
    /// </summary>
    [JsonPropertyName("instType")]
    public SymbolType? SymbolType { get; set; }
    /// <summary>
    /// ["<c>maker</c>"] Maker fee
    /// </summary>
    [JsonPropertyName("maker")]
    public decimal Maker { get; set; }
    /// <summary>
    /// ["<c>taker</c>"] Taker fee
    /// </summary>
    [JsonPropertyName("taker")]
    public decimal Taker { get; set; }
    /// <summary>
    /// ["<c>makerU</c>"] USDT-margined contract maker fee rate
    /// </summary>
    [JsonPropertyName("makerU")]
    public decimal MakerU { get; set; }
    /// <summary>
    /// ["<c>takerU</c>"] USDT-margined contract taker fee rate
    /// </summary>
    [JsonPropertyName("takerU")]
    public decimal TakerU { get; set; }
    /// <summary>
    /// ["<c>makerUSDC</c>"] USDC contract maker fee rate. Currently unsupported
    /// </summary>
    [JsonPropertyName("makerUSDC")]
    public decimal? MakerUSDC { get; set; }
    /// <summary>
    /// ["<c>takerUSDC</c>"] 	USDC contract taker fee rate. Currently unsupported
    /// </summary>
    [JsonPropertyName("takerUSDC")]
    public decimal? TakerUSDC { get; set; }
    /// <summary>
    /// ["<c>delivery</c>"] Delivery fee rate. Currently unsupported
    /// </summary>
    [JsonPropertyName("delivery")]
    public decimal? Delivery { get; set; }
    /// <summary>
    /// ["<c>exercise</c>"] Exercise fee rate. Currently unsupported
    /// </summary>
    [JsonPropertyName("exercise")]
    public decimal? Exercise { get; set; }
    /// <summary>
    /// ["<c>ts</c>"] Data timestamp
    /// </summary>
    [JsonPropertyName("ts")]
    public DateTime Timestamp { get; set; }
}


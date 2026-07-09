using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using DeepCoin.Net.Enums;

namespace DeepCoin.Net.Objects.Models;

/// <summary>
/// All balances info
/// </summary>
public record DeepCoinAllBalances
{
    /// <summary>
    /// ["<c>uTime</c>"] Update time
    /// </summary>
    [JsonPropertyName("uTime")]
    public DateTime UpdateTime { get; set; }
    /// <summary>
    /// ["<c>summary</c>"] Summary
    /// </summary>
    [JsonPropertyName("summary")]
    public DeepCoinAllBalancesSummary Summary { get; set; } = null!;
    /// <summary>
    /// ["<c>accounts</c>"] Accounts
    /// </summary>
    [JsonPropertyName("accounts")]
    public DeepCoinAllBalancesAccount[] Accounts { get; set; } = [];
}

/// <summary>
/// Balances summary
/// </summary>
public record DeepCoinAllBalancesSummary
{
    /// <summary>
    /// ["<c>totalEqUsd</c>"] Total equity value in USD
    /// </summary>
    [JsonPropertyName("totalEqUsd")]
    public decimal TotalEquityUsd { get; set; }
    /// <summary>
    /// ["<c>totalAvailBalByCcy</c>"] Total available balance by asset
    /// </summary>
    [JsonPropertyName("totalAvailBalByCcy")]
    public Dictionary<string, decimal> TotalAvailBalanceByAsset { get; set; } = null!;
}

/// <summary>
/// Account balances
/// </summary>
public record DeepCoinAllBalancesAccount
{
    /// <summary>
    /// ["<c>accountType</c>"] Account type
    /// </summary>
    [JsonPropertyName("accountType")]
    public BalanceType Accounttype { get; set; }
    /// <summary>
    /// ["<c>accountName</c>"] Account name
    /// </summary>
    [JsonPropertyName("accountName")]
    public string AccountName { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>enabled</c>"] Enabled
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }
    /// <summary>
    /// ["<c>details</c>"] Details
    /// </summary>
    [JsonPropertyName("details")]
    public DeepCoinAllBalancesAccountDetails[] Details { get; set; } = [];
}

/// <summary>
/// Account balance details
/// </summary>
public record DeepCoinAllBalancesAccountDetails
{
    /// <summary>
    /// ["<c>ccy</c>"] Asset
    /// </summary>
    [JsonPropertyName("ccy")]
    public string Asset { get; set; } = string.Empty;
    /// <summary>
    /// ["<c>bal</c>"] Balance
    /// </summary>
    [JsonPropertyName("bal")]
    public decimal Balance { get; set; }
    /// <summary>
    /// ["<c>availBal</c>"] Available balance
    /// </summary>
    [JsonPropertyName("availBal")]
    public decimal AvailableBalance { get; set; }
    /// <summary>
    /// ["<c>frozenBal</c>"] Frozen balance
    /// </summary>
    [JsonPropertyName("frozenBal")]
    public decimal FrozenBalance { get; set; }
    /// <summary>
    /// ["<c>unrealizedProfit</c>"] Unrealized profit
    /// </summary>
    [JsonPropertyName("unrealizedProfit")]
    public decimal UnrealizedProfit { get; set; }
    /// <summary>
    /// ["<c>equity</c>"] Equity
    /// </summary>
    [JsonPropertyName("equity")]
    public decimal Equity { get; set; }
    /// <summary>
    /// ["<c>eqUsd</c>"] Equity usd
    /// </summary>
    [JsonPropertyName("eqUsd")]
    public decimal EquityUsd { get; set; }
    /// <summary>
    /// ["<c>uTime</c>"] Update time
    /// </summary>
    [JsonPropertyName("uTime")]
    public DateTime UpdateTime { get; set; }
}


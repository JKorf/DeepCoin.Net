using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Enums;

/// <summary>
/// Data interval
/// </summary>
[JsonConverter(typeof(EnumConverter<DataInterval>))]
public enum DataInterval
{
    /// <summary>
    /// ["<c>5m</c>"] Five minutes
    /// </summary>
    [Map("5m")]
    FiveMinutes,
    /// <summary>
    /// ["<c>15m</c>"] Fifteen minutes
    /// </summary>
    [Map("15m")]
    FifteenMinutes,
    /// <summary>
    /// ["<c>30m</c>"] Thirty minutes
    /// </summary>
    [Map("30m")]
    ThirtyMinutes,
    /// <summary>
    /// ["<c>1H</c>"] One hour
    /// </summary>
    [Map("1H")]
    OneHour,
    /// <summary>
    /// ["<c>4H</c>"] Four hours
    /// </summary>
    [Map("4H")]
    FourHours,
    /// <summary>
    /// ["<c>1D</c>"] One day
    /// </summary>
    [Map("1D")]
    OneDay,
}

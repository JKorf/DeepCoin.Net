using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Kline interval
    /// </summary>
    [JsonConverter(typeof(EnumConverter<KlineInterval>))]
    public enum KlineInterval
    {
        /// <summary>
        /// One minute
        /// </summary>
        [Map("1m")]
        OneMinute = 60,
        /// <summary>
        /// Five minutes
        /// </summary>
        [Map("5m")]
        FiveMinutes = 60 * 5,
        /// <summary>
        /// Fifteen minutes
        /// </summary>
        [Map("15m")]
        FifteenMinutes = 60 * 15,
        /// <summary>
        /// Thirty minutes
        /// </summary>
        [Map("30m")]
        ThirtyMinutes = 60 * 30,
        /// <summary>
        /// One hour
        /// </summary>
        [Map("1H")]
        OneHour = 60 * 60,
        /// <summary>
        /// Four hours
        /// </summary>
        [Map("4H")]
        FourHours = 60 * 60 * 4,
        /// <summary>
        /// Twelve hours
        /// </summary>
        [Map("12H")]
        TwelveHours = 60 * 60 * 12,
        /// <summary>
        /// One minute
        /// </summary>
        [Map("1D")]
        OneDay = 60 * 60 * 24,
        /// <summary>
        /// One week
        /// </summary>
        [Map("1W")]
        OneWeek = 60 * 60 * 24 * 7,
        /// <summary>
        /// One month
        /// </summary>
        [Map("1M")]
        OneMonth = 60 * 60 * 24 * 30,
        /// <summary>
        /// One year
        /// </summary>
        [Map("1Y")]
        OneYear = 60 * 60 * 24 * 365
    }
}

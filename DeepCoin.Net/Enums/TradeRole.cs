using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Trade role
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TradeRole>))]
    public enum TradeRole
    {
        /// <summary>
        /// ["<c>T</c>"] Taker
        /// </summary>
        [Map("T", "1")]
        Taker,
        /// <summary>
        /// ["<c>M</c>"] Maker
        /// </summary>
        [Map("M", "0")]
        Maker
    }
}

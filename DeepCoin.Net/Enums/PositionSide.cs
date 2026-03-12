using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Position side
    /// </summary>
    [JsonConverter(typeof(EnumConverter<PositionSide>))]
    public enum PositionSide
    {
        /// <summary>
        /// ["<c>long</c>"] Long
        /// </summary>
        [Map("long", "0")]
        Long,
        /// <summary>
        /// ["<c>short</c>"] Short
        /// </summary>
        [Map("short", "1")]
        Short
    }
}

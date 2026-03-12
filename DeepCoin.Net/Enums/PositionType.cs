using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Position type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<PositionType>))]
    public enum PositionType
    {
        /// <summary>
        /// ["<c>merge</c>"] Merge position
        /// </summary>
        [Map("merge")]
        Merge,
        /// <summary>
        /// ["<c>split</c>"] Split position
        /// </summary>
        [Map("split")]
        Split
    }
}

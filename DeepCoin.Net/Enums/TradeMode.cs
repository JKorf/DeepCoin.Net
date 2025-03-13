using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Trade mode
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TradeMode>))]
    public enum TradeMode
    {
        /// <summary>
        /// Spot
        /// </summary>
        [Map("cash")]
        Spot,
        /// <summary>
        /// Cross margin
        /// </summary>
        [Map("cross")]
        Cross,
        /// <summary>
        /// Isolated margin
        /// </summary>
        [Map("isolated")]
        Isolated
    }
}

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
        /// ["<c>cash</c>"] Spot
        /// </summary>
        [Map("cash")]
        Spot,
        /// <summary>
        /// ["<c>cross</c>"] Cross margin
        /// </summary>
        [Map("cross")]
        Cross,
        /// <summary>
        /// ["<c>isolated</c>"] Isolated margin
        /// </summary>
        [Map("isolated")]
        Isolated
    }
}

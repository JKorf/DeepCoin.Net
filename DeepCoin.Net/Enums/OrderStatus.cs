using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Order status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderStatus>))]
    public enum OrderStatus
    {
        /// <summary>
        /// Open order
        /// </summary>
        [Map("live", "4")]
        Live,
        /// <summary>
        /// Partially filled open order
        /// </summary>
        [Map("partially_filled")]
        PartiallyFilled,
        /// <summary>
        /// Canceled
        /// </summary>
        [Map("canceled", "6")]
        Canceled,
        /// <summary>
        /// Filled
        /// </summary>
        [Map("filled", "1")]
        Filled
    }
}

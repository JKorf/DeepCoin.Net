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
        /// ["<c>live</c>"] Open order
        /// </summary>
        [Map("live", "4")]
        Live,
        /// <summary>
        /// ["<c>partially_filled</c>"] Partially filled open order
        /// </summary>
        [Map("partially_filled")]
        PartiallyFilled,
        /// <summary>
        /// ["<c>canceled</c>"] Canceled
        /// </summary>
        [Map("canceled", "6")]
        Canceled,
        /// <summary>
        /// ["<c>filled</c>"] Filled
        /// </summary>
        [Map("filled", "1")]
        Filled
    }
}

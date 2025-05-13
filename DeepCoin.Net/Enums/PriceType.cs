using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Price type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<PriceType>))]
    public enum PriceType
    {
        /// <summary>
        /// Last price
        /// </summary>
        [Map("last")]
        LastPrice,
        /// <summary>
        /// Index price
        /// </summary>
        [Map("index")]
        IndexPrice,
        /// <summary>
        /// Mark price
        /// </summary>
        [Map("mark")]
        MarkPrice
    }
}

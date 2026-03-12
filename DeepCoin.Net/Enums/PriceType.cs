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
        /// ["<c>last</c>"] Last price
        /// </summary>
        [Map("last")]
        LastPrice,
        /// <summary>
        /// ["<c>index</c>"] Index price
        /// </summary>
        [Map("index")]
        IndexPrice,
        /// <summary>
        /// ["<c>mark</c>"] Mark price
        /// </summary>
        [Map("mark")]
        MarkPrice
    }
}

using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// XXX
    /// </summary>
    [JsonConverter(typeof(EnumConverter<DeepCoinEnum>))]
    public enum DeepCoinEnum
    {
        /// <summary>
        /// XXX
        /// </summary>
        [Map("XXX")]
        XXX,
    }
}

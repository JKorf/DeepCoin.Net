using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Symbol type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<SymbolType>))]
    public enum SymbolType
    {
        /// <summary>
        /// Spot symbol
        /// </summary>
        [Map("SPOT")]
        Spot,
        /// <summary>
        /// Swap symbol
        /// </summary>
        [Map("SWAP")]
        Swap
    }
}

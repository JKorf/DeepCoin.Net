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
        /// ["<c>SPOT</c>"] Spot symbol
        /// </summary>
        [Map("SPOT")]
        Spot,
        /// <summary>
        /// ["<c>SWAP</c>"] Swap symbol
        /// </summary>
        [Map("SWAP")]
        Swap
    }
}

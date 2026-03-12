using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Product group
    /// </summary>
    [JsonConverter(typeof(EnumConverter<ProductGroup>))]
    public enum ProductGroup
    {
        /// <summary>
        /// ["<c>SwapU</c>"] USDT margined
        /// </summary>
        [Map("SwapU")]
        USDTMargined,
        /// <summary>
        /// ["<c>Swap</c>"] Coin margined
        /// </summary>
        [Map("Swap")]
        CoinMargined,
        /// <summary>
        /// ["<c>Spot</c>"] Spot
        /// </summary>
        [Map("Spot")]
        Spot
    }
}

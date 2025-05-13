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
        /// USDT margined
        /// </summary>
        [Map("SwapU")]
        USDTMargined,
        /// <summary>
        /// Coin margined
        /// </summary>
        [Map("Swap")]
        CoinMargined,
        /// <summary>
        /// Spot
        /// </summary>
        [Map("Spot")]
        Spot
    }
}

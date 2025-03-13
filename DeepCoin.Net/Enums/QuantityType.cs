using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Quantity type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<QuantityType>))]
    public enum QuantityType
    {
        /// <summary>
        /// Quantity in base asset
        /// </summary>
        [Map("base_ccy", "0")]
        BaseAsset,
        /// <summary>
        /// Quantity in quote asset
        /// </summary>
        [Map("quote_ccy", "1")]
        QuoteAsset
    }
}

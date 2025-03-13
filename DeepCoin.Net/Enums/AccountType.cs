using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Account type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<AccountType>))]
    public enum AccountType
    {
        /// <summary>
        /// Email
        /// </summary>
        [Map("email")]
        Email,
        /// <summary>
        /// Phone
        /// </summary>
        [Map("phone")]
        Phone
    }
}

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
        /// ["<c>email</c>"] Email
        /// </summary>
        [Map("email")]
        Email,
        /// <summary>
        /// ["<c>phone</c>"] Phone
        /// </summary>
        [Map("phone")]
        Phone
    }
}

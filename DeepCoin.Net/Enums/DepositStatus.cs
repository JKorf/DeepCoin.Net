using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Deposit status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<DepositStatus>))]
    public enum DepositStatus
    {
        /// <summary>
        /// Confirming
        /// </summary>
        [Map("confirming")]
        Confirming,
        /// <summary>
        /// Success
        /// </summary>
        [Map("succeed")]
        Success
    }
}

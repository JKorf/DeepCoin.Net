using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Withdrawal status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<WithdrawStatus>))]
    public enum WithdrawStatus
    {
        /// <summary>
        /// Auditing
        /// </summary>
        [Map("auditing")]
        Auditing,
        /// <summary>
        /// Confirming
        /// </summary>
        [Map("confirming")]
        Confirming,
        /// <summary>
        /// Rejected
        /// </summary>
        [Map("rejected")]
        Rejected,
        /// <summary>
        /// Success
        /// </summary>
        [Map("succeed")]
        Success
    }
}

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
        /// ["<c>auditing</c>"] Auditing
        /// </summary>
        [Map("auditing")]
        Auditing,
        /// <summary>
        /// ["<c>confirming</c>"] Confirming
        /// </summary>
        [Map("confirming")]
        Confirming,
        /// <summary>
        /// ["<c>rejected</c>"] Rejected
        /// </summary>
        [Map("rejected")]
        Rejected,
        /// <summary>
        /// ["<c>succeed</c>"] Success
        /// </summary>
        [Map("succeed")]
        Success
    }
}

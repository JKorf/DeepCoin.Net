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
        /// ["<c>confirming</c>"] Confirming
        /// </summary>
        [Map("confirming")]
        Confirming,
        /// <summary>
        /// ["<c>succeed</c>"] Success
        /// </summary>
        [Map("succeed")]
        Success
    }
}

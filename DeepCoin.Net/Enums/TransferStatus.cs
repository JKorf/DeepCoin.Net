using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Transfer status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TransferStatus>))]
    public enum TransferStatus
    {
        /// <summary>
        /// Processing
        /// </summary>
        [Map("1", "In Process")]
        Processing,
        /// <summary>
        /// Approved
        /// </summary>
        [Map("2")]
        Approved,
        /// <summary>
        /// Rejected
        /// </summary>
        [Map("3")]
        Rejected
    }
}

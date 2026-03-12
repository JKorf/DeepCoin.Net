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
        /// ["<c>1</c>"] Processing
        /// </summary>
        [Map("1", "In Process")]
        Processing,
        /// <summary>
        /// ["<c>2</c>"] Approved
        /// </summary>
        [Map("2")]
        Approved,
        /// <summary>
        /// ["<c>3</c>"] Rejected
        /// </summary>
        [Map("3")]
        Rejected
    }
}

using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Symbol status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<SymbolStatus>))]
    public enum SymbolStatus
    {
        /// <summary>
        /// ["<c>live</c>"] Live
        /// </summary>
        [Map("live", "2")]
        Live,
        /// <summary>
        /// ["<c>suspend</c>"] Suspended
        /// </summary>
        [Map("suspend", "1")]
        Suspended,
        /// <summary>
        /// ["<c>preopen</c>"] Pre-open
        /// </summary>
        [Map("preopen", "0")]
        PreOpen,
        /// <summary>
        /// ["<c>settlement</c>"] Funding fee settling
        /// </summary>
        [Map("settlement", "3", "4", "5")]
        Settling,
        /// <summary>
        /// ["<c>6</c>"] Closing
        /// </summary>
        [Map("6")]
        Closing,
        /// <summary>
        /// ["<c>7</c>"] Inactive
        /// </summary>
        [Map("7")]
        Inactive,
    }
}

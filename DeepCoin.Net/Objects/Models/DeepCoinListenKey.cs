using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Listen key
    /// </summary>
    [SerializationModel]
    public record DeepCoinListenKey
    {
        /// <summary>
        /// Listen key
        /// </summary>
        [JsonPropertyName("listenkey")]
        public string ListenKey { get; set; } = string.Empty;
        /// <summary>
        /// Expire time
        /// </summary>
        [JsonPropertyName("expire_time")]
        public DateTime ExpireTime { get; set; }
    }
}

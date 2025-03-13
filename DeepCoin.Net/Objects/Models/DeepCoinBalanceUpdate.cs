using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Balance update
    /// </summary>
    [SerializationModel]
    public record DeepCoinBalanceUpdate
    {
        /// <summary>
        /// Account id
        /// </summary>
        [JsonPropertyName("A")]
        public string AccountId { get; set; } = string.Empty;
        /// <summary>
        /// Balance
        /// </summary>
        [JsonPropertyName("B")]
        public decimal Balance { get; set; }
        /// <summary>
        /// Asset name
        /// </summary>
        [JsonPropertyName("C")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// User id
        /// </summary>
        [JsonPropertyName("M")]
        public string UserId { get; set; } = string.Empty;
        /// <summary>
        /// Withdrawable quantity
        /// </summary>
        [JsonPropertyName("W")]
        public decimal Withdrawable { get; set; }
        /// <summary>
        /// Available quantity
        /// </summary>
        [JsonPropertyName("a")]
        public decimal Available { get; set; }
        ///// <summary>
        ///// C
        ///// </summary>
        //[JsonPropertyName("c")]
        //public decimal C { get; set; }
        /// <summary>
        /// Margin used
        /// </summary>
        [JsonPropertyName("u")]
        public decimal MarginUsed { get; set; }
    }
}

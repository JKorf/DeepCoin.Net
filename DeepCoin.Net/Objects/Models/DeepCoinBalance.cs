using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Balance info
    /// </summary>
    public record DeepCoinBalance
    {
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("ccy")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Total balance
        /// </summary>
        [JsonPropertyName("bal")]
        public decimal Balance { get; set; }
        /// <summary>
        /// Frozen balance
        /// </summary>
        [JsonPropertyName("frozenBal")]
        public decimal FrozenBalance { get; set; }
        /// <summary>
        /// Avail balance
        /// </summary>
        [JsonPropertyName("availBal")]
        public decimal AvailBalance { get; set; }
    }
}

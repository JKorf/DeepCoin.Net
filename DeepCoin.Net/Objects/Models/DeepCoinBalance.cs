using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Balance info
    /// </summary>
    [SerializationModel]
    public record DeepCoinBalance
    {
        /// <summary>
        /// ["<c>ccy</c>"] Asset
        /// </summary>
        [JsonPropertyName("ccy")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>bal</c>"] Total balance
        /// </summary>
        [JsonPropertyName("bal")]
        public decimal Balance { get; set; }
        /// <summary>
        /// ["<c>frozenBal</c>"] Frozen balance
        /// </summary>
        [JsonPropertyName("frozenBal")]
        public decimal FrozenBalance { get; set; }
        /// <summary>
        /// ["<c>availBal</c>"] Available balance
        /// </summary>
        [JsonPropertyName("availBal")]
        public decimal AvailableBalance { get; set; }
    }
}

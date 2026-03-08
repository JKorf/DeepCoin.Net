using CryptoExchange.Net.Converters.SystemTextJson;
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
        /// ["<c>A</c>"] Account id
        /// </summary>
        [JsonPropertyName("A")]
        public string AccountId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>B</c>"] Balance
        /// </summary>
        [JsonPropertyName("B")]
        public decimal Balance { get; set; }
        /// <summary>
        /// ["<c>C</c>"] Asset name
        /// </summary>
        [JsonPropertyName("C")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>M</c>"] User id
        /// </summary>
        [JsonPropertyName("M")]
        public string UserId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>W</c>"] Withdrawable quantity
        /// </summary>
        [JsonPropertyName("W")]
        public decimal Withdrawable { get; set; }
        /// <summary>
        /// ["<c>a</c>"] Available quantity
        /// </summary>
        [JsonPropertyName("a")]
        public decimal Available { get; set; }
        ///// <summary>
        ///// C
        ///// </summary>
        //[JsonPropertyName("c")]
        //public decimal C { get; set; }
        /// <summary>
        /// ["<c>u</c>"] Margin used
        /// </summary>
        [JsonPropertyName("u")]
        public decimal MarginUsed { get; set; }
    }
}

using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// The owner of the authenticated API key.
    /// </summary>
    [SerializationModel]
    public record DeepCoinUserId
    {
        /// <summary>
        /// ["<c>uid</c>"] Current user UID.
        /// </summary>
        [JsonPropertyName("uid")]
        public long UserId { get; set; }
    }
}

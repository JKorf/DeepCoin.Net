using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Order result
    /// </summary>
    [SerializationModel]
    public record DeepCoinOrderResult
    {
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("ordId")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("clOrdId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Tag
        /// </summary>
        [JsonPropertyName("tag")]
        public string? Tag { get; set; }
        /// <summary>
        /// Result code
        /// </summary>
        [JsonPropertyName("sCode")]
        public int ResultCode { get; set; }
        [JsonInclude, JsonPropertyName("errorCode")]
        internal int ResultCodeInt { get => ResultCode; set => ResultCode = value; }
        /// <summary>
        /// Result message
        /// </summary>
        [JsonPropertyName("sMsg")]
        public string? ResultMessage { get; set; }
        [JsonInclude, JsonPropertyName("errorMsg")]
        internal string? ResultMessageInt { get => ResultMessage; set => ResultMessage = value; }
    }


}

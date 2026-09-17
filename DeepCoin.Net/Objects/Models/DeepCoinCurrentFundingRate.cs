using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Current funding rate estimate for one perpetual contract.
    /// </summary>
    [SerializationModel]
    public record DeepCoinCurrentFundingRate
    {
        /// <summary>
        /// ["<c>instrumentId</c>"] Native instrument identifier, for example BTCUSDT.
        /// </summary>
        [JsonPropertyName("instrumentId")]
        public string InstrumentId { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>fundingRate</c>"] Funding rate as a decimal fraction.
        /// </summary>
        [JsonPropertyName("fundingRate")]
        public decimal FundingRate { get; set; }
    }
}

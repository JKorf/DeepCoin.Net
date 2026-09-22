using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Current funding rate estimates returned by the funding endpoint.
    /// </summary>
    [SerializationModel]
    public record DeepCoinCurrentFundingRates
    {
        /// <summary>
        /// ["<c>current_fund_rates</c>"] Current estimates by native instrument identifier.
        /// </summary>
        [JsonPropertyName("current_fund_rates")]
        public DeepCoinCurrentFundingRate[] Rates { get; set; } = Array.Empty<DeepCoinCurrentFundingRate>();
    }
}

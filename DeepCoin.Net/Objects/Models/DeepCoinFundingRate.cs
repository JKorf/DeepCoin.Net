using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Funding rate
    /// </summary>
    [SerializationModel]
    public record DeepCoinFundingRate
    {
        /// <summary>
        /// ["<c>settleInterval</c>"] Settle interval
        /// </summary>
        [JsonPropertyName("settleInterval")]
        public int SettleInterval { get; set; }
        /// <summary>
        /// ["<c>instrumentID</c>"] Instrument id
        /// </summary>
        [JsonPropertyName("instrumentID")]
        public string InstrumentId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>nextSettleTime</c>"] Next settle time
        /// </summary>
        [JsonPropertyName("nextSettleTime")]
        public DateTime NextSettleTime { get; set; }
    }


}

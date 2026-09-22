using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// V2 consolidated funding rate and settlement schedule.
    /// </summary>
    [SerializationModel]
    public record DeepCoinV2FundingRate
    {
        /// <summary>
        /// ["<c>instId</c>"] Instrument identifier. Live responses may use compact identifiers such as BTCUSDT.
        /// </summary>
        [JsonPropertyName("instId")]
        public string InstrumentId { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>fundingRate</c>"] Current fractional funding rate reported by the V2 endpoint.
        /// </summary>
        [JsonPropertyName("fundingRate")]
        public decimal FundingRate { get; set; }

        /// <summary>
        /// ["<c>settleInterval</c>"] Settlement interval in seconds.
        /// </summary>
        [JsonPropertyName("settleInterval")]
        public long SettleInterval { get; set; }

        /// <summary>
        /// ["<c>nextSettleTime</c>"] Next settlement time, expressed as Unix seconds on the wire.
        /// </summary>
        [JsonPropertyName("nextSettleTime")]
        public DateTime NextSettleTime { get; set; }
    }
}

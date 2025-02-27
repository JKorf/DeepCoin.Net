using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Funding rate
    /// </summary>
    public record DeepCoinFundingRate
    {
        /// <summary>
        /// Settle interval
        /// </summary>
        [JsonPropertyName("settleInterval")]
        public int SettleInterval { get; set; }
        /// <summary>
        /// Instrument id
        /// </summary>
        [JsonPropertyName("instrumentID")]
        public string InstrumentId { get; set; } = string.Empty;
        /// <summary>
        /// Next settle time
        /// </summary>
        [JsonPropertyName("nextSettleTime")]
        public DateTime NextSettleTime { get; set; }
    }


}

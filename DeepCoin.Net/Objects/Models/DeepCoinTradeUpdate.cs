using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Trade update
    /// </summary>
    [SerializationModel]
    public record DeepCoinTradeUpdate
    {
        /// <summary>
        /// Trade id
        /// </summary>
        [JsonPropertyName("TradeID")]
        public string TradeId { get; set; } = string.Empty;
        /// <summary>
        /// Exchange id
        /// </summary>
        [JsonPropertyName("ExchangeID")]
        public string ExchangeId { get; set; } = string.Empty;
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("InstrumentID")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Trade direction
        /// </summary>
        [JsonPropertyName("Direction")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Trade price
        /// </summary>
        [JsonPropertyName("Price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Trade quantity
        /// </summary>
        [JsonPropertyName("Volume")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Trade timestamp
        /// </summary>
        [JsonPropertyName("TradeTime")]
        public DateTime Timestamp { get; set; }
    }
}

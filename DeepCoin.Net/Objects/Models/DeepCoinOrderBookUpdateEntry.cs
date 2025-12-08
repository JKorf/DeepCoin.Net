using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using DeepCoin.Net.Enums;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Order book update
    /// </summary>
    [SerializationModel]
    public record DeepCoinOrderBookUpdate
    {
        /// <summary>
        /// Sequence number
        /// </summary>
        public long SequenceNumber { get; set; }
        /// <summary>
        /// Updated asks
        /// </summary>
        public DeepCoinOrderBookUpdateEntry[] Asks { get; set; } = [];
        /// <summary>
        /// Updated bids
        /// </summary>
        public DeepCoinOrderBookUpdateEntry[] Bids { get; set; } = [];
    }

    /// <summary>
    /// Order book update entry
    /// </summary>
    [SerializationModel]
    public record DeepCoinOrderBookUpdateEntry : ISymbolOrderBookEntry
    {
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
        public OrderSide Direction { get; set; }
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
        /// Number of orders
        /// </summary>
        [JsonPropertyName("Orders")]
        public int Orders { get; set; }
    }
}

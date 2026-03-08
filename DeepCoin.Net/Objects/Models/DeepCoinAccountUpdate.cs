using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Account update
    /// </summary>
    [SerializationModel]
    public record DeepCoinAccountUpdate
    {
        /// <summary>
        /// ["<c>A</c>"] Account id
        /// </summary>
        [JsonPropertyName("A")]
        public string AccountId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>AD</c>"] Account detail id
        /// </summary>
        [JsonPropertyName("AD")]
        public string AccountDetailId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>Am</c>"] Quantity
        /// </summary>
        [JsonPropertyName("Am")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>B</c>"] Static balance
        /// </summary>
        [JsonPropertyName("B")]
        public decimal Balance { get; set; }
        /// <summary>
        /// ["<c>C</c>"] Asset
        /// </summary>
        [JsonPropertyName("C")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>I</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("I")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>IT</c>"] Create time
        /// </summary>
        [JsonPropertyName("IT")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>M</c>"] User id
        /// </summary>
        [JsonPropertyName("M")]
        public decimal UserId { get; set; }
        /// <summary>
        /// ["<c>PB</c>"] Pre-change balance
        /// </summary>
        [JsonPropertyName("PB")]
        public decimal PreBalance { get; set; }
        /// <summary>
        /// ["<c>R</c>"] Remark
        /// </summary>
        [JsonPropertyName("R")]
        public string? Remark { get; set; }
        /// <summary>
        /// ["<c>S</c>"] Transaction type
        /// </summary>
        [JsonPropertyName("S")]
        public string TransactionType { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>r</c>"] Related change id
        /// </summary>
        [JsonPropertyName("r")]
        public string RelatedId { get; set; } = string.Empty;
    }


}

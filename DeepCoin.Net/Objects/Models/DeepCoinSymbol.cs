using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Symbol info
    /// </summary>
    [SerializationModel]
    public record DeepCoinSymbol
    {
        /// <summary>
        /// ["<c>instType</c>"] Symbol type
        /// </summary>
        [JsonPropertyName("instType")]
        public SymbolType SymbolType { get; set; }
        /// <summary>
        /// ["<c>instId</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("instId")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>uly</c>"] Underlying
        /// </summary>
        [JsonPropertyName("uly")]
        public string? Underlying { get; set; }
        /// <summary>
        /// ["<c>baseCcy</c>"] Base asset
        /// </summary>
        [JsonPropertyName("baseCcy")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>quoteCcy</c>"] Quote asset
        /// </summary>
        [JsonPropertyName("quoteCcy")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>ctVal</c>"] Size of a single contract
        /// </summary>
        [JsonPropertyName("ctVal")]
        public decimal? ContractSize { get; set; }
        /// <summary>
        /// ["<c>ctValCcy</c>"] Asset the contract size is in
        /// </summary>
        [JsonPropertyName("ctValCcy")]
        public string? ContractSizeAsset { get; set; }
        /// <summary>
        /// ["<c>listTime</c>"] List time
        /// </summary>
        [JsonPropertyName("listTime")]
        public DateTime? ListTime { get; set; }
        /// <summary>
        /// ["<c>lever</c>"] Max leverage
        /// </summary>
        [JsonPropertyName("lever")]
        public decimal? MaxLeverage { get; set; }
        /// <summary>
        /// ["<c>tickSz</c>"] Tick quantity
        /// </summary>
        [JsonPropertyName("tickSz")]
        public decimal TickSize { get; set; }
        /// <summary>
        /// ["<c>lotSz</c>"] Lot quantity
        /// </summary>
        [JsonPropertyName("lotSz")]
        public decimal LotSize { get; set; }
        /// <summary>
        /// ["<c>minSz</c>"] Min order quantity
        /// </summary>
        [JsonPropertyName("minSz")]
        public decimal MinQuantity { get; set; }
        /// <summary>
        /// ["<c>ctType</c>"] Contract type
        /// </summary>
        [JsonPropertyName("ctType")]
        public ContractType? ContractType { get; set; }
        /// <summary>
        /// ["<c>alias</c>"] Alias
        /// </summary>
        [JsonPropertyName("alias")]
        public string? Alias { get; set; }
        /// <summary>
        /// ["<c>state</c>"] Status
        /// </summary>
        [JsonPropertyName("state")]
        public SymbolStatus Status { get; set; }
        /// <summary>
        /// ["<c>maxLmtSz</c>"] Max limit order quantity
        /// </summary>
        [JsonPropertyName("maxLmtSz")]
        public decimal MaxLimitQuantity { get; set; }
        /// <summary>
        /// ["<c>maxMktSz</c>"] Max market order quantity
        /// </summary>
        [JsonPropertyName("maxMktSz")]
        public decimal MaxMarketQuantity { get; set; }
    }


}

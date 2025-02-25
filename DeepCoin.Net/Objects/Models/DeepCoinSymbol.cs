using DeepCoin.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Models
{
    /// <summary>
    /// Symbol info
    /// </summary>
    public record DeepCoinSymbol
    {
        /// <summary>
        /// Symbol type
        /// </summary>
        [JsonPropertyName("instType")]
        public SymbolType SymbolType { get; set; }
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("instId")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Underlying
        /// </summary>
        [JsonPropertyName("uly")]
        public string? Underlying { get; set; }
        /// <summary>
        /// Base asset
        /// </summary>
        [JsonPropertyName("baseCcy")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// Quote asset
        /// </summary>
        [JsonPropertyName("quoteCcy")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// Size of a single contract
        /// </summary>
        [JsonPropertyName("ctVal")]
        public decimal? ContractSize { get; set; }
        /// <summary>
        /// Asset the contract size is in
        /// </summary>
        [JsonPropertyName("ctValCcy")]
        public string? ContractSizeAsset { get; set; }
        /// <summary>
        /// List time
        /// </summary>
        [JsonPropertyName("listTime")]
        public DateTime? ListTime { get; set; }
        /// <summary>
        /// Max leverage
        /// </summary>
        [JsonPropertyName("lever")]
        public decimal? MaxLeverage { get; set; }
        /// <summary>
        /// Tick quantity
        /// </summary>
        [JsonPropertyName("tickSz")]
        public decimal TickQuantity { get; set; }
        /// <summary>
        /// Lot quantity
        /// </summary>
        [JsonPropertyName("lotSz")]
        public decimal LotQuantity { get; set; }
        /// <summary>
        /// Min order quantity
        /// </summary>
        [JsonPropertyName("minSz")]
        public decimal MinQuantity { get; set; }
        /// <summary>
        /// Contract type
        /// </summary>
        [JsonPropertyName("ctType")]
        public ContractType? ContractType { get; set; }
        /// <summary>
        /// Alias
        /// </summary>
        [JsonPropertyName("alias")]
        public string? Alias { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("state")]
        public SymbolStatus Status { get; set; }
        /// <summary>
        /// Max limit order quantity
        /// </summary>
        [JsonPropertyName("maxLmtSz")]
        public decimal MaxLimitQuantity { get; set; }
        /// <summary>
        /// Max market order quantity
        /// </summary>
        [JsonPropertyName("maxMktSz")]
        public decimal MaxMarketQuantity { get; set; }
    }


}

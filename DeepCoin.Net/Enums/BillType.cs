using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Bill type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<BillType>))]
    public enum BillType
    {
        /// <summary>
        /// ["<c>2</c>"] Fund in
        /// </summary>
        [Map("2")]
        FundIncome,
        /// <summary>
        /// ["<c>3</c>"] Fund out
        /// </summary>
        [Map("3")]
        FundExpense,
        /// <summary>
        /// ["<c>4</c>"] Fund transfer
        /// </summary>
        [Map("4")]
        FundTransfer,
        /// <summary>
        /// ["<c>5</c>"] Fee
        /// </summary>
        [Map("5")]
        Fee
    }
}

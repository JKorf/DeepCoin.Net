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
        /// Fund in
        /// </summary>
        [Map("2")]
        FundIncome,
        /// <summary>
        /// Fund out
        /// </summary>
        [Map("3")]
        FundExpense,
        /// <summary>
        /// Fund transfer
        /// </summary>
        [Map("4")]
        FundTransfer,
        /// <summary>
        /// Fee
        /// </summary>
        [Map("5")]
        Fee
    }
}

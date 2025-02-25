using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Bill type
    /// </summary>
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

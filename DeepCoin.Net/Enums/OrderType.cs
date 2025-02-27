using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Order type
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// Limit order
        /// </summary>
        [Map("limit")]
        Limit,
        /// <summary>
        /// Market order
        /// </summary>
        [Map("market")]
        Market,
        /// <summary>
        /// Post only order
        /// </summary>
        [Map("post_only")]
        PostOnly,
        /// <summary>
        /// Immediate or cancel
        /// </summary>
        [Map("ioc")]
        ImmediateOrCancel
    }
}

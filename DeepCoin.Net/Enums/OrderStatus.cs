using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Order status
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// Open order
        /// </summary>
        [Map("live", "4")]
        Live,
        /// <summary>
        /// Partially filled open order
        /// </summary>
        [Map("partially_filled")]
        PartiallyFilled,
        /// <summary>
        /// Canceled
        /// </summary>
        [Map("canceled", "6")]
        Canceled,
        /// <summary>
        /// Filled
        /// </summary>
        [Map("filled", "1")]
        Filled
    }
}

using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Order side
    /// </summary>
    public enum OrderSide
    {
        /// <summary>
        /// Buy
        /// </summary>
        [Map("0")]
        Buy,
        /// <summary>
        /// Sell
        /// </summary>
        [Map("1")]
        Sell
    }
}

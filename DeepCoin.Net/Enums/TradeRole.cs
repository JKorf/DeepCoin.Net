using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Trade role
    /// </summary>
    public enum TradeRole
    {
        /// <summary>
        /// Taker
        /// </summary>
        [Map("T", "1")]
        Taker,
        /// <summary>
        /// Maker
        /// </summary>
        [Map("M", "0")]
        Maker
    }
}

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
        [Map("T")]
        Taker,
        /// <summary>
        /// Maker
        /// </summary>
        [Map("M")]
        Maker
    }
}

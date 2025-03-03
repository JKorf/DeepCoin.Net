using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Trade mode
    /// </summary>
    public enum TradeMode
    {
        /// <summary>
        /// Spot
        /// </summary>
        [Map("cash")]
        Spot,
        /// <summary>
        /// Cross margin
        /// </summary>
        [Map("cross")]
        Cross,
        /// <summary>
        /// Isolated margin
        /// </summary>
        [Map("isolated")]
        Isolated
    }
}

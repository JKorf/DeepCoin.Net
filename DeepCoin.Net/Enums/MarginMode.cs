using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Margin mode
    /// </summary>
    public enum MarginMode
    {
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

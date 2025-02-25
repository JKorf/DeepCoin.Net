using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Symbol type
    /// </summary>
    public enum SymbolType
    {
        /// <summary>
        /// Spot symbol
        /// </summary>
        [Map("SPOT")]
        Spot,
        /// <summary>
        /// Swap symbol
        /// </summary>
        [Map("SWAP")]
        Swap
    }
}

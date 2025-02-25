using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Position type
    /// </summary>
    public enum PositionType
    {
        /// <summary>
        /// Merge position
        /// </summary>
        [Map("merge")]
        Merge,
        /// <summary>
        /// Split position
        /// </summary>
        [Map("split")]
        Split
    }
}

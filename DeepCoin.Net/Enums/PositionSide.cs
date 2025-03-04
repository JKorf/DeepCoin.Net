using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Position side
    /// </summary>
    public enum PositionSide
    {
        /// <summary>
        /// Long
        /// </summary>
        [Map("long", "0")]
        Long,
        /// <summary>
        /// Short
        /// </summary>
        [Map("short", "1")]
        Short
    }
}

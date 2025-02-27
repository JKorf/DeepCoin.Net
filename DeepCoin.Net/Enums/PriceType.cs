using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Price type
    /// </summary>
    public enum PriceType
    {
        /// <summary>
        /// Last price
        /// </summary>
        [Map("last")]
        LastPrice,
        /// <summary>
        /// Index price
        /// </summary>
        [Map("index")]
        IndexPrice,
        /// <summary>
        /// Mark price
        /// </summary>
        [Map("mark")]
        MarkPrice
    }
}

using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Quantity type
    /// </summary>
    public enum QuantityType
    {
        /// <summary>
        /// Quantity in base asset
        /// </summary>
        [Map("0")]
        BaseAsset,
        /// <summary>
        /// Quantity in quote asset
        /// </summary>
        [Map("1")]
        QuoteAsset
    }
}

using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Product group
    /// </summary>
    public enum ProductGroup
    {
        /// <summary>
        /// USDT margined
        /// </summary>
        [Map("SwapU")]
        USDTMargined,
        /// <summary>
        /// Coin margined
        /// </summary>
        [Map("Swap")]
        CoinMargined
    }
}

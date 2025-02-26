using CryptoExchange.Net.Objects.Options;
using System;

namespace DeepCoin.Net.Objects.Options
{
    /// <summary>
    /// Options for the DeepCoin SymbolOrderBook
    /// </summary>
    public class DeepCoinOrderBookOptions : OrderBookOptions
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        public static DeepCoinOrderBookOptions Default { get; set; } = new DeepCoinOrderBookOptions();

        /// <summary>
        /// After how much time we should consider the connection dropped if no data is received for this time after the initial subscriptions
        /// </summary>
        public TimeSpan? InitialDataTimeout { get; set; }

        internal DeepCoinOrderBookOptions Copy()
        {
            var result = Copy<DeepCoinOrderBookOptions>();
            result.InitialDataTimeout = InitialDataTimeout;
            return result;
        }
    }
}

using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using System;
using DeepCoin.Net.Objects.Options;

namespace DeepCoin.Net.Interfaces
{
    /// <summary>
    /// DeepCoin local order book factory
    /// </summary>
    public interface IDeepCoinOrderBookFactory
    {
        
        /// <summary>
        /// Exchange order book factory methods
        /// </summary>
        IOrderBookFactory<DeepCoinOrderBookOptions> Exchange { get; }


        /// <summary>
        /// Create a SymbolOrderBook for the symbol
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="options">Book options</param>
        /// <returns></returns>
        ISymbolOrderBook Create(SharedSymbol symbol, Action<DeepCoinOrderBookOptions>? options = null);

        
        /// <summary>
        /// Create a new Exchange local order book instance
        /// </summary>
        ISymbolOrderBook Create(string symbol, Action<DeepCoinOrderBookOptions>? options = null);

    }
}
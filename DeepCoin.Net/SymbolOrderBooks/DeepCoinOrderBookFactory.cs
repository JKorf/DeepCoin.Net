using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.OrderBook;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using DeepCoin.Net.Interfaces;
using DeepCoin.Net.Interfaces.Clients;
using DeepCoin.Net.Objects.Options;

namespace DeepCoin.Net.SymbolOrderBooks
{
    /// <summary>
    /// DeepCoin order book factory
    /// </summary>
    public class DeepCoinOrderBookFactory : IDeepCoinOrderBookFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public DeepCoinOrderBookFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;            
            
            Exchange = new OrderBookFactory<DeepCoinOrderBookOptions>(Create, Create);
        }

        
         /// <inheritdoc />
        public IOrderBookFactory<DeepCoinOrderBookOptions> Exchange { get; }


        /// <inheritdoc />
        public ISymbolOrderBook Create(SharedSymbol symbol, Action<DeepCoinOrderBookOptions>? options = null)
        {
            var symbolName = symbol.GetSymbol(DeepCoinExchange.FormatWebsocketSymbol);
            return Create(symbolName, options);
        }

        
         /// <inheritdoc />
        public ISymbolOrderBook Create(string symbol, Action<DeepCoinOrderBookOptions>? options = null)
            => new DeepCoinSymbolOrderBook(symbol, options, 
                                                          _serviceProvider.GetRequiredService<ILoggerFactory>(),
                                                          _serviceProvider.GetRequiredService<IDeepCoinSocketClient>());


    }
}

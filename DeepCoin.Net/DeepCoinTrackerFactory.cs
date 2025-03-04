using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Trackers.Klines;
using CryptoExchange.Net.Trackers.Trades;
using DeepCoin.Net.Interfaces;
using DeepCoin.Net.Interfaces.Clients;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using DeepCoin.Net.Clients;

namespace DeepCoin.Net
{
    /// <inheritdoc />
    public class DeepCoinTrackerFactory : IDeepCoinTrackerFactory
    {
        private readonly IServiceProvider? _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        public DeepCoinTrackerFactory()
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public DeepCoinTrackerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public IKlineTracker CreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval, int? limit = null, TimeSpan? period = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<IDeepCoinRestClient>() ?? new DeepCoinRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<IDeepCoinSocketClient>() ?? new DeepCoinSocketClient();

            return new KlineTracker(
                _serviceProvider?.GetRequiredService<ILoggerFactory>().CreateLogger(restClient.Exchange),
                restClient.ExchangeApi.SharedClient,
                socketClient.ExchangeApi.SharedClient,
                symbol,
                interval,
                limit,
                period
                );
        }
        /// <inheritdoc />
        public ITradeTracker CreateTradeTracker(SharedSymbol symbol, int? limit = null, TimeSpan? period = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<IDeepCoinRestClient>() ?? new DeepCoinRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<IDeepCoinSocketClient>() ?? new DeepCoinSocketClient();
                        
            return new TradeTracker(
                _serviceProvider?.GetRequiredService<ILoggerFactory>().CreateLogger(restClient.Exchange),
                null,
                null,
                socketClient.ExchangeApi.SharedClient,
                symbol,
                limit,
                period
                );
        }
    }
}

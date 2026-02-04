using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Trackers.Klines;
using CryptoExchange.Net.Trackers.Trades;
using CryptoExchange.Net.Trackers.UserData.Interfaces;
using CryptoExchange.Net.Trackers.UserData.Objects;
using DeepCoin.Net.Clients;
using DeepCoin.Net.Interfaces;
using DeepCoin.Net.Interfaces.Clients;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;

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
        public bool CanCreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval)
        {
            var client = (_serviceProvider?.GetRequiredService<IDeepCoinSocketClient>() ?? new DeepCoinSocketClient());
            return client.ExchangeApi.SharedClient.SubscribeKlineOptions.IsSupported(interval);
        }

        /// <inheritdoc />
        public bool CanCreateTradeTracker(SharedSymbol symbol) => true;

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

        /// <inheritdoc />
        public IUserSpotDataTracker CreateUserSpotDataTracker(SpotUserDataTrackerConfig config)
        {
            var restClient = _serviceProvider?.GetRequiredService<IDeepCoinRestClient>() ?? new DeepCoinRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<IDeepCoinSocketClient>() ?? new DeepCoinSocketClient();
            return new DeepCoinUserSpotDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<DeepCoinUserSpotDataTracker>>() ?? new NullLogger<DeepCoinUserSpotDataTracker>(),
                restClient,
                socketClient,
                null,
                config
                );
        }

        /// <inheritdoc />
        public IUserSpotDataTracker CreateUserSpotDataTracker(string userIdentifier, SpotUserDataTrackerConfig config, ApiCredentials credentials, DeepCoinEnvironment? environment = null)
        {
            var clientProvider = _serviceProvider?.GetRequiredService<IDeepCoinUserClientProvider>() ?? new DeepCoinUserClientProvider();
            var restClient = clientProvider.GetRestClient(userIdentifier, credentials, environment);
            var socketClient = clientProvider.GetSocketClient(userIdentifier, credentials, environment);
            return new DeepCoinUserSpotDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<DeepCoinUserSpotDataTracker>>() ?? new NullLogger<DeepCoinUserSpotDataTracker>(),
                restClient,
                socketClient,
                userIdentifier,
                config
                );
        }

        /// <inheritdoc />
        public IUserFuturesDataTracker CreateUserFuturesDataTracker(FuturesUserDataTrackerConfig config)
        {
            var restClient = _serviceProvider?.GetRequiredService<IDeepCoinRestClient>() ?? new DeepCoinRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<IDeepCoinSocketClient>() ?? new DeepCoinSocketClient();
            return new DeepCoinUserFuturesDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<DeepCoinUserFuturesDataTracker>>() ?? new NullLogger<DeepCoinUserFuturesDataTracker>(),
                restClient,
                socketClient,
                null,
                config
                );
        }

        /// <inheritdoc />
        public IUserFuturesDataTracker CreateUserFuturesDataTracker(string userIdentifier, FuturesUserDataTrackerConfig config, ApiCredentials credentials, DeepCoinEnvironment? environment = null)
        {
            var clientProvider = _serviceProvider?.GetRequiredService<IDeepCoinUserClientProvider>() ?? new DeepCoinUserClientProvider();
            var restClient = clientProvider.GetRestClient(userIdentifier, credentials, environment);
            var socketClient = clientProvider.GetSocketClient(userIdentifier, credentials, environment);
            return new DeepCoinUserFuturesDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<DeepCoinUserFuturesDataTracker>>() ?? new NullLogger<DeepCoinUserFuturesDataTracker>(),
                restClient,
                socketClient,
                userIdentifier,
                config
                );
        }
    }
}

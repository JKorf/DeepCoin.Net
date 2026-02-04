using DeepCoin.Net.Interfaces.Clients;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Trackers.UserData;
using CryptoExchange.Net.Trackers.UserData.Objects;
using Microsoft.Extensions.Logging;

namespace DeepCoin.Net
{
    /// <inheritdoc/>
    public class DeepCoinUserSpotDataTracker : UserSpotDataTracker
    {
        /// <summary>
        /// ctor
        /// </summary>
        public DeepCoinUserSpotDataTracker(
            ILogger<DeepCoinUserSpotDataTracker> logger,
            IDeepCoinRestClient restClient,
            IDeepCoinSocketClient socketClient,
            string? userIdentifier,
            SpotUserDataTrackerConfig config) : base(
                logger,
                restClient.ExchangeApi.SharedClient,
                restClient.ExchangeApi.SharedClient,
                restClient.ExchangeApi.SharedClient,
                socketClient.ExchangeApi.SharedClient,
                restClient.ExchangeApi.SharedClient,
                socketClient.ExchangeApi.SharedClient,
                socketClient.ExchangeApi.SharedClient,
                userIdentifier,
                config)
        {
        }
    }

    /// <inheritdoc/>
    public class DeepCoinUserFuturesDataTracker : UserFuturesDataTracker
    {
        /// <inheritdoc/>
        protected override bool WebsocketPositionUpdatesAreFullSnapshots => false;

        /// <summary>
        /// ctor
        /// </summary>
        public DeepCoinUserFuturesDataTracker(
            ILogger<DeepCoinUserFuturesDataTracker> logger,
            IDeepCoinRestClient restClient,
            IDeepCoinSocketClient socketClient,
            string? userIdentifier,
            FuturesUserDataTrackerConfig config) : base(logger,
                restClient.ExchangeApi.SharedClient,
                restClient.ExchangeApi.SharedClient,
                restClient.ExchangeApi.SharedClient,
                socketClient.ExchangeApi.SharedClient,
                restClient.ExchangeApi.SharedClient,
                socketClient.ExchangeApi.SharedClient,
                socketClient.ExchangeApi.SharedClient,
                socketClient.ExchangeApi.SharedClient,
                userIdentifier,
                config)
        {
        }
    }
}

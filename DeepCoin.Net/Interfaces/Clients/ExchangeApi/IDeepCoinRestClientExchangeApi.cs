using CryptoExchange.Net.Interfaces.Clients;
using System;

namespace DeepCoin.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// DeepCoin Exchange API endpoints
    /// </summary>
    public interface IDeepCoinRestClientExchangeApi : IRestApiClient, IDisposable
    {
        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        /// <see cref="IDeepCoinRestClientExchangeApiAccount"/>
        public IDeepCoinRestClientExchangeApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        /// <see cref="IDeepCoinRestClientExchangeApiExchangeData"/>
        public IDeepCoinRestClientExchangeApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        /// <see cref="IDeepCoinRestClientExchangeApiTrading"/>
        public IDeepCoinRestClientExchangeApiTrading Trading { get; }

        /// <summary>
        /// Get the shared rest requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public IDeepCoinRestClientExchangeApiShared SharedClient { get; }
    }
}

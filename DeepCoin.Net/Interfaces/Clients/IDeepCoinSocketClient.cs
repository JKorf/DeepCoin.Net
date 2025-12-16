using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces.Clients;
using DeepCoin.Net.Interfaces.Clients.ExchangeApi;

namespace DeepCoin.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the DeepCoin websocket API
    /// </summary>
    public interface IDeepCoinSocketClient : ISocketClient
    {
        /// <summary>
        /// Exchange API endpoints
        /// </summary>
        /// <see cref="IDeepCoinSocketClientExchangeApi"/>
        public IDeepCoinSocketClientExchangeApi ExchangeApi { get; }

        /// <summary>
        /// Set the API credentials for this client. All Api clients in this client will use the new credentials, regardless of earlier set options.
        /// </summary>
        /// <param name="credentials">The credentials to set</param>
        void SetApiCredentials(ApiCredentials credentials);
    }
}

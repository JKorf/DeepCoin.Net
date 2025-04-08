using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects.Options;
using DeepCoin.Net.Interfaces.Clients.ExchangeApi;

namespace DeepCoin.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the DeepCoin Rest API. 
    /// </summary>
    public interface IDeepCoinRestClient : IRestClient
    {
        /// <summary>
        /// Exchange API endpoints
        /// </summary>
        /// <see cref="IDeepCoinRestClientExchangeApi"/>
        public IDeepCoinRestClientExchangeApi ExchangeApi { get; }

        /// <summary>
        /// Update specific options
        /// </summary>
        /// <param name="options">Options to update. Only specific options are changeable after the client has been created</param>
        void SetOptions(UpdateOptions options);

        /// <summary>
        /// Set the API credentials for this client. All Api clients in this client will use the new credentials, regardless of earlier set options.
        /// </summary>
        /// <param name="credentials">The credentials to set</param>
        void SetApiCredentials(ApiCredentials credentials);
    }
}

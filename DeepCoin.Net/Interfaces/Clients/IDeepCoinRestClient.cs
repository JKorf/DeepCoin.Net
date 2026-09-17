using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects.Options;
using DeepCoin.Net.Interfaces.Clients.ExchangeApi;
using DeepCoin.Net.Interfaces.Clients.V2Api;

namespace DeepCoin.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the DeepCoin Rest API. 
    /// </summary>
    public interface IDeepCoinRestClient : IRestClient<DeepCoinCredentials>
    {
        /// <summary>
        /// Exchange API endpoints
        /// </summary>
        /// <see cref="IDeepCoinRestClientExchangeApi"/>
        public IDeepCoinRestClientExchangeApi ExchangeApi { get; }

        /// <summary>
        /// Native V2 API endpoints with their own options and request contracts.
        /// </summary>
        IDeepCoinRestClientV2Api V2Api { get; }
    }
}

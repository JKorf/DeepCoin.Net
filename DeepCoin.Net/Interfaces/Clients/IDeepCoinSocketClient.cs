using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces.Clients;
using DeepCoin.Net.Interfaces.Clients.ExchangeApi;
using DeepCoin.Net.Interfaces.Clients.V2Api;

namespace DeepCoin.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the DeepCoin websocket API
    /// </summary>
    public interface IDeepCoinSocketClient : ISocketClient<DeepCoinCredentials>
    {
        /// <summary>
        /// Exchange API endpoints
        /// </summary>
        /// <see cref="IDeepCoinSocketClientExchangeApi"/>
        public IDeepCoinSocketClientExchangeApi ExchangeApi { get; }

        /// <summary>
        /// Native V2 public streams and private streams using V2 listen keys.
        /// </summary>
        public IDeepCoinSocketClientV2Api V2Api { get; }
    }
}

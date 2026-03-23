using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces.Clients;
using DeepCoin.Net.Interfaces.Clients.ExchangeApi;

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
    }
}

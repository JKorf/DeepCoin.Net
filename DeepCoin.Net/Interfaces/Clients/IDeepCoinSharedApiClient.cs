using DeepCoin.Net.Interfaces.Clients.ExchangeApi;

namespace DeepCoin.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for the shared REST and WebSocket API implementations of DeepCoin
    /// </summary>
    public interface IDeepCoinSharedApiClient
    {
        /// <summary>
        /// REST shared API implementations
        /// </summary>
        IDeepCoinRestClientExchangeSharedApi Rest { get; }

        /// <summary>
        /// WebSocket shared API implementations
        /// </summary>
        IDeepCoinSocketClientExchangeSharedApi Socket { get; }
    }
}

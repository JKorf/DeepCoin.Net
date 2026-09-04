using DeepCoin.Net.Interfaces.Clients;
using DeepCoin.Net.Interfaces.Clients.ExchangeApi;

namespace DeepCoin.Net.Clients
{
    /// <inheritdoc />
    public class DeepCoinSharedApiClient : IDeepCoinSharedApiClient
    {
        /// <inheritdoc />
        public IDeepCoinRestClientExchangeSharedApi Rest { get; }
        /// <inheritdoc />
        public IDeepCoinSocketClientExchangeSharedApi Socket { get; }

        /// <summary>
        /// ctor
        /// </summary>
        public DeepCoinSharedApiClient(
            IDeepCoinRestClient restClient,
            IDeepCoinSocketClient socketClient)
        {
            Rest = restClient.ExchangeApi.SharedApi;
            Socket = socketClient.ExchangeApi.SharedApi;
        }
    }
}

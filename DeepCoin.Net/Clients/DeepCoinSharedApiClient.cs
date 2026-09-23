using CryptoExchange.Net.SharedApis;
using DeepCoin.Net.Interfaces.Clients;
using DeepCoin.Net.Interfaces.Clients.ExchangeApi;
using DeepCoin.Net.Objects.Options;
using Microsoft.Extensions.Options;

namespace DeepCoin.Net.Clients
{
    /// <inheritdoc />
    public class DeepCoinSharedApiClient : SharedApiClientBase, IDeepCoinSharedApiClient
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
            IDeepCoinSocketClient socketClient,
            IOptions<DeepCoinOptions> options)
            : base(options.Value.SharedApi.PreferredTransport,
                    restClient.ExchangeApi.SharedApi,
                    socketClient.ExchangeApi.SharedApi
                  )
        {
            Rest = restClient.ExchangeApi.SharedApi;
            Socket = socketClient.ExchangeApi.SharedApi;
        }
    }
}

using CryptoExchange.Net.Interfaces.Clients;
using DeepCoin.Net.Clients;
using DeepCoin.Net.Interfaces.Clients;

namespace CryptoExchange.Net.Interfaces
{
    /// <summary>
    /// Extensions for the ICryptoRestClient and ICryptoSocketClient interfaces
    /// </summary>
    public static class CryptoClientExtensions
    {
        /// <summary>
        /// Get the DeepCoin REST Api client
        /// </summary>
        /// <param name="baseClient"></param>
        /// <returns></returns>
        public static IDeepCoinRestClient DeepCoin(this ICryptoRestClient baseClient) => baseClient.TryGet<IDeepCoinRestClient>(() => new DeepCoinRestClient());

        /// <summary>
        /// Get the DeepCoin Websocket Api client
        /// </summary>
        /// <param name="baseClient"></param>
        /// <returns></returns>
        public static IDeepCoinSocketClient DeepCoin(this ICryptoSocketClient baseClient) => baseClient.TryGet<IDeepCoinSocketClient>(() => new DeepCoinSocketClient());
    }
}

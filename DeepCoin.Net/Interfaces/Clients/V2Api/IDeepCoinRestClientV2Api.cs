using System;
using CryptoExchange.Net.Interfaces.Clients;

namespace DeepCoin.Net.Interfaces.Clients.V2Api
{
    /// <summary>
    /// Native DeepCoin V2 REST endpoints.
    /// </summary>
    public interface IDeepCoinRestClientV2Api : IRestApiClient<DeepCoinCredentials>, IDisposable
    {
        /// <summary>
        /// V2 account settings, balances and asset history.
        /// </summary>
        IDeepCoinRestClientV2ApiAccount Account { get; }
        /// <summary>
        /// V2 public market data.
        /// </summary>
        IDeepCoinRestClientV2ApiExchangeData ExchangeData { get; }
        /// <summary>
        /// V2 order, fill and position operations.
        /// </summary>
        IDeepCoinRestClientV2ApiTrading Trading { get; }
    }
}

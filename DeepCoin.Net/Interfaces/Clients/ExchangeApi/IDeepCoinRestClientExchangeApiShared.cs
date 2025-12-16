using CryptoExchange.Net.SharedApis;

namespace DeepCoin.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// Shared interface for Exchange rest API usage
    /// </summary>
    public interface IDeepCoinRestClientExchangeApiShared :
        IBalanceRestClient,
        IDepositRestClient,
        IKlineRestClient,
        IListenKeyRestClient,
        IOrderBookRestClient,
        IWithdrawalRestClient,
        ISpotTickerRestClient,
        ISpotSymbolRestClient,
        ISpotOrderRestClient,
        ILeverageRestClient,
        IFuturesTickerRestClient,
        IFuturesSymbolRestClient,
        IFuturesOrderRestClient,
        IBookTickerRestClient
    {
    }
}

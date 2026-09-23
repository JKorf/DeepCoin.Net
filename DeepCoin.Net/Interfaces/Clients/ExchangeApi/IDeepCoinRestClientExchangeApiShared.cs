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

    /// <summary>
    /// Shared API interface. Shared APIs provide a common,
    /// exchange-independent contract for accessing functionality across different
    /// exchange client libraries.
    /// </summary>
    public interface IDeepCoinRestClientExchangeSharedApi :
        IGetBalancesRest,
        IGetDepositHistoryRest,
        IGetKlinesRest,
        IGetOrderBookRest,
        IGetWithdrawalHistoryRest,
        IGetTickerRest,
        IGetAllTickersRest,
        IGetSpotSymbolsRest,
        IPlaceSpotOrderRest,
        IGetSpotOrderRest,
        IGetOpenSpotOrdersRest,
        IGetClosedSpotOrdersRest,
        ICancelSpotOrderRest,
        IGetSpotOrderTradesRest,
        IGetSpotUserTradeHistoryRest,
        IGetLeverageRest,
        ISetLeverageRest,
        IGetFuturesSymbolsRest,
        IPlaceFuturesOrderRest,
        IGetFuturesOrderRest,
        IGetOpenFuturesOrdersRest,
        IGetClosedFuturesOrdersRest,
        ICancelFuturesOrderRest,
        IGetFuturesOrderTradesRest,
        IGetFuturesUserTradeHistoryRest,
        IGetPositionsRest,
        IGetBookTickerRest
    { }
}

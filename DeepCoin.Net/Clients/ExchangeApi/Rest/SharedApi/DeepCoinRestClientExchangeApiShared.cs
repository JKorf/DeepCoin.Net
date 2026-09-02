using CryptoExchange.Net;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using DeepCoin.Net.Enums;
using DeepCoin.Net.Interfaces.Clients.ExchangeApi;
using DeepCoin.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeepCoin.Net.Clients.ExchangeApi
{
    internal partial class DeepCoinRestClientExchangeSharedApi : 
        SharedApiBase,
        IDeepCoinRestClientExchangeApiShared,
        IDeepCoinRestClientExchangeSharedApi
    {
        private readonly DeepCoinRestClientExchangeApi _api;

        private const string _topicSpotId = "DeepCoinSpot";
        private const string _topicFuturesId = "DeepCoinFutures";
        private const string _exchangeName = "DeepCoin";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(DeepCoinExchange.Metadata, this);

        public DeepCoinRestClientExchangeSharedApi(DeepCoinRestClientExchangeApi api)
            : base(
                  SharedTransport.Rest,
                  api.Exchange,
                  [TradingMode.Spot, TradingMode.PerpetualLinear, TradingMode.PerpetualInverse],
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                GetBalancesOptions,
                GetDepositHistoryOptions,
                GetKlinesOptions,
                GetOrderBookOptions,
                GetWithdrawalHistoryOptions,
                GetSpotTickerOptions,
                GetAllSpotTickersOptions,
                GetBookTickerOptions,
                GetSpotSymbolsOptions,
                PlaceSpotOrderOptions,
                GetSpotOrderOptions,
                GetOpenSpotOrdersOptions,
                GetClosedSpotOrdersOptions,
                GetSpotOrderTradesOptions,
                GetSpotUserTradeHistoryOptions,
                CancelSpotOrderOptions,
                GetLeverageOptions,
                SetLeverageOptions,
                GetFuturesTickerOptions,
                GetAllFuturesTickersOptions,
                GetFuturesSymbolsOptions,
                PlaceFuturesOrderOptions,
                GetFuturesOrderOptions,
                GetOpenFuturesOrdersOptions,
                GetClosedFuturesOrdersOptions,
                CancelFuturesOrderOptions,
                GetFuturesOrderTradesOptions,
                GetFuturesUserTradeHistoryOptions,
                GetPositionsOptions,
                ClosePositionOptions
                );
        }
    }
}

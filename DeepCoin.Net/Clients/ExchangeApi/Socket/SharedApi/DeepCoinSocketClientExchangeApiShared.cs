using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using DeepCoin.Net.Interfaces.Clients.ExchangeApi;
using System.Threading.Tasks;
using System.Threading;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Objects;
using System.Linq;
using DeepCoin.Net.Enums;
using CryptoExchange.Net;

namespace DeepCoin.Net.Clients.ExchangeApi
{
    internal partial class DeepCoinSocketClientExchangeSharedApi :
        SharedApiBase,
        IDeepCoinSocketClientExchangeApiShared,
        IDeepCoinSocketClientExchangeSharedApi
    {
        private readonly DeepCoinSocketClientExchangeApi _api;

        private const string _topicSpotId = "DeepCoinSpot";
        private const string _topicFuturesId = "DeepCoinFutures";
        private const string _exchangeName = "DeepCoin";
        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(DeepCoinExchange.Metadata, this);

        public DeepCoinSocketClientExchangeSharedApi(DeepCoinSocketClientExchangeApi api)
            : base(
                  SharedTransport.Socket,
                  api.Exchange,
                  [TradingMode.Spot, TradingMode.PerpetualLinear, TradingMode.PerpetualInverse],
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                SubscribeKlineOptions,
                SubscribeTickerOptions,
                SubscribeTradeOptions,
                SubscribeBalanceOptions,
                SubscribeFuturesOrderOptions,
                SubscribeSpotOrderOptions,
                SubscribeUserTradeOptions,
                SubscribePositionOptions
                );
        }
    }
}

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
    internal partial class DeepCoinSocketClientExchangeApi : IDeepCoinSocketClientExchangeApiShared
    {
        private const string _topicSpotId = "DeepCoinSpot";
        private const string _topicFuturesId = "DeepCoinFutures";
        private const string _exchangeName = "DeepCoin";

        public TradingMode[] SupportedTradingModes => new[] { TradingMode.Spot, TradingMode.PerpetualLinear, TradingMode.PerpetualInverse };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();
        public SharedClientInfo Discover() => SharedUtils.GetClientInfo(DeepCoinExchange.Metadata, this);

        #region Kline client
        SubscribeKlineOptions IKlineSocketClient.SubscribeKlineOptions { get; } = new SubscribeKlineOptions(_exchangeName, false, SharedKlineInterval.OneMinute);
        async Task<WebSocketResult<UpdateSubscription>> IKlineSocketClient.SubscribeToKlineUpdatesAsync(SubscribeKlineRequest request, Action<DataEvent<SharedKline>> handler, CancellationToken ct)
        {

            var validationError = SharedClient.SubscribeKlineOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbol = request.Symbol!.GetSymbol(DeepCoinExchange.FormatSymbol);
            var result = await SubscribeToKlineUpdatesAsync(symbol, update => handler(update.ToType(
                new SharedKline(request.Symbol, symbol, update.Data.OpenTime, update.Data.ClosePrice, update.Data.HighPrice, update.Data.LowPrice, update.Data.OpenPrice, update.Data.Volume))), ct).ConfigureAwait(false);

            return result;
        }
        #endregion

        #region Ticker client

        SubscribeTickerOptions ITickerSocketClient.SubscribeTickerOptions { get; } = new SubscribeTickerOptions(_exchangeName);
        async Task<WebSocketResult<UpdateSubscription>> ITickerSocketClient.SubscribeToTickerUpdatesAsync(SubscribeTickerRequest request, Action<DataEvent<SharedSpotTicker>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribeTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbol = request.Symbol!.GetSymbol(DeepCoinExchange.FormatSymbol);
            var result = await SubscribeToSymbolUpdatesAsync(symbol, update => handler(update.ToType(new SharedSpotTicker(ExchangeSymbolCache.ParseSymbol(_topicSpotId, EnvironmentName, null, symbol) ?? ExchangeSymbolCache.ParseSymbol(_topicFuturesId, EnvironmentName, null, symbol), symbol, update.Data.LastPrice, update.Data.HighPrice, update.Data.LowPrice, update.Data.Volume, update.Data.OpenPrice == null ? null : Math.Round((update.Data.LastPrice ?? 0) / update.Data.OpenPrice.Value * 100 - 100, 3))
            {
                // Value is incorrect for spot symbols
                QuoteVolume = request.Symbol.TradingMode == TradingMode.Spot ? null : update.Data.Turnover
            })), ct: ct).ConfigureAwait(false);

            return result;
        }

        #endregion

        #region Trade client

        SubscribeTradeOptions ITradeSocketClient.SubscribeTradeOptions { get; } = new SubscribeTradeOptions(_exchangeName, false);
        async Task<WebSocketResult<UpdateSubscription>> ITradeSocketClient.SubscribeToTradeUpdatesAsync(SubscribeTradeRequest request, Action<DataEvent<SharedTrade[]>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribeTradeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbol = request.Symbol!.GetSymbol(DeepCoinExchange.FormatSymbol);
            var result = await SubscribeToTradeUpdatesAsync(symbol, update => handler(update.ToType<SharedTrade[]>(new[] { 
                new SharedTrade(request.Symbol, symbol, update.Data.Quantity, update.Data.Price, update.Data.Timestamp)
            {
                Side = update.Data.Side == Enums.OrderSide.Sell ? SharedOrderSide.Sell : SharedOrderSide.Buy
            } })), ct: ct).ConfigureAwait(false);

            return result;
        }

        #endregion

        #region Balance client
        SubscribeBalanceOptions IBalanceSocketClient.SubscribeBalanceOptions { get; } = new SubscribeBalanceOptions(_exchangeName, true);
        async Task<WebSocketResult<UpdateSubscription>> IBalanceSocketClient.SubscribeToBalanceUpdatesAsync(SubscribeBalancesRequest request, Action<DataEvent<SharedBalance[]>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribeBalanceOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var result = await SubscribeToUserDataUpdatesAsync(
                onBalanceMessage: update => handler(update.ToType<SharedBalance[]>(update.Data.Select(x =>
                    new SharedBalance(
                        SupportedTradingModes, 
                        x.Asset, 
                        x.Available,
                        x.Balance)).ToArray())),
                ct: ct).ConfigureAwait(false);

            return result;
        }

        #endregion

        #region Futures Order client

        SubscribeFuturesOrderOptions IFuturesOrderSocketClient.SubscribeFuturesOrderOptions { get; } = new SubscribeFuturesOrderOptions(_exchangeName, true);
        async Task<WebSocketResult<UpdateSubscription>> IFuturesOrderSocketClient.SubscribeToFuturesOrderUpdatesAsync(SubscribeFuturesOrderRequest request, Action<DataEvent<SharedFuturesOrder[]>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribeFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var result = await SubscribeToUserDataUpdatesAsync(
                onOrderMessage: update =>
                {
                    var futuresOrders = update.Data.Where(x => !x.Symbol.Contains("/"));
                    if (!futuresOrders.Any())
                        return;

                    handler(update.ToType<SharedFuturesOrder[]>(futuresOrders.Select(x =>
                        new SharedFuturesOrder(
                            ExchangeSymbolCache.ParseSymbol(_topicFuturesId, EnvironmentName, null, x.Symbol),
                            x.Symbol,
                            x.OrderId,
                            ParseOrderType(x.OrderType),
                            x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            ParseOrderStatus(x.Status),
                            x.CreateTime)
                        {
                            AveragePrice = x.AverageFillPrice,
                            OrderPrice = x.OrderPrice,
                            TimeInForce = x.OrderType == OrderType.ImmediateOrCancel ? SharedTimeInForce.ImmediateOrCancel : null,
                            OrderQuantity = new SharedOrderQuantity(contractQuantity: x.Quantity),
                            QuantityFilled = new SharedOrderQuantity(quoteAssetQuantity: x.Turnover, contractQuantity: x.QuantityFilled),
                            UpdateTime = x.UpdateTime,
                            Leverage = x.Leverage,
                            PositionSide = x.PositionSide == PositionSide.Long ? SharedPositionSide.Long : SharedPositionSide.Short,
                        }
                    ).ToArray()));
                },
                ct: ct).ConfigureAwait(false);

            return result;
        }
        #endregion

        #region Spot Order client

        SubscribeSpotOrderOptions ISpotOrderSocketClient.SubscribeSpotOrderOptions { get; } = new SubscribeSpotOrderOptions(_exchangeName, true);
        async Task<WebSocketResult<UpdateSubscription>> ISpotOrderSocketClient.SubscribeToSpotOrderUpdatesAsync(SubscribeSpotOrderRequest request, Action<DataEvent<SharedSpotOrder[]>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribeSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var result = await SubscribeToUserDataUpdatesAsync(
                onOrderMessage: update =>
                {
                    var spotOrders = update.Data.Where(x => x.Symbol.Contains("/"));
                    if (!spotOrders.Any())
                        return;

                    handler(update.ToType<SharedSpotOrder[]>(spotOrders.Select(x =>
                        new SharedSpotOrder(
                            ExchangeSymbolCache.ParseSymbol(_topicSpotId, EnvironmentName, null, x.Symbol),
                            x.Symbol,
                            x.OrderId,
                            ParseOrderType(x.OrderType),
                            x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            ParseOrderStatus(x.Status),
                            x.CreateTime)
                        {
                            AveragePrice = x.AverageFillPrice,
                            OrderPrice = x.OrderPrice,
                            TimeInForce = x.OrderType == OrderType.ImmediateOrCancel ? SharedTimeInForce.ImmediateOrCancel : null,
                            OrderQuantity = new SharedOrderQuantity(x.Quantity),
                            QuantityFilled = new SharedOrderQuantity(x.QuantityFilled, x.Turnover),
                            UpdateTime = x.UpdateTime,
                            
                        }
                    ).ToArray()));
                },
                ct: ct).ConfigureAwait(false);

            return result;
        }
        #endregion

        #region Position client
        SubscribePositionOptions IPositionSocketClient.SubscribePositionOptions { get; } = new SubscribePositionOptions(_exchangeName, true);
        async Task<WebSocketResult<UpdateSubscription>> IPositionSocketClient.SubscribeToPositionUpdatesAsync(SubscribePositionRequest request, Action<DataEvent<SharedPosition[]>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribePositionOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var result = await SubscribeToUserDataUpdatesAsync(
                onPositionMessage: update => handler(update.ToType<SharedPosition[]>(update.Data.Select(x => new SharedPosition(ExchangeSymbolCache.ParseSymbol(_topicFuturesId, EnvironmentName, null, x.Symbol), x.Symbol, x.PositionSize, x.UpdateTime)
                {
                    AverageOpenPrice = x.OpenPrice,
                    PositionMode = SharedPositionMode.HedgeMode,
                    PositionSide = x.PositionSide == Enums.PositionSide.Short ? SharedPositionSide.Short : SharedPositionSide.Long,
                    Leverage = x.Leverage
                }).ToArray())),
                ct: ct).ConfigureAwait(false);

            return result;
        }

        #endregion

        #region User Trade client

        SubscribeUserTradeOptions IUserTradeSocketClient.SubscribeUserTradeOptions { get; } = new SubscribeUserTradeOptions(_exchangeName, true);
        async Task<WebSocketResult<UpdateSubscription>> IUserTradeSocketClient.SubscribeToUserTradeUpdatesAsync(SubscribeUserTradeRequest request, Action<DataEvent<SharedUserTrade[]>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribeUserTradeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var result = await SubscribeToUserDataUpdatesAsync(
                onUserTradeMessage: update => handler(update.ToType<SharedUserTrade[]>(update.Data.Select(x =>
                    new SharedUserTrade(
                        ExchangeSymbolCache.ParseSymbol(_topicFuturesId, EnvironmentName, null, x.Symbol) ?? ExchangeSymbolCache.ParseSymbol(_topicFuturesId, EnvironmentName, null, x.Symbol),
                        x.Symbol,
                        x.OrderId.ToString(),
                        x.TradeId.ToString(),
                        x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                        x.Quantity,
                        x.Price,
                        x.TradeTime)
                    {
                        Fee = x.Fee,
                        FeeAsset = x.FeeAsset,
                        Role = x.TradeRole == TradeRole.Maker ? SharedRole.Maker : SharedRole.Taker
                    }
                ).ToArray())),
                ct: ct).ConfigureAwait(false);

            return result;
        }
        #endregion

        private SharedOrderStatus ParseOrderStatus(OrderStatus status)
        {
            if (status == OrderStatus.Live || status == OrderStatus.PartiallyFilled) return SharedOrderStatus.Open;
            if (status == OrderStatus.Canceled) return SharedOrderStatus.Canceled;
            if (status == OrderStatus.Filled) return SharedOrderStatus.Filled;
            return SharedOrderStatus.Unknown;
        }

        private SharedOrderType ParseOrderType(OrderType type)
        {
            if (type == OrderType.Market) return SharedOrderType.Market;
            if (type == OrderType.PostOnly) return SharedOrderType.LimitMaker;
            return SharedOrderType.Limit;
        }

    }
}

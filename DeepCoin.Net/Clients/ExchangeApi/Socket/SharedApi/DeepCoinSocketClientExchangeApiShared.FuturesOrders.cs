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
    internal partial class DeepCoinSocketClientExchangeSharedApi
    {

        #region Subscribe To Futures Order Updates

        async Task<WebSocketResult<UpdateSubscription>> IFuturesOrderSocketClient.SubscribeToFuturesOrderUpdatesAsync(SubscribeFuturesOrderRequest request, Action<DataEvent<SharedFuturesOrder[]>> handler, CancellationToken ct)
            => await SubscribeToFuturesOrderUpdatesAsync(request, x => handler(x.ToType<SharedFuturesOrder[]>(x.Data)), ct).ConfigureAwait(false);

        public SubscribeFuturesOrderOptions SubscribeFuturesOrderOptions { get; } = new SubscribeFuturesOrderOptions(_exchangeName, true);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToFuturesOrderUpdatesAsync(SubscribeFuturesOrderRequest request, Action<DataEvent<SharedFuturesOrderUpdate[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var result = await _api.SubscribeToUserDataUpdatesAsync(
                onOrderMessage: update =>
                {
                    var futuresOrders = update.Data.Where(x => !x.Symbol.Contains("/"));
                    if (!futuresOrders.Any())
                        return;

                    handler(update.ToType<SharedFuturesOrderUpdate[]>(futuresOrders.Select(x =>
                        new SharedFuturesOrderUpdate(
                            ExchangeSymbolCache.ParseSymbol(_topicFuturesId, _api.EnvironmentName, null, x.Symbol),
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
    }
}

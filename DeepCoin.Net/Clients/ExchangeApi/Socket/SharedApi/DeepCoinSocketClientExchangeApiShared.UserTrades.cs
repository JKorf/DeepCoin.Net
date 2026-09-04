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

        public SubscribeUserTradeOptions SubscribeUserTradeOptions { get; } = new SubscribeUserTradeOptions(_exchangeName, true);
        #region Subscribe To User Trade Updates

        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(SubscribeUserTradeRequest request, Action<DataEvent<SharedUserTrade[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeUserTradeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var result = await _api.SubscribeToUserDataUpdatesAsync(
                onUserTradeMessage: update => handler(update.ToType<SharedUserTrade[]>(update.Data.Select(x =>
                    new SharedUserTrade(
                        ExchangeSymbolCache.ParseSymbol(_topicFuturesId, _api.EnvironmentName, null, x.Symbol) ?? ExchangeSymbolCache.ParseSymbol(_topicFuturesId, _api.EnvironmentName, null, x.Symbol),
                        x.Symbol,
                        x.OrderId.ToString(),
                        x.TradeId.ToString(),
                        x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                        new SharedOrderQuantity(
                            x.Symbol.Contains("SWAP") ? null : x.Quantity,
                            null,
                            x.Symbol.Contains("SWAP") ? x.Quantity : null),
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
    }
}

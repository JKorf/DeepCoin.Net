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

        public SubscribeTradeOptions SubscribeTradeOptions { get; } = new SubscribeTradeOptions(_exchangeName, false);
        #region Subscribe To Trade Updates

        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(SubscribeTradeRequest request, Action<DataEvent<SharedTrade[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeTradeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbol = request.Symbol!.GetSymbol(DeepCoinExchange.FormatSymbol);
            var result = await _api.SubscribeToTradeUpdatesAsync(symbol, update => handler(update.ToType<SharedTrade[]>(new[] {
                new SharedTrade(
                    request.Symbol,
                    symbol,
                    new SharedOrderQuantity(
                        request.TradingMode == TradingMode.Spot ? update.Data.Quantity : null,
                        null,
                        request.TradingMode != TradingMode.Spot ? update.Data.Quantity : null), 
                    update.Data.Price, 
                    update.Data.Timestamp)
            {
                Side = update.Data.Side == Enums.OrderSide.Sell ? SharedOrderSide.Sell : SharedOrderSide.Buy
            } })), ct: ct).ConfigureAwait(false);

            return result;
        }

        #endregion

    }
}

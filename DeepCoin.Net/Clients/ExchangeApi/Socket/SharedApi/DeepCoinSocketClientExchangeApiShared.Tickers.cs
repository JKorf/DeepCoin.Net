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
        #region Ticker client
        async Task<WebSocketResult<UpdateSubscription>> ISubscribeTickerSocket.SubscribeToTickerUpdatesAsync(SubscribeTickerRequest request, Action<DataEvent<SharedTicker>> handler, CancellationToken ct)
            => await SubscribeToTickerUpdatesAsync(request, x => handler(x.ToType<SharedTicker>(x.Data)), ct).ConfigureAwait(false);

        public SubscribeTickerOptions SubscribeTickerOptions { get; } = new SubscribeTickerOptions(_exchangeName);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(SubscribeTickerRequest request, Action<DataEvent<SharedSpotTicker>> handler, CancellationToken ct)
        {
            var validationError = SubscribeTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbol = request.Symbol!.GetSymbol(DeepCoinExchange.FormatSymbol);
            var result = await _api.SubscribeToSymbolUpdatesAsync(symbol, update => handler(update.ToType(
                new SharedSpotTicker(ExchangeSymbolCache.ParseSymbol(_topicSpotId, _api.EnvironmentName, null, symbol) ?? ExchangeSymbolCache.ParseSymbol(_topicFuturesId, _api.EnvironmentName, null, symbol),
                symbol,
                update.Data.LastPrice,
                update.Data.HighPrice,
                update.Data.LowPrice,
                new SharedOrderQuantity(null, request.TradingMode != TradingMode.Spot ? update.Data.Turnover : null, request.TradingMode != TradingMode.Spot ? update.Data.Volume : null),
                update.Data.OpenPrice == null ? null : Math.Round((update.Data.LastPrice ?? 0) / update.Data.OpenPrice.Value * 100 - 100, 3))
            {
                //// Value is incorrect for spot symbols
                //QuoteVolume = request.Symbol.TradingMode == TradingMode.Spot ? null : update.Data.Turnover
            })), ct: ct).ConfigureAwait(false);

            return result;
        }

        #endregion
    }
}

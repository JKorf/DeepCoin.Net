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
        public SubscribeKlineOptions SubscribeKlineOptions { get; } = new SubscribeKlineOptions(_exchangeName, false, SharedKlineInterval.OneMinute);
        #region Subscribe To Kline Updates

        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(SubscribeKlineRequest request, Action<DataEvent<SharedKline>> handler, CancellationToken ct)
        {

            var validationError = SubscribeKlineOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbol = request.Symbol!.GetSymbol(DeepCoinExchange.FormatSymbol);
            var result = await _api.SubscribeToKlineUpdatesAsync(symbol, update => handler(update.ToType(
                new SharedKline(
                    request.Symbol,
                    symbol, 
                    update.Data.OpenTime,
                    update.Data.ClosePrice,
                    update.Data.HighPrice,
                    update.Data.LowPrice, 
                    update.Data.OpenPrice,
                    new SharedOrderQuantity(update.Data.Volume, update.Data.Turnover)))), ct).ConfigureAwait(false);

            return result;
        }

        #endregion
    }
}

using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Guards;
using DeepCoin.Net.Enums;
using DeepCoin.Net.Interfaces.Clients.ExchangeApi;
using DeepCoin.Net.Objects.Models;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DeepCoin.Net.Clients.ExchangeApi
{
    /// <inheritdoc />
    internal class DeepCoinRestClientExchangeApiExchangeData : IDeepCoinRestClientExchangeApiExchangeData
    {
        private readonly DeepCoinRestClientExchangeApi _baseClient;
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        internal DeepCoinRestClientExchangeApiExchangeData(ILogger logger, DeepCoinRestClientExchangeApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Tickers
        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinTicker[]>> GetTickersAsync(SymbolType symbolType, string? underlying = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instType", symbolType);
            parameters.Add("uly", underlying);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "deepcoin/market/tickers", DeepCoinExchange.RateLimiter.DeepCoin, 1, false);
            return await _baseClient.SendAsync<DeepCoinTicker[]>(request, parameters, ct).ConfigureAwait(false);
        }
        #endregion

        #region Get Symbols

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinSymbol[]>> GetSymbolsAsync(SymbolType type, string? underlying = null, string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instType", type);
            parameters.Add("uly", underlying);
            parameters.Add("instId", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/market/instruments", DeepCoinExchange.RateLimiter.DeepCoin, 1, false, limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinSymbol[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinKline[]>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("bar", interval);
            parameters.Add("after", endTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "deepcoin/market/candles", DeepCoinExchange.RateLimiter.DeepCoin, 1, false, limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinKline[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinOrderBook>> GetOrderBookAsync(string symbol, int? depth = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("sz", depth ?? 20);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "deepcoin/market/books", DeepCoinExchange.RateLimiter.DeepCoin, 1, false, limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinOrderBook>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Funding Rate

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinFundingRate[]>> GetFundingRateAsync(ProductGroup contractType, string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instType", contractType);
            parameters.Add("instId", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/trade/funding-rate", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinFundingRate[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Mark Price

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinMarkPrice[]>> GetMarkPricesAsync(
            SymbolType symbolType,
            string? underlying = null,
            string? symbol = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instType", symbolType);
            parameters.Add("uly", underlying);
            parameters.Add("instId", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/market/mark-price", DeepCoinExchange.RateLimiter.DeepCoin, 1, false, limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinMarkPrice[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Open Interest And Volume

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinOpenInterest[]>> GetOpenInterestAndVolumeAsync(
            string symbol,
            DataInterval? interval = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("bar", interval);
            parameters.Add("startTime", startTime);
            parameters.Add("endTime", endTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/market/open-interest-volume", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinOpenInterest[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Long Short Ratio

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinLongShortRatio[]>> GetLongShortRatioAsync(
            string symbol,
            DataInterval? interval = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("bar", interval);
            parameters.Add("startTime", startTime);
            parameters.Add("endTime", endTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/market/long-short-ratio", DeepCoinExchange.RateLimiter.DeepCoin, 1, false, limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinLongShortRatio[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Taker Buy Sell Volume

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinTakerBuySellVolume[]>> GetTakerBuySellVolumeAsync(
            string symbol,
            DataInterval? interval = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("bar", interval);
            parameters.Add("startTime", startTime);
            parameters.Add("endTime", endTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/market/taker-volume", DeepCoinExchange.RateLimiter.DeepCoin, 1, false, limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinTakerBuySellVolume[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}

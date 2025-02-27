using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using DeepCoin.Net.Interfaces.Clients.ExchangeApi;
using DeepCoin.Net.Objects.Models;
using DeepCoin.Net.Enums;
using CryptoExchange.Net.RateLimiting.Guards;

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
        public async Task<WebCallResult<DeepCoinTicker[]>> GetTickersAsync(SymbolType symbolType, string? underlying = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddEnum("instType", symbolType);
            parameters.AddOptional("uly", underlying);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "deepcoin/market/tickers", DeepCoinExchange.RateLimiter.DeepCoin, 1, false);
            return await _baseClient.SendAsync<DeepCoinTicker[]>(request, parameters, ct).ConfigureAwait(false);
        }
        #endregion

        #region Get Symbols

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<DeepCoinSymbol>>> GetSymbolsAsync(SymbolType type, string? underlying = null, string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddEnum("instType", type);
            parameters.AddOptional("uly", underlying);
            parameters.AddOptional("instId", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/deepcoin/market/instruments", DeepCoinExchange.RateLimiter.DeepCoin, 1, false, limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<IEnumerable<DeepCoinSymbol>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<DeepCoinKline>>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            parameters.AddEnum("bar", interval);
            parameters.AddOptionalMilliseconds("after", endTime);
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "deepcoin/market/candles", DeepCoinExchange.RateLimiter.DeepCoin, 1, false, limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<IEnumerable<DeepCoinKline>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<WebCallResult<DeepCoinOrderBook>> GetOrderBookAsync(string symbol, int? depth = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            parameters.Add("sz", depth ?? 20);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "deepcoin/market/books", DeepCoinExchange.RateLimiter.DeepCoin, 1, false, limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinOrderBook>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Funding Rate

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<DeepCoinFundingRate>>> GetFundingRateAsync(ProductGroup contractType, string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddEnum("instType", contractType);
            parameters.AddOptional("instId", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/deepcoin/trade/funding-rate", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<IEnumerable<DeepCoinFundingRate>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}

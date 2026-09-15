using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Guards;
using DeepCoin.Net.Enums;
using DeepCoin.Net.Interfaces.Clients.V2Api;
using DeepCoin.Net.Objects.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DeepCoin.Net.Clients.V2Api
{
    /// <inheritdoc />
    internal class DeepCoinRestClientV2ApiExchangeData : IDeepCoinRestClientV2ApiExchangeData
    {
        private readonly DeepCoinRestClientV2Api _baseClient;
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        internal DeepCoinRestClientV2ApiExchangeData(ILogger logger, DeepCoinRestClientV2Api baseClient)
        {
            _baseClient = baseClient;
        }

        /// <inheritdoc />
        public async Task<HttpResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/v2/market/time", DeepCoinExchange.RateLimiter.RestMarket, 1, false, limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinV2ServerTime>(request, null, ct).ConfigureAwait(false);
            return result.Success ? HttpResult.Ok(result, result.Data.Timestamp) : HttpResult.Fail<DateTime>(result);
        }

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinSymbol[]>> GetSymbolsAsync(SymbolType type, string? underlying = null, string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instType", type);
            parameters.Add("uly", underlying);
            parameters.Add("instId", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/v2/market/instruments", DeepCoinExchange.RateLimiter.RestMarket, 1, false, limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            return await _baseClient.SendAsync<DeepCoinSymbol[]>(request, parameters, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinTicker[]>> GetTickersAsync(SymbolType symbolType, string? underlying = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instType", symbolType);
            parameters.Add("uly", underlying);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/v2/market/tickers", DeepCoinExchange.RateLimiter.RestMarket, 1, false, limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            return await _baseClient.SendAsync<DeepCoinTicker[]>(request, parameters, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinKline[]>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            if (limit is < 1 or > 300)
                throw new ArgumentOutOfRangeException(nameof(limit), "V2 candlesticks support between 1 and 300 results.");
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("bar", interval);
            parameters.Add("endTime", endTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/v2/market/candles", DeepCoinExchange.RateLimiter.RestMarket, 1, false, limitGuard: new SingleLimitGuard(50, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            return await _baseClient.SendAsync<DeepCoinKline[]>(request, parameters, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinOrderBook>> GetOrderBookAsync(string symbol, int? depth = null, CancellationToken ct = default)
        {
            if (depth is < 1 or > 400)
                throw new ArgumentOutOfRangeException(nameof(depth), "V2 order books support between 1 and 400 price levels.");
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("limit", depth ?? 20);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/v2/market/books", DeepCoinExchange.RateLimiter.RestMarket, 1, false, limitGuard: new SingleLimitGuard(50, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            return await _baseClient.SendAsync<DeepCoinOrderBook>(request, parameters, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinV2FundingRate[]>> GetFundingRatesAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/v2/market/funding-rate", DeepCoinExchange.RateLimiter.RestMarket, 1, false, limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            return await _baseClient.SendAsync<DeepCoinV2FundingRate[]>(request, parameters, ct).ConfigureAwait(false);
        }
    }
}

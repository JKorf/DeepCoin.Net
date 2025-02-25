using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using DeepCoin.Net.Interfaces.Clients.ExchangeApi;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System;
using DeepCoin.Net.Enums;
using DeepCoin.Net.Objects.Models;
using CryptoExchange.Net.RateLimiting.Guards;

namespace DeepCoin.Net.Clients.ExchangeApi
{
    /// <inheritdoc />
    internal class DeepCoinRestClientExchangeApiTrading : IDeepCoinRestClientExchangeApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly DeepCoinRestClientExchangeApi _baseClient;
        private readonly ILogger _logger;

        internal DeepCoinRestClientExchangeApiTrading(ILogger logger, DeepCoinRestClientExchangeApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;
        }

        #region Get Positions

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<DeepCoinPosition>>> GetPositionsAsync(SymbolType symbolType, string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddEnum("instType", symbolType);
            parameters.AddOptional("instId", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/deepcoin/account/positions", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<IEnumerable<DeepCoinPosition>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}

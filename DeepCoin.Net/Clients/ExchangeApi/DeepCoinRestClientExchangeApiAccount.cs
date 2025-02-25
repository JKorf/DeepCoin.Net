using CryptoExchange.Net.Objects;
using DeepCoin.Net.Clients.ExchangeApi;
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
    internal class DeepCoinRestClientExchangeApiAccount : IDeepCoinRestClientExchangeApiAccount
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly DeepCoinRestClientExchangeApi _baseClient;

        internal DeepCoinRestClientExchangeApiAccount(DeepCoinRestClientExchangeApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Balances

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<DeepCoinBalance>>> GetBalancesAsync(SymbolType accountType, string? asset = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddEnum("instType", accountType);
            parameters.AddOptional("ccy", asset);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "deepcoin/account/balances", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<IEnumerable<DeepCoinBalance>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Bills

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<DeepCoinBill>>> GetBillsAsync(SymbolType symbolType, string? asset = null, BillType? billType = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddEnum("instType", symbolType);
            parameters.AddOptional("ccy", asset);
            parameters.AddOptionalEnum("type", billType);
            parameters.AddOptionalMillisecondsString("after", startTime);
            parameters.AddOptionalMillisecondsString("before", endTime);
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/deepcoin/account/bills", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<IEnumerable<DeepCoinBill>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Set Leverage

        /// <inheritdoc />
        public async Task<WebCallResult<DeepCoinLeverage>> SetLeverageAsync(string symbol, decimal leverage, MarginMode marginMode, PositionType positionType, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            parameters.Add("lever", leverage);
            parameters.AddEnum("mgnMode", marginMode);
            parameters.AddEnum("mrgPosition", positionType);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/deepcoin/account/set-leverage", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinLeverage>(request, parameters, ct).ConfigureAwait(false);
            if (result.Data.ResultCode != 0)
                return result.AsError<DeepCoinLeverage>(new ServerError(result.Data.ResultCode, result.Data.ResultMessage!));

            return result;
        }

        #endregion

    }
}

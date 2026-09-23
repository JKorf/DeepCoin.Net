using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Guards;
using DeepCoin.Net.Enums;
using DeepCoin.Net.Objects.Models;
using DeepCoin.Net.Interfaces.Clients.V2Api;

namespace DeepCoin.Net.Clients.V2Api
{
    /// <inheritdoc />
    internal class DeepCoinRestClientV2ApiAccount : IDeepCoinRestClientV2ApiAccount
    {
        #region Fields
        private static readonly RequestDefinitionCache _definitions = new();
        private readonly DeepCoinRestClientV2Api _baseClient;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the native V2 account methods.
        /// </summary>
        internal DeepCoinRestClientV2ApiAccount(DeepCoinRestClientV2Api baseClient) => _baseClient = baseClient;
        #endregion

        #region Methods
        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinUserId>> GetUserIdAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/v2/account/uid", DeepCoinExchange.RateLimiter.RestHistory, 1, true, limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            return await _baseClient.SendAsync<DeepCoinUserId>(request, parameters, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinV2TransferResult>> TransferAsync(string asset, decimal quantity, long userId, TransferAccountType fromAccount, TransferAccountType toAccount, string? clientId = null, CancellationToken ct = default)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "Transfer quantity must be positive.");
            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId), "The authenticated owner's UID is required.");
            if (!Enum.IsDefined(typeof(TransferAccountType), fromAccount))
                throw new ArgumentOutOfRangeException(nameof(fromAccount));
            if (!Enum.IsDefined(typeof(TransferAccountType), toAccount))
                throw new ArgumentOutOfRangeException(nameof(toAccount));
            if (fromAccount == toAccount)
                throw new ArgumentException("Transfer wallets must be distinct.", nameof(toAccount));

            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("type", "internal");
            parameters.Add("ccy", asset);
            parameters.Add("amt", quantity);
            parameters.Add("clientId", clientId);
            parameters.Add("internal", new Dictionary<string, object>
            {
                { "uid", userId },
                { "fromAcctType", (int)fromAccount },
                { "toAcctType", (int)toAccount }
            });
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/deepcoin/v2/asset/unified-transfer", DeepCoinExchange.RateLimiter.RestHistory, 1, true, limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            return await _baseClient.SendAsync<DeepCoinV2TransferResult>(request, parameters, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinAllBalances>> GetAllBalancesAsync(IEnumerable<BalanceType>? accountTypes = null, IEnumerable<string>? assets = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.AddCommaSeparated("accountType", accountTypes);
            parameters.AddCommaSeparated("ccy", assets);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/v2/account/all-balances", DeepCoinExchange.RateLimiter.RestAccount, 1, true, limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinAllBalances>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinFeeRate[]>> GetTradeFeeAsync(SymbolType symbolType, string? symbol = null, string? symbolFamily = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instType", symbolType);
            parameters.Add("instId", symbol);
            parameters.Add("instFamily", symbolFamily);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/v2/account/trade-fee", DeepCoinExchange.RateLimiter.RestAccount, 1, true, limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinFeeRate[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinV2Bill[]>> GetBillsAsync(SymbolType symbolType, string? asset = null, BillType? billType = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instType", symbolType);
            parameters.Add("ccy", asset);
            parameters.Add("type", billType);
            parameters.Add("startTime", startTime);
            parameters.Add("endTime", endTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/v2/account/bills", DeepCoinExchange.RateLimiter.RestHistory, 1, true, limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinV2Bill[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinV2Leverage>> SetLeverageAsync(string symbol, decimal leverage, TradeMode tradeMode, PositionType positionType, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            if (leverage < 1 || leverage > int.MaxValue || decimal.Truncate(leverage) != leverage)
                throw new ArgumentOutOfRangeException(nameof(leverage), "DeepCoin V2 leverage must be a positive integer.");
            parameters.Add("instId", symbol);
            parameters.Add("lever", (int)leverage);
            parameters.Add("mgnMode", tradeMode);
            parameters.Add("mrgPosition", positionType);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/deepcoin/v2/account/set-leverage", DeepCoinExchange.RateLimiter.RestHistory, 1, true, limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinV2Leverage>(request, parameters, ct).ConfigureAwait(false);
            if (result.Success && result.Data.ResultCode != 0)
                return HttpResult.Fail<DeepCoinV2Leverage>(result, new ServerError(result.Data.ResultCode ?? -1, _baseClient.GetErrorInfo(result.Data.ResultCode ?? -1, result.Data.ResultMessage ?? "V2 leverage acknowledgement omitted its execution code.")));
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinV2DepositPage>> GetDepositHistoryAsync(string? asset = null, string? transactionHash = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("ccy", asset);
            parameters.Add("txHash", transactionHash);
            parameters.Add("startTime", startTime, DateTimeSerialization.SecondsNumber);
            parameters.Add("endTime", endTime, DateTimeSerialization.SecondsNumber);
            parameters.Add("page", page);
            parameters.Add("limit", pageSize);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/v2/asset/deposit-list", DeepCoinExchange.RateLimiter.RestHistory, 1, true, limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinV2DepositPage>(request, parameters, ct).ConfigureAwait(false);
            if (result.Success && result.Data.Data == null)
                result.Data.Data = [];
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinV2WithdrawPage>> GetWithdrawHistoryAsync(string? asset = null, string? transactionHash = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("ccy", asset);
            parameters.Add("txId", transactionHash);
            parameters.Add("startTime", startTime);
            parameters.Add("endTime", endTime);
            parameters.Add("page", page);
            parameters.Add("size", pageSize);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/v2/asset/withdraw-list", DeepCoinExchange.RateLimiter.RestHistory, 1, true, limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinV2WithdrawPage>(request, parameters, ct).ConfigureAwait(false);
            if (result.Success && result.Data.Data == null)
                result.Data.Data = [];
            // The live V2 empty-history response is count=0,data=[{}]. Normalize only that exact
            // empty record; populated records with missing/unknown statuses must remain visible.
            if (result.Success && result.Data.Total == 0 && result.Data.Data.Length == 1 && result.Data.Data[0] == new DeepCoinV2Withdrawal())
                result.Data.Data = [];
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinListenKey>> StartUserStreamAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/v2/listenkey/acquire", DeepCoinExchange.RateLimiter.RestHistory, 1, true, limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinListenKey>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinListenKey>> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("listenkey", listenKey);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/v2/listenkey/extend", DeepCoinExchange.RateLimiter.RestHistory, 1, true, limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinListenKey>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }
        #endregion
    }
}

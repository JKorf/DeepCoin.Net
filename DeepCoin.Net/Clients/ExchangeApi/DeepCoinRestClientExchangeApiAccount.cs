using CryptoExchange.Net.Objects;
using DeepCoin.Net.Interfaces.Clients.ExchangeApi;
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
        public async Task<HttpResult<DeepCoinBalance[]>> GetBalancesAsync(SymbolType accountType, string? asset = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instType", accountType);
            parameters.Add("ccy", asset);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "deepcoin/account/balances", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinBalance[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Bills

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinBill[]>> GetBillsAsync(SymbolType symbolType, string? asset = null, BillType? billType = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instType", symbolType);
            parameters.Add("ccy", asset);
            parameters.Add("type", billType);
            parameters.Add("after", startTime);
            parameters.Add("before", endTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/account/bills", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinBill[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Set Leverage

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinLeverage>> SetLeverageAsync(string symbol, decimal leverage, TradeMode tradeMode, PositionType positionType, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("lever", leverage);
            parameters.Add("mgnMode", tradeMode);
            parameters.Add("mrgPosition", positionType);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/deepcoin/account/set-leverage", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinLeverage>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<DeepCoinLeverage>(result);

            if (result.Data.ResultCode != 0)
                return HttpResult.Fail<DeepCoinLeverage>(result, new ServerError(result.Data.ResultCode, _baseClient.GetErrorInfo(result.Data.ResultCode, result.Data.ResultMessage!)));

            return result;
        }

        #endregion

        // Transfer endpoints currently not useable
        //#region Get Transferable Assets

        ///// <inheritdoc />
        //public async Task<HttpResult<DeepCoinTransferableAsset>> GetTransferableAssetsAsync(CancellationToken ct = default)
        //{
        //    var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
        //    var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/internal-transfer/support", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
        //    var result = await _baseClient.SendAsync<DeepCoinTransferableAsset>(request, parameters, ct).ConfigureAwait(false);
        //    return result;
        //}

        //#endregion

        //#region Transfer

        ///// <inheritdoc />
        //public async Task<HttpResult<DeepCoinTransferResult>> TransferAsync(string asset, decimal quantity, string toAccount, AccountType toAccountType, string? clientOrderId = null, CancellationToken ct = default)
        //{
        //    var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
        //    parameters.Add("coin", asset);
        //    parameters.Add("amount", quantity);
        //    parameters.Add("receiverAccount", toAccount);
        //    parameters.Add("accountType", toAccountType);
        //    parameters.Add("receiverUID", clientOrderId);
        //    var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/deepcoin/internal-transfer", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
        //    var result = await _baseClient.SendAsync<DeepCoinTransferResult>(request, parameters, ct).ConfigureAwait(false);
        //    return result;
        //}

        //#endregion

        //#region Get Transfer History

        ///// <inheritdoc />
        //public async Task<HttpResult<DeepCoinTransferPage>> GetTransferHistoryAsync(string? toAccount = null, string? asset = null, TransferStatus? status = null, string? receiverId = null, string? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
        //{
        //    var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
        //    parameters.Add("account", toAccount);
        //    parameters.Add("coin", asset);
        //    parameters.Add("status", status);
        //    parameters.Add("receiverUID", receiverId);
        //    parameters.Add("orderId", orderId);
        //    parameters.Add("startTime", startTime);
        //    parameters.Add("endTime", endTime);
        //    parameters.Add("page", page);
        //    parameters.Add("size", pageSize);
        //    var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/internal-transfer/history-order", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
        //    var result = await _baseClient.SendAsync<DeepCoinTransferPage>(request, parameters, ct).ConfigureAwait(false);
        //    return result;
        //}

        //#endregion

        #region Get Deposit History

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinDepositPage>> GetDepositHistoryAsync(string? asset = null, string? transactionHash = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("coin", asset);
            parameters.Add("txHash", transactionHash);
            parameters.Add("startTime", startTime, DateTimeSerialization.SecondsString);
            parameters.Add("endTime", endTime, DateTimeSerialization.SecondsString);
            parameters.Add("page", page);
            parameters.Add("size", pageSize);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/asset/deposit-list", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinDepositPage>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return result;

            if (result.Data.Data == null)
                result.Data.Data = [];
            
            return result;
        }

        #endregion

        #region Get Withdraw History

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinWithdrawPage>> GetWithdrawHistoryAsync(string? asset = null, string? transactionHash = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("coin", asset);
            parameters.Add("txHash", transactionHash);
            parameters.Add("startTime", startTime, DateTimeSerialization.SecondsString);
            parameters.Add("endTime", endTime, DateTimeSerialization.SecondsString);
            parameters.Add("page", page);
            parameters.Add("size", pageSize);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/asset/withdraw-list", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinWithdrawPage>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return result;

            if (result.Data.Data == null)
                result.Data.Data = [];

            return result;
        }

        #endregion

        #region Start User Stream 

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinListenKey>> StartUserStreamAsync(CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/listenkey/acquire", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinListenKey>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Keep Alive User Stream 

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinListenKey>> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("listenkey", listenKey);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/listenkey/extend", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinListenKey>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Trade Fee

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinFeeRate[]>> GetTradeFeeAsync(
            SymbolType symbolType,
            string? symbol = null,
            string? symbolFamily = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instType", symbolType);
            parameters.Add("instId", symbol);
            parameters.Add("instFamily", symbolFamily);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/account/trade-fee", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinFeeRate[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion
    }
}

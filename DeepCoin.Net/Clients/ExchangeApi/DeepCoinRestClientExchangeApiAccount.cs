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
        public async Task<WebCallResult<DeepCoinBalance[]>> GetBalancesAsync(SymbolType accountType, string? asset = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddEnum("instType", accountType);
            parameters.AddOptional("ccy", asset);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "deepcoin/account/balances", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinBalance[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Bills

        /// <inheritdoc />
        public async Task<WebCallResult<DeepCoinBill[]>> GetBillsAsync(SymbolType symbolType, string? asset = null, BillType? billType = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddEnum("instType", symbolType);
            parameters.AddOptional("ccy", asset);
            parameters.AddOptionalEnum("type", billType);
            parameters.AddOptionalMillisecondsString("after", startTime);
            parameters.AddOptionalMillisecondsString("before", endTime);
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/deepcoin/account/bills", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinBill[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Set Leverage

        /// <inheritdoc />
        public async Task<WebCallResult<DeepCoinLeverage>> SetLeverageAsync(string symbol, decimal leverage, TradeMode tradeMode, PositionType positionType, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            parameters.Add("lever", leverage);
            parameters.AddEnum("mgnMode", tradeMode);
            parameters.AddEnum("mrgPosition", positionType);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/deepcoin/account/set-leverage", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinLeverage>(request, parameters, ct).ConfigureAwait(false);
            if (result.Data.ResultCode != 0)
                return result.AsError<DeepCoinLeverage>(new ServerError(result.Data.ResultCode, result.Data.ResultMessage!));

            return result;
        }

        #endregion

        // Transfer endpoints currently not useable
        //#region Get Transferable Assets

        ///// <inheritdoc />
        //public async Task<WebCallResult<DeepCoinTransferableAsset>> GetTransferableAssetsAsync(CancellationToken ct = default)
        //{
        //    var parameters = new ParameterCollection();
        //    var request = _definitions.GetOrCreate(HttpMethod.Get, "/deepcoin/internal-transfer/support", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
        //    var result = await _baseClient.SendAsync<DeepCoinTransferableAsset>(request, parameters, ct).ConfigureAwait(false);
        //    return result;
        //}

        //#endregion

        //#region Transfer

        ///// <inheritdoc />
        //public async Task<WebCallResult<DeepCoinTransferResult>> TransferAsync(string asset, decimal quantity, string toAccount, AccountType toAccountType, string? clientOrderId = null, CancellationToken ct = default)
        //{
        //    var parameters = new ParameterCollection();
        //    parameters.Add("coin", asset);
        //    parameters.AddString("amount", quantity);
        //    parameters.Add("receiverAccount", toAccount);
        //    parameters.AddEnum("accountType", toAccountType);
        //    parameters.AddOptional("receiverUID", clientOrderId);
        //    var request = _definitions.GetOrCreate(HttpMethod.Post, "/deepcoin/internal-transfer", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
        //    var result = await _baseClient.SendAsync<DeepCoinTransferResult>(request, parameters, ct).ConfigureAwait(false);
        //    return result;
        //}

        //#endregion

        //#region Get Transfer History

        ///// <inheritdoc />
        //public async Task<WebCallResult<DeepCoinTransferPage>> GetTransferHistoryAsync(string? toAccount = null, string? asset = null, TransferStatus? status = null, string? receiverId = null, string? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
        //{
        //    var parameters = new ParameterCollection();
        //    parameters.AddOptional("account", toAccount);
        //    parameters.AddOptional("coin", asset);
        //    parameters.AddOptionalEnum("status", status);
        //    parameters.AddOptional("receiverUID", receiverId);
        //    parameters.AddOptional("orderId", orderId);
        //    parameters.AddOptionalMillisecondsString("startTime", startTime);
        //    parameters.AddOptionalMillisecondsString("endTime", endTime);
        //    parameters.AddOptional("page", page);
        //    parameters.AddOptional("size", pageSize);
        //    var request = _definitions.GetOrCreate(HttpMethod.Get, "/deepcoin/internal-transfer/history-order", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
        //    var result = await _baseClient.SendAsync<DeepCoinTransferPage>(request, parameters, ct).ConfigureAwait(false);
        //    return result;
        //}

        //#endregion

        #region Get Deposit History

        /// <inheritdoc />
        public async Task<WebCallResult<DeepCoinDepositPage>> GetDepositHistoryAsync(string? asset = null, string? transactionHash = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("coin", asset);
            parameters.AddOptional("txHash", transactionHash);
            parameters.AddOptionalMillisecondsString("startTime", startTime);
            parameters.AddOptionalMillisecondsString("endTime", endTime);
            parameters.AddOptional("page", page);
            parameters.AddOptional("size", pageSize);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/deepcoin/asset/deposit-list", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinDepositPage>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result;

            if (result.Data.Data == null)
                result.Data.Data = [];
            
            return result;
        }

        #endregion

        #region Get Withdraw History

        /// <inheritdoc />
        public async Task<WebCallResult<DeepCoinWithdrawPage>> GetWithdrawHistoryAsync(string? asset = null, string? transactionHash = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("coin", asset);
            parameters.AddOptional("txHash", transactionHash);
            parameters.AddOptionalMillisecondsString("startTime", startTime);
            parameters.AddOptionalMillisecondsString("endTime", endTime);
            parameters.AddOptional("page", page);
            parameters.AddOptional("size", pageSize);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/deepcoin/asset/withdraw-list", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinWithdrawPage>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result;

            if (result.Data.Data == null)
                result.Data.Data = [];

            return result;
        }

        #endregion

        #region Start User Stream 

        /// <inheritdoc />
        public async Task<WebCallResult<DeepCoinListenKey>> StartUserStreamAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/deepcoin/listenkey/acquire", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinListenKey>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Keep Alive User Stream 

        /// <inheritdoc />
        public async Task<WebCallResult<DeepCoinListenKey>> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("listenkey", listenKey);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/deepcoin/listenkey/extend", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinListenKey>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion
    }
}

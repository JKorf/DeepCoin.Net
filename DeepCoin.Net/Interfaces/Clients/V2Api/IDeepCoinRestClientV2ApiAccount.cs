using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using DeepCoin.Net.Enums;
using DeepCoin.Net.Objects.Models;

namespace DeepCoin.Net.Interfaces.Clients.V2Api
{
    /// <summary>
    /// V2 account endpoints used by trading clients and private subscriptions.
    /// </summary>
    public interface IDeepCoinRestClientV2ApiAccount
    {
        /// <summary>
        /// Gets the UID bound to the authenticated API key.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/DeepCoinAccount/getUID" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/v2/account/uid
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        Task<HttpResult<DeepCoinUserId>> GetUserIdAsync(CancellationToken ct = default);

        /// <summary>
        /// Transfers asset units between physical wallets belonging to the API key owner.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/assets/unified-transfer" /><br />
        /// Endpoint:<br />
        /// POST /deepcoin/v2/asset/unified-transfer
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>ccy</c>"] Native currency code.</param>
        /// <param name="quantity">["<c>amt</c>"] Positive amount in asset units.</param>
        /// <param name="userId">["<c>uid</c>"] UID bound to the authenticated API key, in the internal object.</param>
        /// <param name="fromAccount">["<c>fromAcctType</c>"] Source physical wallet, in the internal object.</param>
        /// <param name="toAccount">["<c>toAcctType</c>"] Destination physical wallet, in the internal object.</param>
        /// <param name="clientId">["<c>clientId</c>"] Optional client idempotency identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<HttpResult<DeepCoinV2TransferResult>> TransferAsync(string asset, decimal quantity, long userId, TransferAccountType fromAccount, TransferAccountType toAccount, string? clientId = null, CancellationToken ct = default);

        /// <summary>
        /// Gets balances with explicit physical wallet identities.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/DeepCoinAccount/getAllAccountBalances" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/v2/account/all-balances
        /// </para>
        /// </summary>
        Task<HttpResult<DeepCoinAllBalances>> GetAllBalancesAsync(IEnumerable<BalanceType>? accountTypes = null, IEnumerable<string>? assets = null, CancellationToken ct = default);

        /// <summary>
        /// Gets account-specific maker and taker fee fractions.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/DeepCoinAccount/getTradeFee" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/v2/account/trade-fee
        /// </para>
        /// </summary>
        Task<HttpResult<DeepCoinFeeRate[]>> GetTradeFeeAsync(SymbolType symbolType, string? symbol = null, string? symbolFamily = null, CancellationToken ct = default);

        /// <summary>
        /// Gets account bills between Unix millisecond timestamps.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/DeepCoinAccount/getAccountBills" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/v2/account/bills
        /// </para>
        /// </summary>
        Task<HttpResult<DeepCoinV2Bill[]>> GetBillsAsync(SymbolType symbolType, string? asset = null, BillType? billType = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Sets integer leverage for an explicit margin and position mode.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/DeepCoinAccount/accountSetLeverage" /><br />
        /// Endpoint:<br />
        /// POST /deepcoin/v2/account/set-leverage
        /// </para>
        /// </summary>
        Task<HttpResult<DeepCoinV2Leverage>> SetLeverageAsync(string symbol, decimal leverage, TradeMode tradeMode, PositionType positionType, CancellationToken ct = default);

        /// <summary>
        /// Gets deposit history using the V2 currency and limit parameters.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/assets/deposit" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/v2/asset/deposit-list
        /// </para>
        /// </summary>
        Task<HttpResult<DeepCoinV2DepositPage>> GetDepositHistoryAsync(string? asset = null, string? transactionHash = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default);

        /// <summary>
        /// Gets V2 withdrawal history with canonical amounts, fees and status.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/assets/withdraw" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/v2/asset/withdraw-list
        /// </para>
        /// </summary>
        Task<HttpResult<DeepCoinV2WithdrawPage>> GetWithdrawHistoryAsync(string? asset = null, string? transactionHash = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default);

        /// <summary>
        /// Acquires the V2 listen key for the documented private WebSocket protocol.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/privateWS/subscribe" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/v2/listenkey/acquire
        /// </para>
        /// </summary>
        Task<HttpResult<DeepCoinListenKey>> StartUserStreamAsync(CancellationToken ct = default);

        /// <summary>
        /// Extends the V2 private listen-key lifetime by one hour.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/privateWS/subscribe" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/v2/listenkey/extend
        /// </para>
        /// </summary>
        Task<HttpResult<DeepCoinListenKey>> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default);
    }
}


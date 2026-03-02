using System.Threading.Tasks;
using System.Threading;
using DeepCoin.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using DeepCoin.Net.Enums;
using System;

namespace DeepCoin.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// DeepCoin Exchange account endpoints. Account endpoints include balance info, withdraw/deposit info and requesting and account settings
    /// </summary>
    public interface IDeepCoinRestClientExchangeApiAccount
    {
        /// <summary>
        /// Get account balances
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinAccount/getAccountBalance" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/account/balances
        /// </para>
        /// </summary>
        /// <param name="accountType">Account type</param>
        /// <param name="asset">Filter by asset, for example `ETH`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<DeepCoinBalance[]>> GetBalancesAsync(SymbolType accountType, string? asset = null, CancellationToken ct = default);

        /// <summary>
        /// 
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinAccount/getAccountBills" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/account/bills
        /// </para>
        /// </summary>
        /// <param name="accountType">Account type</param>
        /// <param name="asset">The asset, for example `ETH`</param>
        /// <param name="billType">Filter by bill type</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max number of results, max 100</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<DeepCoinBill[]>> GetBillsAsync(SymbolType accountType, string? asset = null, BillType? billType = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Set leverage for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinAccount/accountSetLeverage" /><br />
        /// Endpoint:<br />
        /// POST /deepcoin/account/set-leverage
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="leverage">Leverage</param>
        /// <param name="tradeMode">Margin mode</param>
        /// <param name="positionType">Position type</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<DeepCoinLeverage>> SetLeverageAsync(string symbol, decimal leverage, TradeMode tradeMode, PositionType positionType, CancellationToken ct = default);

        // Transfer endpoints currently not useable
        ///// <summary>
        ///// Get list of transferable assets
        ///// <para><a href="https://www.deepcoin.com/docs/InternalTransfer/getSupportCoin" /></para>
        ///// </summary>
        ///// <param name="ct">Cancellation token</param>
        //Task<WebCallResult<DeepCoinTransferableAsset>> GetTransferableAssetsAsync(CancellationToken ct = default);

        ///// <summary>
        ///// Transfer an asset to another account
        ///// <para><a href="https://www.deepcoin.com/docs/InternalTransfer/goInternalTransfer" /></para>
        ///// </summary>
        ///// <param name="asset">The asset, for example `ETH`</param>
        ///// <param name="quantity">Quantity</param>
        ///// <param name="toAccount">To account</param>
        ///// <param name="toAccountType">Account type</param>
        ///// <param name="clientOrderId">Client order id</param>
        ///// <param name="ct">Cancellation token</param>
        //Task<WebCallResult<DeepCoinTransferResult>> TransferAsync(string asset, decimal quantity, string toAccount, AccountType toAccountType, string? clientOrderId = null, CancellationToken ct = default);

        ///// <summary>
        ///// Get transfer history
        ///// <para><a href="https://www.deepcoin.com/docs/InternalTransfer/internalTransferHistory" /></para>
        ///// </summary>
        ///// <param name="toAccount">Filter by to account</param>
        ///// <param name="asset">Filter by asset</param>
        ///// <param name="status">Filter by status</param>
        ///// <param name="receiverId">Receiver account id</param>
        ///// <param name="orderId">Filter by order id</param>
        ///// <param name="startTime">Filter by start time</param>
        ///// <param name="endTime">Filter by end time</param>
        ///// <param name="page">Page number</param>
        ///// <param name="pageSize">Max number of results</param>
        ///// <param name="ct">Cancellation token</param>
        //Task<WebCallResult<DeepCoinTransferPage>> GetTransferHistoryAsync(string? toAccount = null, string? asset = null, TransferStatus? status = null, string? receiverId = null, string? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default);

        /// <summary>
        /// Get deposit history
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/assets/deposit" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/asset/deposit-list
        /// </para>
        /// </summary>
        /// <param name="asset">Filter by asset, for example `ETH`</param>
        /// <param name="transactionHash">Filter by transaction hash</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<DeepCoinDepositPage>> GetDepositHistoryAsync(string? asset = null, string? transactionHash = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default);

        /// <summary>
        /// Get withdrawal history
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/assets/withdraw" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/asset/withdraw-list
        /// </para>
        /// </summary>
        /// <param name="asset">Filter by asset, for example `ETH`</param>
        /// <param name="transactionHash">Filter by transaction hash</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<DeepCoinWithdrawPage>> GetWithdrawHistoryAsync(string? asset = null, string? transactionHash = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default);

        /// <summary>
        /// Start the user stream and return the listen key which can be used to subscribe to updates in the socket client
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/privateWS/subscribe" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/listenkey/acquire
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<DeepCoinListenKey>> StartUserStreamAsync(CancellationToken ct = default);

        /// <summary>
        /// Extend the lifetime of a listen key
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/privateWS/subscribe" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/listenkey/extend
        /// </para>
        /// </summary>
        /// <param name="listenKey">Listen key to extend</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<DeepCoinListenKey>> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default);
    }
}

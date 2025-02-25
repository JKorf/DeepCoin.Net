using System.Collections.Generic;
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
        /// <para><a href="https://www.deepcoin.com/docs/DeepCoinAccount/getAccountBalance" /></para>
        /// </summary>
        /// <param name="accountType">Account type</param>
        /// <param name="asset">Filter by asset, for example `ETH`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<IEnumerable<DeepCoinBalance>>> GetBalancesAsync(SymbolType accountType, string? asset = null, CancellationToken ct = default);

        /// <summary>
        /// 
        /// <para><a href="https://www.deepcoin.com/docs/DeepCoinAccount/getAccountBills" /></para>
        /// </summary>
        /// <param name="accountType">Account type</param>
        /// <param name="asset">The asset, for example `ETH`</param>
        /// <param name="billType">Filter by bill type</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max number of results, max 100</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<IEnumerable<DeepCoinBill>>> GetBillsAsync(SymbolType accountType, string? asset = null, BillType? billType = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Set leverage for a symbol
        /// <para><a href="https://www.deepcoin.com/docs/DeepCoinAccount/accountSetLeverage" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="leverage">Leverage</param>
        /// <param name="marginMode">Margin mode</param>
        /// <param name="positionType">Position type</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<DeepCoinLeverage>> SetLeverageAsync(string symbol, decimal leverage, MarginMode marginMode, PositionType positionType, CancellationToken ct = default);

    }
}

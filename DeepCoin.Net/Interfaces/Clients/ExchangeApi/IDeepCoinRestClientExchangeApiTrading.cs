using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using CryptoExchange.Net.Objects;
using DeepCoin.Net.Enums;
using DeepCoin.Net.Objects.Models;

namespace DeepCoin.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// DeepCoin Exchange trading endpoints, placing and managing orders.
    /// </summary>
    public interface IDeepCoinRestClientExchangeApiTrading
    {
        /// <summary>
        /// Get positions list
        /// <para><a href="https://www.deepcoin.com/docs/DeepCoinAccount/accountPositions" /></para>
        /// </summary>
        /// <param name="symbolType">Symbol type</param>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<IEnumerable<DeepCoinPosition>>> GetPositionsAsync(SymbolType symbolType, string? symbol = null, CancellationToken ct = default);

    }
}

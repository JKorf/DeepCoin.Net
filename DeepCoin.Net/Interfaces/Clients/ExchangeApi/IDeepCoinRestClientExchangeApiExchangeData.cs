using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using DeepCoin.Net.Enums;
using DeepCoin.Net.Objects.Models;

namespace DeepCoin.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// DeepCoin Exchange exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface IDeepCoinRestClientExchangeApiExchangeData
    {
        /// <summary>
        /// Get ticker price info
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinMarket/getMarketTickers" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/market/tickers
        /// </para>
        /// </summary>
        /// <param name="symbolType">Type of symbol</param>
        /// <param name="underlying">Filter by underlying (swap only)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<DeepCoinTicker[]>> GetTickersAsync(SymbolType symbolType, string? underlying = null, CancellationToken ct = default);

        /// <summary>
        /// Get symbols list
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinMarket/getBaseInfo" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/market/instruments
        /// </para>
        /// </summary>
        /// <param name="type">Symbol type</param>
        /// <param name="underlying">Filter by underlying</param>
        /// <param name="symbol">Filter by symbol name</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<DeepCoinSymbol[]>> GetSymbolsAsync(SymbolType type, string? underlying = null, string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get kline/candlestick data
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinMarket/getKlineData" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/market/candles
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max number of results, max 300</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<DeepCoinKline[]>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get order book
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinMarket/marketBooks" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/market/books
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="depth">Number of order book rows, max 400</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<DeepCoinOrderBook>> GetOrderBookAsync(string symbol, int? depth = null, CancellationToken ct = default);

        /// <summary>
        /// Get funding rate
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinTrade/fundingRate" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/trade/funding-rate
        /// </para>
        /// </summary>
        /// <param name="type">Contract type</param>
        /// <param name="symbol">Filter by symbol, for example `ETH-USDT-SWAP`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<DeepCoinFundingRate[]>> GetFundingRateAsync(ProductGroup type, string? symbol = null, CancellationToken ct = default);

    }
}

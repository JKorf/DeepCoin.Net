using CryptoExchange.Net.Objects;
using DeepCoin.Net.Enums;
using DeepCoin.Net.Objects.Models;
using Microsoft.VisualBasic;
using System;
using System.Threading;
using System.Threading.Tasks;

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
        /// <param name="symbolType">["<c>instType</c>"] Type of symbol</param>
        /// <param name="underlying">["<c>uly</c>"] Filter by underlying (swap only)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<DeepCoinTicker[]>> GetTickersAsync(SymbolType symbolType, string? underlying = null, CancellationToken ct = default);

        /// <summary>
        /// Get symbols list
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinMarket/getBaseInfo" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/market/instruments
        /// </para>
        /// </summary>
        /// <param name="type">["<c>instType</c>"] Symbol type</param>
        /// <param name="underlying">["<c>uly</c>"] Filter by underlying</param>
        /// <param name="symbol">["<c>instId</c>"] Filter by symbol name</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<DeepCoinSymbol[]>> GetSymbolsAsync(SymbolType type, string? underlying = null, string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get kline/candlestick data
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinMarket/getKlineData" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/market/candles
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETH-USDT`</param>
        /// <param name="interval">["<c>bar</c>"] Kline interval</param>
        /// <param name="endTime">["<c>after</c>"] Filter by end time</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results, max 300</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<DeepCoinKline[]>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get order book
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinMarket/marketBooks" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/market/books
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETH-USDT`</param>
        /// <param name="depth">["<c>sz</c>"] Number of order book rows, max 400</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<DeepCoinOrderBook>> GetOrderBookAsync(string symbol, int? depth = null, CancellationToken ct = default);

        /// <summary>
        /// Get funding rate
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinTrade/fundingRate" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/trade/funding-rate
        /// </para>
        /// </summary>
        /// <param name="type">["<c>instType</c>"] Contract type</param>
        /// <param name="symbol">["<c>instId</c>"] Filter by symbol, for example `ETH-USDT-SWAP`</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<DeepCoinFundingRate[]>> GetFundingRateAsync(ProductGroup type, string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get mark prices
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinMarket/getMarkPrice" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/market/mark-price<br />
        /// </para>
        /// </summary>
        /// <param name="symbolType">["<c>instType</c>"] Symbol type</param>
        /// <param name="underlying">["<c>uly</c>"] Filter by underlying</param>
        /// <param name="symbol">["<c>instId</c>"] Filter by symbol</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<DeepCoinMarkPrice[]>> GetMarkPricesAsync(
            SymbolType symbolType,
            string? underlying = null,
            string? symbol = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get open interest and volume
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinMarket/openInterestVolume" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/market/open-interest-volume<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETH-USDT-SWAP`</param>
        /// <param name="interval">["<c>bar</c>"] Interval</param>
        /// <param name="startTime">["<c>startTime</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>endTime</c>"] Filter by end time</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results, max 300</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<DeepCoinOpenInterest[]>> GetOpenInterestAndVolumeAsync(
            string symbol,
            DataInterval? interval = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get long short ratio
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinMarket/longShortRatio" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/market/long-short-ratio<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETH-USDT-SWAP`</param>
        /// <param name="interval">["<c>bar</c>"] Data interval</param>
        /// <param name="startTime">["<c>startTime</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>endTime</c>"] Filter by end time</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results, max 300</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<DeepCoinLongShortRatio[]>> GetLongShortRatioAsync(
            string symbol,
            DataInterval? interval = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get taker buy/sell volume
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinMarket/takerVolume" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/market/taker-volume<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETHUSDT`</param>
        /// <param name="interval">["<c>bar</c>"] Data interval</param>
        /// <param name="startTime">["<c>startTime</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>endTime</c>"] Filter by end time</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results, max 300</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<DeepCoinTakerBuySellVolume[]>> GetTakerBuySellVolumeAsync(
            string symbol,
            DataInterval? interval = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default);

    }
}

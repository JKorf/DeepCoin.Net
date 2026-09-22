using CryptoExchange.Net.Objects;
using DeepCoin.Net.Enums;
using DeepCoin.Net.Objects.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DeepCoin.Net.Interfaces.Clients.V2Api
{
    /// <summary>
    /// DeepCoin V2 market data endpoints.
    /// </summary>
    public interface IDeepCoinRestClientV2ApiExchangeData
    {
        /// <summary>
        /// Get the V2 exchange server time.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/DeepCoinMarket/systemInfo" /><br />
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        Task<HttpResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default);

        /// <summary>
        /// Get V2 product information.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/DeepCoinMarket/getBaseInfo" /><br />
        /// </para>
        /// </summary>
        /// <param name="type">["<c>instType</c>"] Spot or swap products.</param>
        /// <param name="underlying">["<c>uly</c>"] Optional underlying index for perpetuals.</param>
        /// <param name="symbol">["<c>instId</c>"] Optional instrument identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<HttpResult<DeepCoinSymbol[]>> GetSymbolsAsync(SymbolType type, string? underlying = null, string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Get V2 market tickers.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/DeepCoinMarket/getMarketTickers" /><br />
        /// </para>
        /// </summary>
        /// <param name="symbolType">["<c>instType</c>"] Spot or swap products.</param>
        /// <param name="underlying">["<c>uly</c>"] Optional underlying index for perpetuals.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<HttpResult<DeepCoinTicker[]>> GetTickersAsync(SymbolType symbolType, string? underlying = null, CancellationToken ct = default);

        /// <summary>
        /// Get V2 candlesticks using the V2 endTime pagination parameter.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/DeepCoinMarket/getKlineData" /><br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] Instrument identifier.</param>
        /// <param name="interval">["<c>bar</c>"] Candlestick interval.</param>
        /// <param name="endTime">["<c>endTime</c>"] Return records earlier than this time.</param>
        /// <param name="limit">["<c>limit</c>"] Number of results, at most 300; defaults to 100.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<HttpResult<DeepCoinKline[]>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get V2 depth using the required V2 limit parameter.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/DeepCoinMarket/marketBooks" /><br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] Instrument identifier.</param>
        /// <param name="depth">["<c>limit</c>"] Number of price levels, at most 400; this client defaults to 20.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<HttpResult<DeepCoinOrderBook>> GetOrderBookAsync(string symbol, int? depth = null, CancellationToken ct = default);

        /// <summary>
        /// Get consolidated V2 funding rates and settlement schedules for one or all contracts.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/DeepCoinMarket/fundingRate" /><br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] Optional canonical instrument identifier, for example BTC-USDT-SWAP.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<HttpResult<DeepCoinV2FundingRate[]>> GetFundingRatesAsync(string? symbol = null, CancellationToken ct = default);
    }
}

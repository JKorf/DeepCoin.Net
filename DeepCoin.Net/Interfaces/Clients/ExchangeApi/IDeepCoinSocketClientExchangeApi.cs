using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects.Sockets;
using DeepCoin.Net.Objects.Models;
using DeepCoin.Net.Enums;

namespace DeepCoin.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// DeepCoin Exchange streams
    /// </summary>
    public interface IDeepCoinSocketClientExchangeApi : ISocketApiClient, IDisposable
    {
        /// <summary>
        /// Subscribe to symbol/ticker updates for a symbol
        /// <para><a href="https://www.deepcoin.com/docs/publicWS/latestMarketData" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolUpdatesAsync(string symbol, Action<DataEvent<DeepCoinSymbolUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to live trade updates for a symbol
        /// <para><a href="https://www.deepcoin.com/docs/publicWS/lastTransactions" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<DeepCoinTradeUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to kline/candlestick updates for a symbol. Only 1 minute klines supported.
        /// <para><a href="https://www.deepcoin.com/docs/publicWS/KLines" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, Action<DataEvent<DeepCoinKlineUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to 25-level incremental order book updates
        /// <para><a href="https://www.deepcoin.com/docs/publicWS/25LevelIncrementalMarketData" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, Action<DataEvent<DeepCoinOrderBookUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Get the shared socket requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public IDeepCoinSocketClientExchangeApiShared SharedClient { get; }
    }
}

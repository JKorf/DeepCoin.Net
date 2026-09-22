using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects.Sockets;
using DeepCoin.Net.Objects.Models;
using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Authentication;

namespace DeepCoin.Net.Interfaces.Clients.V2Api
{
    /// <summary>
    /// DeepCoin V2 public streams and private streams authenticated with V2 listen keys
    /// </summary>
    public interface IDeepCoinSocketClientV2Api : ISocketApiClient<DeepCoinCredentials>, IDisposable
    {
        /// <summary>
        /// Subscribe to ticker updates for a symbol. Each event contains the received array of ticker updates.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/publicWS/latestMarketData" /><br />
        /// Endpoint:<br />
        /// streamlet/trade/public/swap or spot?platform=api&amp;version=v2
        /// </para>
        /// </summary>
        /// <param name="symbol">The canonical symbol, for example `ETH-USDT` for spot or `ETH-USDT-SWAP` for futures</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<DeepCoinV2TickerData[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to trade updates for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/publicWS/lastTransactions" /><br />
        /// Endpoint:<br />
        /// streamlet/trade/public/swap or spot?platform=api&amp;version=v2
        /// </para>
        /// </summary>
        /// <param name="symbol">The canonical symbol, for example `ETH-USDT` for spot or `ETH-USDT-SWAP` for futures</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<DeepCoinV2TradeData[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to candle updates for a symbol. Only 1 minute klines supported.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/publicWS/KLines" /><br />
        /// Endpoint:<br />
        /// streamlet/trade/public/swap or spot?platform=api&amp;version=v2
        /// </para>
        /// </summary>
        /// <param name="symbol">The canonical symbol, for example `ETH-USDT` for spot or `ETH-USDT-SWAP` for futures</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, Action<DataEvent<DeepCoinKline[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order book snapshots and incremental updates.
        /// Each event contains one book payload. The event metadata supplies the symbol, timestamp and snapshot/update kind.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/publicWS/LevelIncrementalMarketData" /><br />
        /// Endpoint:<br />
        /// streamlet/trade/public/swap or spot?platform=api&amp;version=v2
        /// </para>
        /// </summary>
        /// <param name="symbol">The canonical symbol, for example `ETH-USDT` for spot or `ETH-USDT-SWAP` for futures</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, Action<DataEvent<DeepCoinV2OrderBookData>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user data updates. A V2 listen key is automatically obtained by the client and renewed as needed.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/privateWS/subscribe" /><br />
        /// Endpoint:<br />
        /// v1/private
        /// </para>
        /// </summary>
        /// <param name="onOrderMessage">The event handler for order updates</param>
        /// <param name="onBalanceMessage">The event handler for balance updates</param>
        /// <param name="onPositionMessage">The event handler for position updates</param>
        /// <param name="onUserTradeMessage">The event handler for user trade updates</param>
        /// <param name="onAccountMessage">The event handler for account updates</param>
        /// <param name="onTriggerOrderMessage">The event handler for trigger order updates</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToUserDataUpdatesAsync(
            Action<DataEvent<DeepCoinOrderUpdate[]>>? onOrderMessage = null,
            Action<DataEvent<DeepCoinBalanceUpdate[]>>? onBalanceMessage = null,
            Action<DataEvent<DeepCoinPositionUpdate[]>>? onPositionMessage = null,
            Action<DataEvent<DeepCoinUserTradeUpdate[]>>? onUserTradeMessage = null,
            Action<DataEvent<DeepCoinAccountUpdate[]>>? onAccountMessage = null,
            Action<DataEvent<DeepCoinTriggerOrderUpdate[]>>? onTriggerOrderMessage = null,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user data updates with a caller-managed V2 listen key.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/privateWS/subscribe" /><br />
        /// Endpoint:<br />
        /// v1/private
        /// </para>
        /// </summary>
        /// <param name="listenKey">ListenKey as returned by <see cref="IDeepCoinRestClientV2ApiAccount.StartUserStreamAsync(CancellationToken)">restClient.V2Api.Account.StartUserStreamAsync</see></param>
        /// <param name="onOrderMessage">The event handler for order updates</param>
        /// <param name="onBalanceMessage">The event handler for balance updates</param>
        /// <param name="onPositionMessage">The event handler for position updates</param>
        /// <param name="onUserTradeMessage">The event handler for user trade updates</param>
        /// <param name="onAccountMessage">The event handler for account updates</param>
        /// <param name="onTriggerOrderMessage">The event handler for trigger order updates</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToUserDataUpdatesAsync(
            string listenKey,
            Action<DataEvent<DeepCoinOrderUpdate[]>>? onOrderMessage = null,
            Action<DataEvent<DeepCoinBalanceUpdate[]>>? onBalanceMessage = null,
            Action<DataEvent<DeepCoinPositionUpdate[]>>? onPositionMessage = null,
            Action<DataEvent<DeepCoinUserTradeUpdate[]>>? onUserTradeMessage = null,
            Action<DataEvent<DeepCoinAccountUpdate[]>>? onAccountMessage = null,
            Action<DataEvent<DeepCoinTriggerOrderUpdate[]>>? onTriggerOrderMessage = null,
            CancellationToken ct = default);

    }
}

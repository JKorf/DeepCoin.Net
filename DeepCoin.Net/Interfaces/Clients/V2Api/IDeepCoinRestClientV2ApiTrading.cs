using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using DeepCoin.Net.Enums;
using DeepCoin.Net.Objects.Models;

namespace DeepCoin.Net.Interfaces.Clients.V2Api
{
    /// <summary>
    /// DeepCoin V2 order and position endpoints.
    /// </summary>
    public interface IDeepCoinRestClientV2ApiTrading
    {
        /// <summary>
        /// Get one order by exchange or client order id.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/DeepCoinTrade/orders-detail" /><br />
        /// Endpoint:<br />
        /// POST /deepcoin/v2/trade/orders-detail
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] Product identifier.</param>
        /// <param name="orderId">["<c>ordId</c>"] Exchange order id; takes priority over clientOrderId.</param>
        /// <param name="clientOrderId">["<c>clOrdId</c>"] Client order id; either order id must be provided.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<HttpResult<DeepCoinOrder>> GetOrderAsync(string symbol, string? orderId = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Place an order. V2 returns the exchange order id only.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/DeepCoinTrade/order" /><br />
        /// Endpoint:<br />
        /// POST /deepcoin/v2/trade/order
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] Product identifier.</param>
        /// <param name="side">["<c>side</c>"] Order side.</param>
        /// <param name="orderType">["<c>ordType</c>"] Order type.</param>
        /// <param name="quantity">["<c>sz</c>"] Spot quantity or futures contracts.</param>
        /// <param name="price">["<c>px</c>"] Limit price.</param>
        /// <param name="tradeMode">["<c>tdMode</c>"] Cash for spot, cross or isolated for futures.</param>
        /// <param name="asset">["<c>ccy</c>"] Margin currency.</param>
        /// <param name="clientOrderId">["<c>clOrdId</c>"] Client order id, 1-20 alphanumeric characters.</param>
        /// <param name="quantityType">["<c>tgtCcy</c>"] Spot market-order quantity currency.</param>
        /// <param name="positionSide">["<c>posSide</c>"] Required futures position side.</param>
        /// <param name="positionType">["<c>mrgPosition</c>"] Required futures aggregation mode.</param>
        /// <param name="closePosId">["<c>closePosId</c>"] Position to close in split mode.</param>
        /// <param name="reduceOnly">["<c>reduceOnly</c>"] Reduce-only flag.</param>
        /// <param name="tpTriggerPrice">["<c>tpTriggerPx</c>"] Take-profit trigger price.</param>
        /// <param name="slTriggerPrice">["<c>slTriggerPx</c>"] Stop-loss trigger price.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<HttpResult<DeepCoinOrderResult>> PlaceOrderAsync(string symbol, OrderSide side, OrderType orderType, decimal quantity, decimal? price = null, TradeMode? tradeMode = null, string? asset = null, string? clientOrderId = null, QuantityType? quantityType = null, PositionSide? positionSide = null, PositionType? positionType = null, string? closePosId = null, bool? reduceOnly = null, decimal? tpTriggerPrice = null, decimal? slTriggerPrice = null, CancellationToken ct = default);

        /// <summary>
        /// Cancel an order by exchange order id. V2 does not accept a client order id.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/DeepCoinTrade/cancelOrder" /><br />
        /// Endpoint:<br />
        /// POST /deepcoin/v2/trade/cancel-order
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] Product identifier.</param>
        /// <param name="orderId">["<c>ordId</c>"] Exchange order id.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<HttpResult<DeepCoinOrderResult>> CancelOrderAsync(string symbol, string orderId, CancellationToken ct = default);

        /// <summary>
        /// Get pending orders, newest first. Omitting the symbol queries all product categories before pagination.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/DeepCoinTrade/ordersPendingV2" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/v2/trade/orders-pending
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] Optional product identifier.</param>
        /// <param name="page">["<c>page</c>"] Page number, starting at one.</param>
        /// <param name="pageSize">["<c>limit</c>"] Page size, maximum 100.</param>
        /// <param name="orderId">["<c>ordId</c>"] Optional exchange order id.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<HttpResult<DeepCoinOrder[]>> GetOpenOrdersAsync(string? symbol = null, int? page = null, int? pageSize = null, string? orderId = null, CancellationToken ct = default);

        /// <summary>
        /// Get executed trades.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/DeepCoinTrade/tradeFills" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/v2/trade/fills
        /// </para>
        /// </summary>
        /// <param name="symbolType">["<c>instType</c>"] Product category.</param>
        /// <param name="symbol">["<c>instId</c>"] Optional product identifier.</param>
        /// <param name="orderId">["<c>ordId</c>"] Optional exchange order id.</param>
        /// <param name="afterId">["<c>after</c>"] Return fills newer than this bill id.</param>
        /// <param name="beforeId">["<c>before</c>"] Return fills older than this bill id.</param>
        /// <param name="startTime">["<c>startTime</c>"] Start time, serialized as Unix milliseconds.</param>
        /// <param name="endTime">["<c>endTime</c>"] End time, serialized as Unix milliseconds.</param>
        /// <param name="limit">["<c>limit</c>"] Maximum results, at most 100.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<HttpResult<DeepCoinUserTrade[]>> GetUserTradesAsync(SymbolType symbolType, string? symbol = null, string? orderId = null, string? afterId = null, string? beforeId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get current positions, with sizes in contracts.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/DeepCoinAccount/accountPositions" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/v2/account/positions
        /// </para>
        /// </summary>
        /// <param name="symbolType">["<c>instType</c>"] Product category.</param>
        /// <param name="symbol">["<c>instId</c>"] Optional product identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<HttpResult<DeepCoinPosition[]>> GetPositionsAsync(SymbolType symbolType, string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Close exactly the specified positions. Inspect each result code for individual failures. A successful response can contain an empty result list; query positions to confirm closure when individual acknowledgements are absent.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/v2/DeepCoinTrade/close-position" /><br />
        /// Endpoint:<br />
        /// POST /deepcoin/v2/trade/close-position
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>instId</c>"] Product identifier.</param>
        /// <param name="positionIds">["<c>posIds</c>"] Nonempty collection of exact position identifiers.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<HttpResult<DeepCoinV2ClosePositionResult[]>> ClosePositionsAsync(string symbol, IEnumerable<string> positionIds, CancellationToken ct = default);
    }
}

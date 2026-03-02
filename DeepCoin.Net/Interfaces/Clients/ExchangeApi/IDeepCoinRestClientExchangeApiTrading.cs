using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using CryptoExchange.Net.Objects;
using DeepCoin.Net.Enums;
using DeepCoin.Net.Objects.Models;
using System;

namespace DeepCoin.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// DeepCoin Exchange trading endpoints, placing and managing orders.
    /// </summary>
    public interface IDeepCoinRestClientExchangeApiTrading
    {
        /// <summary>
        /// Get positions list
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinAccount/accountPositions" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/account/positions
        /// </para>
        /// </summary>
        /// <param name="symbolType">Symbol type</param>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<DeepCoinPosition[]>> GetPositionsAsync(SymbolType symbolType, string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Place a new order
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinTrade/order" /><br />
        /// Endpoint:<br />
        /// POST /deepcoin/trade/order
        /// </para>
        /// </summary>
        /// <param name="symbol">Symbol name, for example `ETH-USDT`</param>
        /// <param name="side">Order side</param>
        /// <param name="orderType">Order type</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="price">Limit price</param>
        /// <param name="tradeMode">Margin mode</param>
        /// <param name="asset">Margin asset, for example `ETH`</param>
        /// <param name="clientOrderId">Client order id</param>
        /// <param name="quantityType">Quantity type</param>
        /// <param name="positionSide">Position side</param>
        /// <param name="positionType">Position type</param>
        /// <param name="closePosId">Id of position to close</param>
        /// <param name="reduceOnly">Reduce only</param>
        /// <param name="tpTriggerPrice">Take profit trigger price</param>
        /// <param name="slTriggerPrice">Stop loss trigger price</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<DeepCoinOrderResult>> PlaceOrderAsync(string symbol, OrderSide side, OrderType orderType, decimal quantity, decimal? price = null, TradeMode? tradeMode = null, string? asset = null, string? clientOrderId = null, QuantityType? quantityType = null, PositionSide? positionSide = null, PositionType? positionType = null, string? closePosId = null, bool? reduceOnly = null, decimal? tpTriggerPrice = null, decimal? slTriggerPrice = null, CancellationToken ct = default);
        
        /// <summary>
        /// Edit an existing order. Spot not supported.
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinTrade/replaceOrder" /><br />
        /// Endpoint:<br />
        /// POST /deepcoin/trade/replace-order
        /// </para>
        /// </summary>
        /// <param name="orderId">Order id</param>
        /// <param name="price">New price</param>
        /// <param name="quantity">New quantity</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<DeepCoinOrderResult>> EditOrderAsync(string orderId, decimal? price = null, decimal? quantity = null, CancellationToken ct = default);

        /// <summary>
        /// Cancel an open order
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinTrade/cancelOrder" /><br />
        /// Endpoint:<br />
        /// POST /deepcoin/trade/cancel-order
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">Order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<DeepCoinOrderResult>> CancelOrderAsync(string symbol, string orderId, CancellationToken ct = default);

        /// <summary>
        /// Cancel multiple orders. Make sure to check the result data of the call to see if orders actually successfully canceled
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinTrade/batchCancelOrder" /><br />
        /// Endpoint:<br />
        /// POST /deepcoin/trade/batch-cancel-order
        /// </para>
        /// </summary>
        /// <param name="orderIds">Ids of orders to cancel</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<DeepCoinCancellationResult>> CancelOrdersAsync(IEnumerable<string> orderIds, CancellationToken ct = default);

        /// <summary>
        /// Cancel all orders matching the parameters
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinTrade/cancelAllOrder" /><br />
        /// Endpoint:<br />
        /// POST /deepcoin/trade/swap/cancel-all
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="productGroup">Product group</param>
        /// <param name="marginMode">Margin mode</param>
        /// <param name="positionType">Position type</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<DeepCoinCancellationResult>> CancelAllOrdersAsync(string symbol, ProductGroup productGroup, TradeMode marginMode, PositionType positionType, CancellationToken ct = default);

        /// <summary>
        /// Get user trades
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinTrade/tradeFills" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/trade/fills
        /// </para>
        /// </summary>
        /// <param name="symbolType">Symbol type</param>
        /// <param name="symbol">Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">Filter by order id</param>
        /// <param name="afterId">Return results after this id</param>
        /// <param name="beforeId">Return results before this id</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<DeepCoinUserTrade[]>> GetUserTradesAsync(SymbolType symbolType, string? symbol = null, string? orderId = null, string? afterId = null, string? beforeId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get a open order by id
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinTrade/orderByID" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/trade/orderByID
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">Order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<DeepCoinOrder>> GetOpenOrderAsync(string symbol, string orderId, CancellationToken ct = default);

        /// <summary>
        /// Get closed order by id
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinTrade/finishOrderByID" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/trade/finishOrderByID
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">Order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<DeepCoinOrder>> GetClosedOrderAsync(string symbol, string orderId, CancellationToken ct = default);

        /// <summary>
        /// Get closed order history
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinTrade/ordersHistory" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/trade/orders-history
        /// </para>
        /// </summary>
        /// <param name="symbolType">Symbol type</param>
        /// <param name="symbol">Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">Filter by order id</param>
        /// <param name="orderType">Filter by order type</param>
        /// <param name="status">Filter by order status</param>
        /// <param name="afterId">Return results after this id</param>
        /// <param name="beforeId">Return results before this id</param>
        /// <param name="limit">Max number of results, max 100</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<DeepCoinOrder[]>> GetClosedOrdersAsync(SymbolType symbolType, string? symbol = null, string? orderId = null, OrderType? orderType = null, OrderStatus? status = null, string? afterId = null, string? beforeId = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get open orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinTrade/ordersPendingV2" /><br />
        /// Endpoint:<br />
        /// GET /deepcoin/trade/v2/orders-pending
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETH-USDT`</param>
        /// <param name="page">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="orderId">Filter by order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<DeepCoinOrder[]>> GetOpenOrdersAsync(string symbol, int? page = null, int? pageSize = null, string? orderId = null, CancellationToken ct = default);

        /// <summary>
        /// Set take profit / stop loss trigger price
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.deepcoin.com/docs/DeepCoinTrade/replaceTPSL" /><br />
        /// Endpoint:<br />
        /// POST /deepcoin/trade/replace-order-sltp
        /// </para>
        /// </summary>
        /// <param name="orderId">Order id</param>
        /// <param name="takeProfitTriggerPrice">Take profit trigger price</param>
        /// <param name="stopLossTriggerPrice">Stop loss trigger price</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> SetTpSlAsync(string orderId, decimal? takeProfitTriggerPrice = null, decimal? stopLossTriggerPrice = null, CancellationToken ct = default);

    }
}

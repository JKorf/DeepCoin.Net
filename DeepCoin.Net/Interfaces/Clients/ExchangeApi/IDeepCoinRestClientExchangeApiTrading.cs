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
        /// <param name="symbolType">["<c>instType</c>"] Symbol type</param>
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETH-USDT`</param>
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
        /// <param name="symbol">["<c>instId</c>"] Symbol name, for example `ETH-USDT`</param>
        /// <param name="side">["<c>side</c>"] Order side</param>
        /// <param name="orderType">["<c>ordType</c>"] Order type</param>
        /// <param name="quantity">["<c>size</c>"] Quantity</param>
        /// <param name="price">["<c>px</c>"] Limit price</param>
        /// <param name="tradeMode">["<c>tdMode</c>"] Margin mode</param>
        /// <param name="asset">["<c>ccy</c>"] Margin asset, for example `ETH`</param>
        /// <param name="clientOrderId">["<c>clOrdId</c>"] Client order id</param>
        /// <param name="quantityType">["<c>tgtCcy</c>"] Quantity type</param>
        /// <param name="positionSide">["<c>posSide</c>"] Position side</param>
        /// <param name="positionType">["<c>mrgPosition</c>"] Position type</param>
        /// <param name="closePosId">["<c>closePosId</c>"] Id of position to close</param>
        /// <param name="reduceOnly">["<c>reduceOnly</c>"] Reduce only</param>
        /// <param name="tpTriggerPrice">["<c>tpTriggerPx</c>"] Take profit trigger price</param>
        /// <param name="slTriggerPrice">["<c>slTriggerPx</c>"] Stop loss trigger price</param>
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
        /// <param name="orderId">["<c>OrderSysID</c>"] Order id</param>
        /// <param name="price">["<c>price</c>"] New price</param>
        /// <param name="quantity">["<c>volume</c>"] New quantity</param>
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
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">["<c>ordId</c>"] Order id</param>
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
        /// <param name="orderIds">["<c>OrderSysIDs</c>"] Ids of orders to cancel</param>
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
        /// <param name="symbol">["<c>instrumentID</c>"] The symbol, for example `ETH-USDT`</param>
        /// <param name="productGroup">["<c>ProductGroup</c>"] Product group</param>
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
        /// <param name="symbolType">["<c>instType</c>"] Symbol type</param>
        /// <param name="symbol">["<c>instId</c>"] Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">["<c>ordId</c>"] Filter by order id</param>
        /// <param name="afterId">["<c>after</c>"] Return results after this id</param>
        /// <param name="beforeId">["<c>before</c>"] Return results before this id</param>
        /// <param name="startTime">["<c>begin</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>end</c>"] Filter by end time</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results</param>
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
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">["<c>ordId</c>"] Order id</param>
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
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">["<c>ordId</c>"] Order id</param>
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
        /// <param name="symbolType">["<c>instType</c>"] Symbol type</param>
        /// <param name="symbol">["<c>instId</c>"] Filter by symbol, for example `ETH-USDT`</param>
        /// <param name="orderId">["<c>ordId</c>"] Filter by order id</param>
        /// <param name="orderType">["<c>ordType</c>"] Filter by order type</param>
        /// <param name="status">["<c>state</c>"] Filter by order status</param>
        /// <param name="afterId">["<c>after</c>"] Return results after this id</param>
        /// <param name="beforeId">["<c>before</c>"] Return results before this id</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results, max 100</param>
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
        /// <param name="symbol">["<c>instId</c>"] The symbol, for example `ETH-USDT`</param>
        /// <param name="page">["<c>index</c>"] Page index</param>
        /// <param name="pageSize">["<c>limit</c>"] Page size</param>
        /// <param name="orderId">["<c>ordId</c>"] Filter by order id</param>
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
        /// <param name="orderId">["<c>orderSysID</c>"] Order id</param>
        /// <param name="takeProfitTriggerPrice">["<c>tpTriggerPx</c>"] Take profit trigger price</param>
        /// <param name="stopLossTriggerPrice">["<c>slTriggerPx</c>"] Stop loss trigger price</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> SetTpSlAsync(string orderId, decimal? takeProfitTriggerPrice = null, decimal? stopLossTriggerPrice = null, CancellationToken ct = default);

    }
}

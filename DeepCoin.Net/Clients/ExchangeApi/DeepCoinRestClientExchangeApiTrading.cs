using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using DeepCoin.Net.Interfaces.Clients.ExchangeApi;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System;
using DeepCoin.Net.Enums;
using DeepCoin.Net.Objects.Models;
using CryptoExchange.Net.RateLimiting.Guards;
using System.Drawing;
using System.Linq;

namespace DeepCoin.Net.Clients.ExchangeApi
{
    /// <inheritdoc />
    internal class DeepCoinRestClientExchangeApiTrading : IDeepCoinRestClientExchangeApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly DeepCoinRestClientExchangeApi _baseClient;
        private readonly ILogger _logger;

        internal DeepCoinRestClientExchangeApiTrading(ILogger logger, DeepCoinRestClientExchangeApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;
        }

        #region Get Positions

        /// <inheritdoc />
        public async Task<WebCallResult<DeepCoinPosition[]>> GetPositionsAsync(SymbolType symbolType, string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddEnum("instType", symbolType);
            parameters.AddOptional("instId", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/deepcoin/account/positions", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinPosition[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Place Order

        /// <inheritdoc />
        public async Task<WebCallResult<DeepCoinOrderResult>> PlaceOrderAsync(string symbol, OrderSide side, OrderType orderType, decimal quantity, decimal? price = null, TradeMode? tradeMode = null, string? asset = null, string? clientOrderId = null, QuantityType? quantityType = null, PositionSide? positionSide = null, PositionType? positionType = null, string? closePosId = null, bool? reduceOnly = null, decimal? tpTriggerPrice = null, decimal? slTriggerPrice = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            parameters.AddEnum("side", side);
            parameters.Add("ordType", orderType);
            parameters.AddString("size", quantity);
            parameters.AddEnum("tdMode", tradeMode ?? TradeMode.Cross);
            parameters.AddOptionalString("px", price);
            parameters.AddOptional("ccy", asset);
            parameters.AddOptional("clOrdId", clientOrderId);
            parameters.AddOptionalEnum("tgtCcy", quantityType);
            parameters.AddOptionalEnum("posSide", positionSide);
            parameters.AddOptionalEnum("mrgPosition", positionType);
            parameters.AddOptional("closePosId", closePosId);
            parameters.AddOptional("reduceOnly", reduceOnly);
            parameters.AddOptionalString("tpTriggerPx", tpTriggerPrice);
            parameters.AddOptionalString("slTriggerPx", slTriggerPrice);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/deepcoin/trade/order", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinOrderResult>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result;

            if (result.Data.ResultCode != 0)
                return result.AsError<DeepCoinOrderResult>(new ServerError(result.Data.ResultCode, result.Data.ResultMessage!));
            
            return result;
        }

        #endregion

        #region Edit Order

        /// <inheritdoc />
        public async Task<WebCallResult<DeepCoinOrderResult>> EditOrderAsync(string orderId, decimal? price = null, decimal? quantity = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("OrderSysID", $"{orderId}");
            parameters.AddOptional("price", price);
            parameters.AddOptional("volume", quantity);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/deepcoin/trade/replace-order", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinOrderResult>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result;

            if (result.Data.ResultCode != 0)
                return result.AsError<DeepCoinOrderResult>(new ServerError(result.Data.ResultCode, result.Data.ResultMessage!));

            return result;
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public async Task<WebCallResult<DeepCoinOrderResult>> CancelOrderAsync(string symbol, string orderId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            parameters.Add("ordId", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/deepcoin/trade/cancel-order", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinOrderResult>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result;

            if (result.Data.ResultCode != 0)
                return result.AsError<DeepCoinOrderResult>(new ServerError(result.Data.ResultCode, result.Data.ResultMessage!));

            return result;
        }

        #endregion

        #region Cancel Orders

        /// <inheritdoc />
        public async Task<WebCallResult<DeepCoinCancellationResult>> CancelOrdersAsync(IEnumerable<string> orderIds, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("OrderSysIDs", orderIds.ToArray());
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/deepcoin/trade/batch-cancel-order", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinCancellationResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel All Orders

        /// <inheritdoc />
        public async Task<WebCallResult<DeepCoinCancellationResult>> CancelAllOrdersAsync(string symbol, ProductGroup productGroup, TradeMode tradeMode, PositionType positionType, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instrumentID", symbol);
            parameters.AddEnum("ProductGroup", productGroup);
            parameters.Add("IsCrossMargin", tradeMode == TradeMode.Cross);
            parameters.Add("IsMergeMode", positionType);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/deepcoin/trade/swap/cancel-all", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinCancellationResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get User Trades

        /// <inheritdoc />
        public async Task<WebCallResult<DeepCoinUserTrade[]>> GetUserTradesAsync(SymbolType symbolType, string? symbol = null, string? orderId = null, string? afterId = null, string? beforeId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddEnum("instType", symbolType);
            parameters.AddOptional("instId", symbol);
            parameters.AddOptional("ordId", orderId);
            parameters.AddOptional("after", afterId);
            parameters.AddOptional("before", beforeId);
            parameters.AddOptionalMilliseconds("begin", startTime);
            parameters.AddOptionalMilliseconds("end", endTime);
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/deepcoin/trade/fills", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinUserTrade[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Open Order

        /// <inheritdoc />
        public async Task<WebCallResult<DeepCoinOrder>> GetOpenOrderAsync(string symbol, string orderId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            parameters.Add("ordId", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/deepcoin/trade/orderByID", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinOrder[]>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result.As<DeepCoinOrder>(default);

            var order = result.Data.SingleOrDefault();
            if (order == null)
                return result.AsError<DeepCoinOrder>(new ServerError("Order not found"));            

            return result.As(order);
        }

        #endregion

        #region Get Closed Order

        /// <inheritdoc />
        public async Task<WebCallResult<DeepCoinOrder>> GetClosedOrderAsync(string symbol, string orderId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            parameters.Add("ordId", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/deepcoin/trade/finishOrderByID", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinOrder[]>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result.As<DeepCoinOrder>(default);

            var order = result.Data.SingleOrDefault();
            if (order == null)
                return result.AsError<DeepCoinOrder>(new ServerError("Order not found"));

            return result.As(order);
        }

        #endregion

        #region Get Closed Orders

        /// <inheritdoc />
        public async Task<WebCallResult<DeepCoinOrder[]>> GetClosedOrdersAsync(SymbolType symbolType, string? symbol = null, string? orderId = null, OrderType? orderType = null, OrderStatus? status = null, string? afterId = null, string? beforeId = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddEnum("instType", symbolType);
            parameters.AddOptional("instId", symbol);
            parameters.AddOptional("ordId", orderId);
            parameters.AddOptionalEnum("ordType", orderType);
            parameters.AddOptionalEnum("state", status);
            parameters.AddOptional("after", afterId);
            parameters.AddOptional("before", beforeId);
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/deepcoin/trade/orders-history", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Open Orders

        /// <inheritdoc />
        public async Task<WebCallResult<DeepCoinOrder[]>> GetOpenOrdersAsync(string symbol, int? page = null, int? pageSize = null, string? orderId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("instId", symbol);
            parameters.Add("index", page ?? 1);
            parameters.AddOptional("limit", pageSize);
            parameters.AddOptional("ordId", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/deepcoin/trade/v2/orders-pending", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Set Tp Sl

        /// <inheritdoc />
        public async Task<WebCallResult> SetTpSlAsync(string orderId, decimal? takeProfitTriggerPrice = null, decimal? stopLossTriggerPrice = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("orderSysID", orderId);
            parameters.AddOptionalString("tpTriggerPx", takeProfitTriggerPrice);
            parameters.AddOptionalString("slTriggerPx", stopLossTriggerPrice);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/deepcoin/trade/replace-order-sltp", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}

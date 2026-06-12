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
using System.Linq;
using CryptoExchange.Net.Objects.Errors;

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
        public async Task<HttpResult<DeepCoinPosition[]>> GetPositionsAsync(SymbolType symbolType, string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instType", symbolType);
            parameters.Add("instId", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/account/positions", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinPosition[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Place Order

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinOrderResult>> PlaceOrderAsync(string symbol, OrderSide side, OrderType orderType, decimal quantity, decimal? price = null, TradeMode? tradeMode = null, string? asset = null, string? clientOrderId = null, QuantityType? quantityType = null, PositionSide? positionSide = null, PositionType? positionType = null, string? closePosId = null, bool? reduceOnly = null, decimal? tpTriggerPrice = null, decimal? slTriggerPrice = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("side", side);
            parameters.Add("ordType", orderType);
            parameters.Add("size", quantity);
            parameters.Add("tdMode", tradeMode ?? TradeMode.Cross);
            parameters.Add("px", price);
            parameters.Add("ccy", asset);
            parameters.Add("clOrdId", clientOrderId);
            parameters.Add("tgtCcy", quantityType);
            parameters.Add("posSide", positionSide);
            parameters.Add("mrgPosition", positionType);
            parameters.Add("closePosId", closePosId);
            parameters.Add("reduceOnly", reduceOnly);
            parameters.Add("tpTriggerPx", tpTriggerPrice);
            parameters.Add("slTriggerPx", slTriggerPrice);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/deepcoin/trade/order", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinOrderResult>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return result;

            if (result.Data.ResultCode != 0)
                return HttpResult.Fail<DeepCoinOrderResult>(result, new ServerError(result.Data.ResultCode, _baseClient.GetErrorInfo(result.Data.ResultCode, result.Data.ResultMessage!)));
            
            return result;
        }

        #endregion

        #region Edit Order

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinOrderResult>> EditOrderAsync(string orderId, decimal? price = null, decimal? quantity = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("OrderSysID", $"{orderId}");
            parameters.Add("price", price);
            parameters.Add("volume", quantity);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/deepcoin/trade/replace-order", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinOrderResult>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return result;

            if (result.Data.ResultCode != 0)
                return HttpResult.Fail<DeepCoinOrderResult>(result, new ServerError(result.Data.ResultCode, _baseClient.GetErrorInfo(result.Data.ResultCode, result.Data.ResultMessage!)));

            return result;
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinOrderResult>> CancelOrderAsync(string symbol, string orderId, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("ordId", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/deepcoin/trade/cancel-order", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinOrderResult>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return result;

            if (result.Data.ResultCode != 0)
                return HttpResult.Fail<DeepCoinOrderResult>(result, new ServerError(result.Data.ResultCode, _baseClient.GetErrorInfo(result.Data.ResultCode, result.Data.ResultMessage!)));

            return result;
        }

        #endregion

        #region Cancel Orders

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinCancellationResult>> CancelOrdersAsync(IEnumerable<string> orderIds, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("OrderSysIDs", orderIds.ToArray());
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/deepcoin/trade/batch-cancel-order", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinCancellationResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel All Orders

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinCancellationResult>> CancelAllOrdersAsync(string symbol, ProductGroup productGroup, TradeMode tradeMode, PositionType positionType, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instrumentID", symbol);
            parameters.Add("ProductGroup", productGroup);
            parameters.Add("IsCrossMargin", tradeMode == TradeMode.Cross ? "1" : "0");
            parameters.Add("IsMergeMode", positionType == PositionType.Merge ? "1" : "0");
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/deepcoin/trade/swap/cancel-all", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinCancellationResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get User Trades

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinUserTrade[]>> GetUserTradesAsync(SymbolType symbolType, string? symbol = null, string? orderId = null, string? afterId = null, string? beforeId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instType", symbolType);
            parameters.Add("instId", symbol);
            parameters.Add("ordId", orderId);
            parameters.Add("after", afterId);
            parameters.Add("before", beforeId);
            parameters.Add("begin", startTime);
            parameters.Add("end", endTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/trade/fills", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinUserTrade[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Open Order

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinOrder>> GetOpenOrderAsync(string symbol, string orderId, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("ordId", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/trade/orderByID", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinOrder[]>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<DeepCoinOrder>(result);

            var order = result.Data.SingleOrDefault();
            if (order == null)
                return HttpResult.Fail<DeepCoinOrder>(result, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            return HttpResult.Ok(result, order);
        }

        #endregion

        #region Get Closed Order

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinOrder>> GetClosedOrderAsync(string symbol, string orderId, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("ordId", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/trade/finishOrderByID", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinOrder[]>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<DeepCoinOrder>(result);

            var order = result.Data.SingleOrDefault();
            if (order == null)
                return HttpResult.Fail<DeepCoinOrder>(result, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            return HttpResult.Ok(result, order);
        }

        #endregion

        #region Get Closed Orders

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinOrder[]>> GetClosedOrdersAsync(SymbolType symbolType, string? symbol = null, string? orderId = null, OrderType? orderType = null, OrderStatus? status = null, string? afterId = null, string? beforeId = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instType", symbolType);
            parameters.Add("instId", symbol);
            parameters.Add("ordId", orderId);
            parameters.Add("ordType", orderType);
            parameters.Add("state", status);
            parameters.Add("after", afterId);
            parameters.Add("before", beforeId);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/trade/orders-history", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Open Orders

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinOrder[]>> GetOpenOrdersAsync(string symbol, int? page = null, int? pageSize = null, string? orderId = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("index", page ?? 1);
            parameters.Add("limit", pageSize);
            parameters.Add("ordId", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/trade/v2/orders-pending", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<DeepCoinOrder[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Set Tp Sl

        /// <inheritdoc />
        public async Task<HttpResult> SetTpSlAsync(string orderId, decimal? takeProfitTriggerPrice = null, decimal? stopLossTriggerPrice = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("orderSysID", orderId);
            parameters.Add("tpTriggerPx", takeProfitTriggerPrice);
            parameters.Add("slTriggerPx", stopLossTriggerPrice);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/deepcoin/trade/replace-order-sltp", DeepCoinExchange.RateLimiter.DeepCoin, 1, true, limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}

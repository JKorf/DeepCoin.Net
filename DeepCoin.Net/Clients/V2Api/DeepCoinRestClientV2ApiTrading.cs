using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net.Objects.Errors;
using DeepCoin.Net.Enums;
using DeepCoin.Net.Interfaces.Clients.V2Api;
using DeepCoin.Net.Objects.Models;
using Microsoft.Extensions.Logging;

namespace DeepCoin.Net.Clients.V2Api
{
    /// <inheritdoc />
    internal class DeepCoinRestClientV2ApiTrading : IDeepCoinRestClientV2ApiTrading
    {
        #region Statics
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        #endregion

        #region Fields
        private readonly DeepCoinRestClientV2Api _baseClient;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates the native V2 trading endpoint group.
        /// </summary>
        internal DeepCoinRestClientV2ApiTrading(ILogger logger, DeepCoinRestClientV2Api baseClient)
        {
            _baseClient = baseClient;
        }
        #endregion

        #region Methods
        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinOrder>> GetOrderAsync(string symbol, string? orderId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(orderId) && string.IsNullOrWhiteSpace(clientOrderId))
                throw new ArgumentException("An exchange or client order id is required", nameof(orderId));

            var order = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            order.Add("instId", symbol);
            order.Add("ordId", orderId);
            order.Add("clOrdId", clientOrderId);
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("orders", new[] { order });
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/deepcoin/v2/trade/orders-detail", DeepCoinExchange.RateLimiter.RestOrder, 1, true);
            var result = await _baseClient.SendAsync<DeepCoinV2OrderQueryResult[]>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<DeepCoinOrder>(result);
            if (result.Data == null || result.Data.Length == 0)
                return HttpResult.Fail<DeepCoinOrder>(result, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));
            if (result.Data.Length != 1)
                return HttpResult.Fail<DeepCoinOrder>(result, new ServerError(new ErrorInfo(ErrorType.Unknown, "DeepCoin returned multiple results for one order lookup")));

            var item = result.Data[0];
            if (item.ResultCode != 0)
            {
                var info = _baseClient.GetErrorInfo(item.ResultCode?.ToString() ?? "", item.ResultMessage ?? "DeepCoin omitted the order lookup result code");
                return HttpResult.Fail<DeepCoinOrder>(result, new ServerError(item.ResultCode?.ToString() ?? "", info));
            }
            if (item.Order == null)
                return HttpResult.Fail<DeepCoinOrder>(result, new ServerError(new ErrorInfo(ErrorType.Unknown, "DeepCoin omitted successful order details")));
            return HttpResult.Ok(result, item.Order);
        }

        /// <inheritdoc />
        public Task<HttpResult<DeepCoinOrderResult>> PlaceOrderAsync(string symbol, OrderSide side, OrderType orderType, decimal quantity, decimal? price = null, TradeMode? tradeMode = null, string? asset = null, string? clientOrderId = null, QuantityType? quantityType = null, PositionSide? positionSide = null, PositionType? positionType = null, string? closePosId = null, bool? reduceOnly = null, decimal? tpTriggerPrice = null, decimal? slTriggerPrice = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("side", side);
            parameters.Add("ordType", orderType);
            parameters.Add("sz", quantity);
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
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/deepcoin/v2/trade/order", DeepCoinExchange.RateLimiter.RestOrder, 1, true);
            return _baseClient.SendAsync<DeepCoinOrderResult>(request, parameters, ct);
        }

        /// <inheritdoc />
        public Task<HttpResult<DeepCoinOrderResult>> CancelOrderAsync(string symbol, string orderId, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("ordId", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/deepcoin/v2/trade/cancel-order", DeepCoinExchange.RateLimiter.RestOrder, 1, true);
            return _baseClient.SendAsync<DeepCoinOrderResult>(request, parameters, ct);
        }

        /// <inheritdoc />
        public Task<HttpResult<DeepCoinOrder[]>> GetOpenOrdersAsync(string? symbol = null, int? page = null, int? pageSize = null, string? orderId = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("page", page ?? 1);
            parameters.Add("limit", pageSize);
            parameters.Add("ordId", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/v2/trade/orders-pending", DeepCoinExchange.RateLimiter.RestAccount, 1, true, limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            return _baseClient.SendAsync<DeepCoinOrder[]>(request, parameters, ct);
        }

        /// <inheritdoc />
        public Task<HttpResult<DeepCoinUserTrade[]>> GetUserTradesAsync(SymbolType symbolType, string? symbol = null, string? orderId = null, string? afterId = null, string? beforeId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instType", symbolType);
            parameters.Add("instId", symbol);
            parameters.Add("ordId", orderId);
            parameters.Add("after", afterId);
            parameters.Add("before", beforeId);
            parameters.Add("startTime", startTime);
            parameters.Add("endTime", endTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/v2/trade/fills", DeepCoinExchange.RateLimiter.RestHistory, 1, true, limitGuard: new SingleLimitGuard(5, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            return _baseClient.SendAsync<DeepCoinUserTrade[]>(request, parameters, ct);
        }

        /// <inheritdoc />
        public Task<HttpResult<DeepCoinPosition[]>> GetPositionsAsync(SymbolType symbolType, string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instType", symbolType);
            parameters.Add("instId", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/deepcoin/v2/account/positions", DeepCoinExchange.RateLimiter.RestAccount, 1, true, limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            return _baseClient.SendAsync<DeepCoinPosition[]>(request, parameters, ct);
        }

        /// <inheritdoc />
        public async Task<HttpResult<DeepCoinV2ClosePositionResult[]>> ClosePositionsAsync(string symbol, IEnumerable<string> positionIds, CancellationToken ct = default)
        {
            var ids = positionIds.ToArray();
            // Omitting posIds closes every position for the symbol, so never serialize an empty selection.
            if (ids.Length == 0 || ids.Any(string.IsNullOrWhiteSpace))
                throw new ArgumentException("At least one nonempty position id is required", nameof(positionIds));
            var parameters = new Parameters(DeepCoinExchange._parameterSerializationSettings);
            parameters.Add("instId", symbol);
            parameters.Add("posIds", ids);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/deepcoin/v2/trade/close-position", DeepCoinExchange.RateLimiter.RestOrder, 1, true);
            // The endpoint wraps its result list in another data object, including when the successful list is empty.
            var result = await _baseClient.SendAsync<DeepCoinV2ClosePositionResponse>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<DeepCoinV2ClosePositionResult[]>(result);
            if (result.Data?.Results == null)
                return HttpResult.Fail<DeepCoinV2ClosePositionResult[]>(result, new ServerError(new ErrorInfo(ErrorType.Unknown, "DeepCoin omitted the position closing result list")));
            return HttpResult.Ok(result, result.Data.Results);
        }
        #endregion
    }
}

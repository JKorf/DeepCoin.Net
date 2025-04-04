using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Text;
using DeepCoin.Net.Interfaces.Clients.ExchangeApi;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using CryptoExchange.Net.Objects;
using DeepCoin.Net.Enums;
using DeepCoin.Net.Objects.Models;
using CryptoExchange.Net;

namespace DeepCoin.Net.Clients.ExchangeApi
{
    internal partial class DeepCoinRestClientExchangeApi : IDeepCoinRestClientExchangeApiShared
    {
        private const string _topicSpotId = "DeepCoinSpot";
        private const string _topicFuturesId = "DeepCoinFutures";
        public string Exchange => "DeepCoin";

        public TradingMode[] SupportedTradingModes => new[] { TradingMode.Spot, TradingMode.PerpetualLinear, TradingMode.PerpetualInverse };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();

        #region Balance client
        EndpointOptions<GetBalancesRequest> IBalanceRestClient.GetBalancesOptions { get; } = new EndpointOptions<GetBalancesRequest>(true);

        async Task<ExchangeWebResult<SharedBalance[]>> IBalanceRestClient.GetBalancesAsync(GetBalancesRequest request, CancellationToken ct)
        {
            var validationError = ((IBalanceRestClient)this).GetBalancesOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedBalance[]>(Exchange, validationError);

            var type = (request.TradingMode == null || request.TradingMode == TradingMode.Spot) ? Enums.SymbolType.Spot : Enums.SymbolType.Swap;

            var result = await Account.GetBalancesAsync(type, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedBalance[]>(Exchange, null, default);

            return result.AsExchangeResult<SharedBalance[]>(Exchange, TradingMode.Spot, result.Data.Select(x => new SharedBalance(x.Asset, x.AvailableBalance, x.Balance)).ToArray());
        }

        #endregion

        #region Deposit client
        EndpointOptions<GetDepositAddressesRequest> IDepositRestClient.GetDepositAddressesOptions { get; } = new EndpointOptions<GetDepositAddressesRequest>(true)
        {
            RequestNotes = "Deposit addresses request not available in API"
        };

        Task<ExchangeWebResult<SharedDepositAddress[]>> IDepositRestClient.GetDepositAddressesAsync(GetDepositAddressesRequest request, CancellationToken ct)
        {
            return Task.FromResult(new ExchangeWebResult<SharedDepositAddress[]>(Exchange, new InvalidOperationError("Deposit addresses request not available in API")));
        }

        GetDepositsOptions IDepositRestClient.GetDepositsOptions { get; } = new GetDepositsOptions(SharedPaginationSupport.Descending, true, 50);
        async Task<ExchangeWebResult<SharedDeposit[]>> IDepositRestClient.GetDepositsAsync(GetDepositsRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((IDepositRestClient)this).GetDepositsOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedDeposit[]>(Exchange, validationError);

            // Determine page token
            int page = 1;
            int pageSize = request.Limit ?? 50;
            if (pageToken is PageToken paginationToken)
            {
                page = paginationToken.Page;
                pageSize = paginationToken.PageSize;
            }

            // Get data
            var result = await Account.GetDepositHistoryAsync(request.Asset,
                startTime: request.StartTime,
                endTime: request.EndTime,
                pageSize: pageSize,
                page: page,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedDeposit[]>(Exchange, null, default);

            // Determine next token
            PageToken? nextToken = null;
            if (result.Data.Total > (result.Data.Page * result.Data.PageSize))
                nextToken = new PageToken(page++, pageSize);

            return result.AsExchangeResult<SharedDeposit[]>(Exchange, TradingMode.Spot, result.Data.Data.Select(x => new SharedDeposit(x.Asset, x.Quantity, x.DepositStatus == Enums.DepositStatus.Success, x.CreateTime)
            {
                TransactionId = x.TransactionHash,
                Network = x.NetworkName,
            }).ToArray(), nextToken);
        }

        #endregion

        #region Kline client

        GetKlinesOptions IKlineRestClient.GetKlinesOptions { get; } = new GetKlinesOptions(SharedPaginationSupport.Descending, true, 300, false,
                SharedKlineInterval.OneMinute,
                SharedKlineInterval.FiveMinutes,
                SharedKlineInterval.FifteenMinutes,
                SharedKlineInterval.ThirtyMinutes,
                SharedKlineInterval.OneHour,
                SharedKlineInterval.FourHours,
                SharedKlineInterval.TwelveHours,
                SharedKlineInterval.OneDay,
                SharedKlineInterval.OneWeek,
                SharedKlineInterval.OneMonth
            );

        async Task<ExchangeWebResult<SharedKline[]>> IKlineRestClient.GetKlinesAsync(GetKlinesRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var interval = (Enums.KlineInterval)request.Interval;
            if (!Enum.IsDefined(typeof(Enums.KlineInterval), interval))
                return new ExchangeWebResult<SharedKline[]>(Exchange, new ArgumentError("Interval not supported"));

            var validationError = ((IKlineRestClient)this).GetKlinesOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedKline[]>(Exchange, validationError);

            // Determine pagination
            // Data is normally returned oldest first, so to do newest first pagination we have to do some calc
            DateTime endTime = request.EndTime ?? DateTime.UtcNow;
            if (pageToken is DateTimeToken dateTimeToken)
                endTime = dateTimeToken.LastTime;

            var limit = request.Limit ?? 300;

            // Get data
            var result = await ExchangeData.GetKlinesAsync(
                request.Symbol.GetSymbol(FormatSymbol),
                interval,
                endTime,
                limit,
                ct: ct
                ).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedKline[]>(Exchange, null, default);

            // Get next token
            DateTimeToken? nextToken = null;
            if (result.Data.Count() == limit)
            {
                var minOpenTime = result.Data.Min(x => x.OpenTime);
                if (request.StartTime == null || minOpenTime > request.StartTime.Value)
                    nextToken = new DateTimeToken(minOpenTime.AddSeconds(-(int)interval));
            }

            return result.AsExchangeResult<SharedKline[]>(Exchange, request.Symbol.TradingMode, result.Data.Select(x => new SharedKline(x.OpenTime, x.ClosePrice, x.HighPrice, x.LowPrice, x.OpenPrice, x.Volume)).ToArray(), nextToken);
        }

        #endregion

        #region Listen Key client

        EndpointOptions<StartListenKeyRequest> IListenKeyRestClient.StartOptions { get; } = new EndpointOptions<StartListenKeyRequest>(true);
        async Task<ExchangeWebResult<string>> IListenKeyRestClient.StartListenKeyAsync(StartListenKeyRequest request, CancellationToken ct)
        {
            var validationError = ((IListenKeyRestClient)this).StartOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<string>(Exchange, validationError);

            // Get data
            var result = await Account.StartUserStreamAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<string>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, TradingMode.Spot, result.Data.ListenKey);
        }
        EndpointOptions<KeepAliveListenKeyRequest> IListenKeyRestClient.KeepAliveOptions { get; } = new EndpointOptions<KeepAliveListenKeyRequest>(true);
        async Task<ExchangeWebResult<string>> IListenKeyRestClient.KeepAliveListenKeyAsync(KeepAliveListenKeyRequest request, CancellationToken ct)
        {
            var validationError = ((IListenKeyRestClient)this).KeepAliveOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<string>(Exchange, validationError);

            // Get data
            var result = await Account.KeepAliveUserStreamAsync(request.ListenKey, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<string>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, TradingMode.Spot, request.ListenKey);
        }

        EndpointOptions<StopListenKeyRequest> IListenKeyRestClient.StopOptions { get; } = new EndpointOptions<StopListenKeyRequest>(true);
        Task<ExchangeWebResult<string>> IListenKeyRestClient.StopListenKeyAsync(StopListenKeyRequest request, CancellationToken ct)
        {            
            return Task.FromResult(new ExchangeWebResult<string>(Exchange, SupportedTradingModes, null, null, null, null, null, null, null, null, null, null, ResultDataSource.Server, request.ListenKey, null));
        }
        #endregion

        #region Order Book client
        GetOrderBookOptions IOrderBookRestClient.GetOrderBookOptions { get; } = new GetOrderBookOptions(1, 400, false);
        async Task<ExchangeWebResult<SharedOrderBook>> IOrderBookRestClient.GetOrderBookAsync(GetOrderBookRequest request, CancellationToken ct)
        {
            var validationError = ((IOrderBookRestClient)this).GetOrderBookOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedOrderBook>(Exchange, validationError);

            var result = await ExchangeData.GetOrderBookAsync(
                request.Symbol.GetSymbol(FormatSymbol),
                depth: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedOrderBook>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedOrderBook(result.Data.Asks, result.Data.Bids));
        }

        #endregion

        #region Withdrawal client

        GetWithdrawalsOptions IWithdrawalRestClient.GetWithdrawalsOptions { get; } = new GetWithdrawalsOptions(SharedPaginationSupport.Descending, true, 1000);
        async Task<ExchangeWebResult<SharedWithdrawal[]>> IWithdrawalRestClient.GetWithdrawalsAsync(GetWithdrawalsRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((IWithdrawalRestClient)this).GetWithdrawalsOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedWithdrawal[]>(Exchange, validationError);

            // Determine page token
            int page = 1;
            int pageSize = request.Limit ?? 50;
            if (pageToken is PageToken paginationToken)
            {
                page = paginationToken.Page;
                pageSize = paginationToken.PageSize;
            }

            // Get data
            var result = await Account.GetWithdrawHistoryAsync(
                request.Asset,
                startTime: request.StartTime,
                endTime: request.EndTime,
                pageSize: pageSize,
                page: page,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedWithdrawal[]>(Exchange, null, default);

            // Determine next token
            PageToken? nextToken = null;
            if (result.Data.Total > (result.Data.Page * result.Data.PageSize))
                nextToken = new PageToken(page++, pageSize);

            return result.AsExchangeResult<SharedWithdrawal[]>(Exchange, TradingMode.Spot, result.Data.Data.Select(x => new SharedWithdrawal(x.Asset, x.Address, x.Quantity, x.DepositStatus == Enums.WithdrawStatus.Success, x.CreateTime)
            {
                Address = x.Address
            }).ToArray());
        }

        #endregion

        #region Spot Ticker client

        EndpointOptions<GetTickerRequest> ISpotTickerRestClient.GetSpotTickerOptions { get; } = new EndpointOptions<GetTickerRequest>(false);
        async Task<ExchangeWebResult<SharedSpotTicker>> ISpotTickerRestClient.GetSpotTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotTickerRestClient)this).GetSpotTickerOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotTicker>(Exchange, validationError);

            var result = await ExchangeData.GetTickersAsync(Enums.SymbolType.Spot, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedSpotTicker>(Exchange, null, default);

            var symbol = result.Data.SingleOrDefault(x => x.Symbol == request.Symbol.GetSymbol(FormatSymbol));
            if (symbol == null)
                return result.AsExchangeError<SharedSpotTicker>(Exchange, new ServerError("Symbol not found"));

            return result.AsExchangeResult(Exchange, TradingMode.Spot, new SharedSpotTicker(ExchangeSymbolCache.ParseSymbol(_topicSpotId, symbol.Symbol), symbol.Symbol, symbol.LastPrice, symbol.HighPrice, symbol.LowPrice, symbol.Volume, symbol.OpenPrice == null ? null : Math.Round((symbol.LastPrice ?? 0) / symbol.OpenPrice.Value * 100 - 100, 3)));
        }

        EndpointOptions<GetTickersRequest> ISpotTickerRestClient.GetSpotTickersOptions { get; } = new EndpointOptions<GetTickersRequest>(false);
        async Task<ExchangeWebResult<SharedSpotTicker[]>> ISpotTickerRestClient.GetSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotTickerRestClient)this).GetSpotTickersOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotTicker[]>(Exchange, validationError);

            var result = await ExchangeData.GetTickersAsync(Enums.SymbolType.Spot, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedSpotTicker[]>(Exchange, null, default);

            return result.AsExchangeResult<SharedSpotTicker[]>(Exchange, TradingMode.Spot, result.Data.Select(x => new SharedSpotTicker(ExchangeSymbolCache.ParseSymbol(_topicSpotId, x.Symbol), x.Symbol, x.LastPrice, x.HighPrice, x.LowPrice, x.Volume, x.OpenPrice == null ? null : Math.Round((x.LastPrice ?? 0) / x.OpenPrice.Value * 100 - 100, 3))).ToArray());
        }

        #endregion

        #region Book Ticker client

        EndpointOptions<GetBookTickerRequest> IBookTickerRestClient.GetBookTickerOptions { get; } = new EndpointOptions<GetBookTickerRequest>(false);
        async Task<ExchangeWebResult<SharedBookTicker>> IBookTickerRestClient.GetBookTickerAsync(GetBookTickerRequest request, CancellationToken ct)
        {
            var validationError = ((IBookTickerRestClient)this).GetBookTickerOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedBookTicker>(Exchange, validationError);

            var symbol = request.Symbol.GetSymbol(FormatSymbol);
            var resultTicker = await ExchangeData.GetOrderBookAsync(symbol, 1, ct: ct).ConfigureAwait(false);
            if (!resultTicker)
                return resultTicker.AsExchangeResult<SharedBookTicker>(Exchange, null, default);

            return resultTicker.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedBookTicker(
                ExchangeSymbolCache.ParseSymbol(request.Symbol.TradingMode == TradingMode.Spot ? _topicSpotId : _topicFuturesId, symbol),
                symbol,
                resultTicker.Data.Asks[0].Price,
                resultTicker.Data.Asks[0].Quantity,
                resultTicker.Data.Bids[0].Price,
                resultTicker.Data.Bids[0].Quantity));
        }

        #endregion

        #region Spot Symbol client
        EndpointOptions<GetSymbolsRequest> ISpotSymbolRestClient.GetSpotSymbolsOptions { get; } = new EndpointOptions<GetSymbolsRequest>(false);

        async Task<ExchangeWebResult<SharedSpotSymbol[]>> ISpotSymbolRestClient.GetSpotSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotSymbolRestClient)this).GetSpotSymbolsOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotSymbol[]>(Exchange, validationError);

            var result = await ExchangeData.GetSymbolsAsync(Enums.SymbolType.Spot, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedSpotSymbol[]>(Exchange, null, default);

            var response = result.AsExchangeResult<SharedSpotSymbol[]>(Exchange, TradingMode.Spot, result.Data.Select(s => new SharedSpotSymbol(s.BaseAsset, s.QuoteAsset, s.Symbol, s.Status == Enums.SymbolStatus.Live)
            {
                MaxTradeQuantity = Math.Min(s.MaxLimitQuantity, s.MaxMarketQuantity),
                MinTradeQuantity = s.MinQuantity,
                PriceStep = s.TickSize,
                QuantityStep = s.LotSize
            }).ToArray());

            ExchangeSymbolCache.UpdateSymbolInfo(_topicSpotId, response.Data);
            return response;
        }

        #endregion

        #region Spot Order Client

        SharedFeeDeductionType ISpotOrderRestClient.SpotFeeDeductionType => SharedFeeDeductionType.DeductFromOutput;
        SharedFeeAssetType ISpotOrderRestClient.SpotFeeAssetType => SharedFeeAssetType.OutputAsset;
        SharedOrderType[] ISpotOrderRestClient.SpotSupportedOrderTypes { get; } = new[] { SharedOrderType.Limit, SharedOrderType.Market, SharedOrderType.LimitMaker };
        SharedTimeInForce[] ISpotOrderRestClient.SpotSupportedTimeInForce { get; } = new[] { SharedTimeInForce.GoodTillCanceled, SharedTimeInForce.ImmediateOrCancel };
        SharedQuantitySupport ISpotOrderRestClient.SpotSupportedOrderQuantity { get; } = new SharedQuantitySupport(
                SharedQuantityType.BaseAndQuoteAsset,
                SharedQuantityType.BaseAndQuoteAsset,
                SharedQuantityType.BaseAndQuoteAsset,
                SharedQuantityType.BaseAndQuoteAsset);

        string ISpotOrderRestClient.GenerateClientOrderId() => ExchangeHelpers.RandomString(32);

        PlaceSpotOrderOptions ISpotOrderRestClient.PlaceSpotOrderOptions { get; } = new PlaceSpotOrderOptions();
        async Task<ExchangeWebResult<SharedId>> ISpotOrderRestClient.PlaceSpotOrderAsync(PlaceSpotOrderRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).PlaceSpotOrderOptions.ValidateRequest(
                Exchange,
                request,
                request.Symbol.TradingMode,
                SupportedTradingModes,
                ((ISpotOrderRestClient)this).SpotSupportedOrderTypes,
                ((ISpotOrderRestClient)this).SpotSupportedTimeInForce,
                ((ISpotOrderRestClient)this).SpotSupportedOrderQuantity);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var result = await Trading.PlaceOrderAsync(
                request.Symbol.GetSymbol(FormatSymbol),
                request.Side == SharedOrderSide.Buy ? Enums.OrderSide.Buy : Enums.OrderSide.Sell,
                request.OrderType == SharedOrderType.Limit ? (request.TimeInForce == SharedTimeInForce.ImmediateOrCancel ? Enums.OrderType.ImmediateOrCancel : Enums.OrderType.Limit) : request.OrderType == SharedOrderType.Market ? Enums.OrderType.Market : Enums.OrderType.PostOnly,
                quantity: request.Quantity?.QuantityInBaseAsset ?? request.Quantity?.QuantityInQuoteAsset ?? 0,
                price: request.Price,
                quantityType: request.Quantity?.QuantityInQuoteAsset != null ? Enums.QuantityType.BaseAsset : Enums.QuantityType.QuoteAsset,
                clientOrderId: request.ClientOrderId,
                ct: ct).ConfigureAwait(false);

            if (!result)
                return result.AsExchangeResult<SharedId>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, TradingMode.Spot, new SharedId(result.Data.OrderId.ToString()));
        }

        EndpointOptions<GetOrderRequest> ISpotOrderRestClient.GetSpotOrderOptions { get; } = new EndpointOptions<GetOrderRequest>(true);
        async Task<ExchangeWebResult<SharedSpotOrder>> ISpotOrderRestClient.GetSpotOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetSpotOrderOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotOrder>(Exchange, validationError);

            var symbol = request.Symbol.GetSymbol(FormatSymbol);
            var order = await Trading.GetOpenOrderAsync(symbol, request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order) 
            {
                order = await Trading.GetClosedOrderAsync(symbol, request.OrderId, ct: ct).ConfigureAwait(false);
                
                if (!order) 
                    return order.AsExchangeResult<SharedSpotOrder>(Exchange, null, default);
            }

            return order.AsExchangeResult(Exchange, TradingMode.Spot, new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicSpotId, order.Data.Symbol),
                order.Data.Symbol,
                order.Data.OrderId,
                ParseOrderType(order.Data.OrderType),
                order.Data.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(order.Data.Status),
                order.Data.CreateTime)
            {
                ClientOrderId = order.Data.ClientOrderId,
                AveragePrice = order.Data.AverageFillPrice,
                OrderPrice = order.Data.Price,
                TimeInForce = order.Data.OrderType == OrderType.ImmediateOrCancel ? SharedTimeInForce.ImmediateOrCancel : null,
                OrderQuantity = new SharedOrderQuantity((order.Data.QuantityType == null || order.Data.QuantityType == QuantityType.BaseAsset) ? order.Data.Quantity : null, order.Data.QuantityType == QuantityType.QuoteAsset ? order.Data.Quantity : null),
                QuantityFilled = new SharedOrderQuantity(order.Data.QuantityFilled),                
                UpdateTime = order.Data.UpdateTime,
                Fee = order.Data.Fee,
                FeeAsset = order.Data.FeeAsset
            });
        }

        EndpointOptions<GetOpenOrdersRequest> ISpotOrderRestClient.GetOpenSpotOrdersOptions { get; } = new EndpointOptions<GetOpenOrdersRequest>(true)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(GetOpenOrdersRequest.Symbol), typeof(SharedSymbol), "Symbol to get open orders for", "ETH-USDT")
            }
        };

        async Task<ExchangeWebResult<SharedSpotOrder[]>> ISpotOrderRestClient.GetOpenSpotOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetOpenSpotOrdersOptions.ValidateRequest(Exchange, request, request.Symbol?.TradingMode ?? request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotOrder[]>(Exchange, validationError);

            var symbol = request.Symbol?.GetSymbol(FormatSymbol);
            var orders = await Trading.GetOpenOrdersAsync(symbol!, ct: ct).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<SharedSpotOrder[]>(Exchange, null, default);

            return orders.AsExchangeResult<SharedSpotOrder[]>(Exchange, TradingMode.Spot, orders.Data.Select(x => new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicSpotId, x.Symbol), 
                x.Symbol,
                x.OrderId,
                ParseOrderType(x.OrderType),
                x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(x.Status),
                x.CreateTime)
            {
                ClientOrderId = x.ClientOrderId,
                AveragePrice = x.AverageFillPrice,
                OrderPrice = x.Price,
                TimeInForce = x.OrderType == OrderType.ImmediateOrCancel ? SharedTimeInForce.ImmediateOrCancel : null,
                OrderQuantity = new SharedOrderQuantity((x.QuantityType == null || x.QuantityType == QuantityType.BaseAsset) ? x.Quantity : null, x.QuantityType == QuantityType.QuoteAsset ? x.Quantity : null),
                QuantityFilled = new SharedOrderQuantity(x.QuantityFilled),
                UpdateTime = x.UpdateTime,
                Fee = x.Fee,
                FeeAsset = x.FeeAsset,
            }).ToArray());
        }

        PaginatedEndpointOptions<GetClosedOrdersRequest> ISpotOrderRestClient.GetClosedSpotOrdersOptions { get; } = new PaginatedEndpointOptions<GetClosedOrdersRequest>(SharedPaginationSupport.Descending, false, 100, true);
        async Task<ExchangeWebResult<SharedSpotOrder[]>> ISpotOrderRestClient.GetClosedSpotOrdersAsync(GetClosedOrdersRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetClosedSpotOrdersOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotOrder[]>(Exchange, validationError);

            // Determine page token
            string? fromId = null;
            if (pageToken is FromIdToken fromIdToken)
                fromId = fromIdToken.FromToken;

            // Get data
            var orders = await Trading.GetClosedOrdersAsync(SymbolType.Spot,
                request.Symbol.GetSymbol(FormatSymbol),
                limit: request.Limit ?? 100,
                beforeId: fromId,
                ct: ct).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<SharedSpotOrder[]>(Exchange, null, default);

            // Get next token
            FromIdToken? nextToken = null;
            if (orders.Data.Any())
                nextToken = new FromIdToken(orders.Data.Min(o => o.OrderId)!);

            return orders.AsExchangeResult<SharedSpotOrder[]>(Exchange, TradingMode.Spot, orders.Data.Select(x => new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicSpotId, x.Symbol), 
                x.Symbol,
                x.OrderId,
                ParseOrderType(x.OrderType),
                x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(x.Status),
                x.CreateTime)
            {
                ClientOrderId = x.ClientOrderId,
                AveragePrice = x.AverageFillPrice,
                OrderPrice = x.Price,
                TimeInForce = x.OrderType == OrderType.ImmediateOrCancel ? SharedTimeInForce.ImmediateOrCancel : null,
                OrderQuantity = new SharedOrderQuantity((x.QuantityType == null || x.QuantityType == QuantityType.BaseAsset) ? x.Quantity : null, x.QuantityType == QuantityType.QuoteAsset ? x.Quantity : null),
                QuantityFilled = new SharedOrderQuantity(x.QuantityFilled),
                UpdateTime = x.UpdateTime,
                Fee = x.Fee,
                FeeAsset = x.FeeAsset,
            }).ToArray(), nextToken);
        }

        EndpointOptions<GetOrderTradesRequest> ISpotOrderRestClient.GetSpotOrderTradesOptions { get; } = new EndpointOptions<GetOrderTradesRequest>(true);
        async Task<ExchangeWebResult<SharedUserTrade[]>> ISpotOrderRestClient.GetSpotOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetSpotOrderTradesOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedUserTrade[]>(Exchange, validationError);

            var orders = await Trading.GetUserTradesAsync(
                SymbolType.Spot,
                request.Symbol.GetSymbol(FormatSymbol),
                orderId: request.OrderId, 
                ct: ct).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<SharedUserTrade[]>(Exchange, null, default);

            return orders.AsExchangeResult<SharedUserTrade[]>(Exchange, TradingMode.Spot, orders.Data.Select(x => new SharedUserTrade(
                ExchangeSymbolCache.ParseSymbol(_topicSpotId, x.Symbol), 
                x.Symbol,
                x.OrderId.ToString(),
                x.TradeId.ToString(),
                x.OrderSide == OrderSide.Sell ? SharedOrderSide.Sell: SharedOrderSide.Buy,
                x.Quantity,
                x.Price,
                x.Timestamp)
            {
                Fee = x.Fee,
                FeeAsset = x.FeeAsset,
                Role = x.Role == TradeRole.Maker ? SharedRole.Maker : SharedRole.Taker
            }).ToArray());
        }

        PaginatedEndpointOptions<GetUserTradesRequest> ISpotOrderRestClient.GetSpotUserTradesOptions { get; } = new PaginatedEndpointOptions<GetUserTradesRequest>(SharedPaginationSupport.Descending, true, 100, true);
        async Task<ExchangeWebResult<SharedUserTrade[]>> ISpotOrderRestClient.GetSpotUserTradesAsync(GetUserTradesRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetSpotUserTradesOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedUserTrade[]>(Exchange, validationError);

            // Determine page token
            DateTime? fromTimestamp = null;
            if (pageToken is DateTimeToken dateTimeToken)
                fromTimestamp = dateTimeToken.LastTime;

            // Get data
            var orders = await Trading.GetUserTradesAsync(
                SymbolType.Spot,
                request.Symbol.GetSymbol(FormatSymbol),
                startTime: request.StartTime,
                endTime: fromTimestamp ?? request.EndTime,
                limit: request.Limit ?? 100,
                ct: ct
                ).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<SharedUserTrade[]>(Exchange, null, default);

            // Get next token
            DateTimeToken? nextToken = null;
            if (orders.Data.Count() == (request.Limit ?? 100))
                nextToken = new DateTimeToken(orders.Data.Min(o => o.Timestamp).AddMilliseconds(-1));

            return orders.AsExchangeResult<SharedUserTrade[]>(Exchange, TradingMode.Spot, orders.Data.Select(x => new SharedUserTrade(
                ExchangeSymbolCache.ParseSymbol(_topicSpotId, x.Symbol), 
                x.Symbol,
                x.OrderId.ToString(),
                x.TradeId.ToString(),
                x.OrderSide == OrderSide.Sell ? SharedOrderSide.Sell : SharedOrderSide.Buy,
                x.Quantity,
                x.Price,
                x.Timestamp)
            {
                Fee = x.Fee,
                FeeAsset = x.FeeAsset,
                Role = x.Role == TradeRole.Maker ? SharedRole.Maker : SharedRole.Taker
            }).ToArray(), nextToken);
        }

        EndpointOptions<CancelOrderRequest> ISpotOrderRestClient.CancelSpotOrderOptions { get; } = new EndpointOptions<CancelOrderRequest>(true);
        async Task<ExchangeWebResult<SharedId>> ISpotOrderRestClient.CancelSpotOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).CancelSpotOrderOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var order = await Trading.CancelOrderAsync(request.Symbol.GetSymbol(FormatSymbol), request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedId>(Exchange, null, default);

            return order.AsExchangeResult(Exchange, TradingMode.Spot, new SharedId(order.Data.OrderId.ToString()));
        }

        private SharedOrderStatus ParseOrderStatus(OrderStatus status)
        {
            if (status == OrderStatus.Live || status == OrderStatus.PartiallyFilled) return SharedOrderStatus.Open;
            if (status == OrderStatus.Canceled) return SharedOrderStatus.Canceled;
            return SharedOrderStatus.Filled;
        }

        private SharedOrderType ParseOrderType(OrderType type)
        {
            if (type == OrderType.Market) return SharedOrderType.Market;
            if (type == OrderType.PostOnly) return SharedOrderType.LimitMaker;
            return SharedOrderType.Limit;
        }

        #endregion

        #region Leverage client
        SharedLeverageSettingMode ILeverageRestClient.LeverageSettingType => SharedLeverageSettingMode.PerSymbol;

        EndpointOptions<GetLeverageRequest> ILeverageRestClient.GetLeverageOptions { get; } = new EndpointOptions<GetLeverageRequest>(true);
        async Task<ExchangeWebResult<SharedLeverage>> ILeverageRestClient.GetLeverageAsync(GetLeverageRequest request, CancellationToken ct)
        {
            var validationError = ((ILeverageRestClient)this).GetLeverageOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedLeverage>(Exchange, validationError);

            var result = await Trading.GetPositionsAsync(SymbolType.Swap, symbol: request.Symbol.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedLeverage>(Exchange, null, default);

            if (!result.Data.Any())
                return result.AsExchangeError<SharedLeverage>(Exchange, new ServerError("Not found"));

            return result.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedLeverage(result.Data.First().Leverage)
            {
                Side = request.PositionSide
            });
        }

        SetLeverageOptions ILeverageRestClient.SetLeverageOptions { get; } = new SetLeverageOptions()
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(SetLeverageRequest.MarginMode), typeof(SharedMarginMode), "Margin mode to set leverage for", TradeMode.Cross)
            },
            OptionalExchangeParameters = new List<ParameterDescription>
            {
                new ParameterDescription("PositionType", typeof(PositionType), "Merge or split position mode", PositionType.Merge)
            }
        };

        async Task<ExchangeWebResult<SharedLeverage>> ILeverageRestClient.SetLeverageAsync(SetLeverageRequest request, CancellationToken ct)
        {
            var validationError = ((ILeverageRestClient)this).SetLeverageOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedLeverage>(Exchange, validationError);

            var positionType = ExchangeParameters.GetValue<PositionType?>(request.ExchangeParameters, Exchange, "PositionType");

            var result = await Account.SetLeverageAsync(
                symbol: request.Symbol.GetSymbol(FormatSymbol), 
                request.Leverage,
                tradeMode: request.MarginMode == SharedMarginMode.Cross ? TradeMode.Cross : TradeMode.Isolated,
                positionType ?? PositionType.Merge,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedLeverage>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedLeverage(result.Data.Leverage));
        }
        #endregion

        #region Futures Ticker client

        EndpointOptions<GetTickerRequest> IFuturesTickerRestClient.GetFuturesTickerOptions { get; } = new EndpointOptions<GetTickerRequest>(false);
        async Task<ExchangeWebResult<SharedFuturesTicker>> IFuturesTickerRestClient.GetFuturesTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesTickerRestClient)this).GetFuturesTickerOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFuturesTicker>(Exchange, validationError);

            var resultTicker = await ExchangeData.GetTickersAsync(SymbolType.Swap, ct: ct).ConfigureAwait(false);
            if (!resultTicker)
                return resultTicker.AsExchangeResult<SharedFuturesTicker>(Exchange, null, default);

            var symbol = resultTicker.Data.SingleOrDefault(x => x.Symbol == request.Symbol.GetSymbol(FormatSymbol));
            if (symbol == null)
                return resultTicker.AsExchangeError<SharedFuturesTicker>(Exchange, new ServerError("Symbol not found"));

            return resultTicker.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedFuturesTicker(ExchangeSymbolCache.ParseSymbol(_topicFuturesId, symbol.Symbol), symbol.Symbol, symbol.LastPrice, symbol.HighPrice, symbol.LowPrice, symbol.Volume, symbol.OpenPrice == null ? null : Math.Round((symbol.LastPrice ?? 0) / symbol.OpenPrice.Value * 100 - 100, 3)));
        }

        EndpointOptions<GetTickersRequest> IFuturesTickerRestClient.GetFuturesTickersOptions { get; } = new EndpointOptions<GetTickersRequest>(false);
        async Task<ExchangeWebResult<SharedFuturesTicker[]>> IFuturesTickerRestClient.GetFuturesTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesTickerRestClient)this).GetFuturesTickersOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFuturesTicker[]>(Exchange, validationError);

            var result = await ExchangeData.GetTickersAsync(Enums.SymbolType.Swap, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedFuturesTicker[]>(Exchange, null, default);

            return result.AsExchangeResult<SharedFuturesTicker[]>(Exchange, TradingMode.Spot, result.Data.Select(x => new SharedFuturesTicker(ExchangeSymbolCache.ParseSymbol(_topicFuturesId, x.Symbol), x.Symbol, x.LastPrice, x.HighPrice, x.LowPrice, x.Volume, x.OpenPrice == null ? null : Math.Round((x.LastPrice ?? 0) / x.OpenPrice.Value * 100 - 100, 3))).ToArray());
        }

        #endregion

        #region Futures Symbol client

        EndpointOptions<GetSymbolsRequest> IFuturesSymbolRestClient.GetFuturesSymbolsOptions { get; } = new EndpointOptions<GetSymbolsRequest>(false);
        async Task<ExchangeWebResult<SharedFuturesSymbol[]>> IFuturesSymbolRestClient.GetFuturesSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesSymbolRestClient)this).GetFuturesSymbolsOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFuturesSymbol[]>(Exchange, validationError);

            var result = await ExchangeData.GetSymbolsAsync(Enums.SymbolType.Swap, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedFuturesSymbol[]>(Exchange, null, default);

            var response = result.AsExchangeResult<SharedFuturesSymbol[]>(Exchange, TradingMode.Spot, result.Data.Select(s => 
            new SharedFuturesSymbol(s.QuoteAsset.Equals("USD") ? TradingMode.PerpetualInverse : TradingMode.PerpetualLinear, s.BaseAsset, s.QuoteAsset, s.Symbol, s.Status == Enums.SymbolStatus.Live)
            {
                MaxTradeQuantity = Math.Min(s.MaxLimitQuantity, s.MaxMarketQuantity),
                MinTradeQuantity = s.MinQuantity,
                PriceStep = s.TickSize,
                QuantityStep = s.LotSize,
                ContractSize = s.ContractSize          ,
                MaxLongLeverage = s.MaxLeverage,
                MaxShortLeverage = s.MaxLeverage
            }).ToArray());

            ExchangeSymbolCache.UpdateSymbolInfo(_topicFuturesId, response.Data);
            return response;
        }

        #endregion

        #region Futures Order Client
        SharedFeeDeductionType IFuturesOrderRestClient.FuturesFeeDeductionType => SharedFeeDeductionType.AddToCost;
        SharedFeeAssetType IFuturesOrderRestClient.FuturesFeeAssetType => SharedFeeAssetType.QuoteAsset;

        SharedOrderType[] IFuturesOrderRestClient.FuturesSupportedOrderTypes { get; } = new[] { SharedOrderType.Limit, SharedOrderType.Market, SharedOrderType.LimitMaker };
        SharedTimeInForce[] IFuturesOrderRestClient.FuturesSupportedTimeInForce { get; } = new[] { SharedTimeInForce.GoodTillCanceled, SharedTimeInForce.ImmediateOrCancel };
        SharedQuantitySupport IFuturesOrderRestClient.FuturesSupportedOrderQuantity { get; } = new SharedQuantitySupport(
                SharedQuantityType.Contracts,
                SharedQuantityType.Contracts,
                SharedQuantityType.Contracts,
                SharedQuantityType.Contracts);

        string IFuturesOrderRestClient.GenerateClientOrderId() => ExchangeHelpers.RandomString(32);

        PlaceFuturesOrderOptions IFuturesOrderRestClient.PlaceFuturesOrderOptions { get; } = new PlaceFuturesOrderOptions(true)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(PlaceFuturesOrderRequest.PositionSide), typeof(SharedPositionSide), "Position side of the order", PositionSide.Long)
            },
            OptionalExchangeParameters = new List<ParameterDescription>
            {
                new ParameterDescription("PositionType", typeof(PositionType), "Merge or split position mode", PositionType.Merge)
            }
        };
        async Task<ExchangeWebResult<SharedId>> IFuturesOrderRestClient.PlaceFuturesOrderAsync(PlaceFuturesOrderRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).PlaceFuturesOrderOptions.ValidateRequest(
                Exchange,
                request,
                request.Symbol.TradingMode,
                SupportedTradingModes,
                ((IFuturesOrderRestClient)this).FuturesSupportedOrderTypes,
                ((IFuturesOrderRestClient)this).FuturesSupportedTimeInForce,
                ((IFuturesOrderRestClient)this).FuturesSupportedOrderQuantity);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var positionType = ExchangeParameters.GetValue<PositionType?>(request.ExchangeParameters, Exchange, "PositionType");

            var result = await Trading.PlaceOrderAsync(
                request.Symbol.GetSymbol(FormatSymbol),
                request.Side == SharedOrderSide.Buy ? Enums.OrderSide.Buy : Enums.OrderSide.Sell,
                request.OrderType == SharedOrderType.Limit ? (request.TimeInForce == SharedTimeInForce.ImmediateOrCancel ? Enums.OrderType.ImmediateOrCancel : Enums.OrderType.Limit) : request.OrderType == SharedOrderType.Market ? Enums.OrderType.Market : Enums.OrderType.PostOnly,
                quantity: request.Quantity?.QuantityInContracts ?? 0,
                price: request.Price,
                clientOrderId: request.ClientOrderId,
                tradeMode: request.MarginMode == null ? null: request.MarginMode == SharedMarginMode.Isolated ? TradeMode.Isolated : TradeMode.Cross,
                positionSide: request.PositionSide == null? null: request.PositionSide == SharedPositionSide.Long? PositionSide.Long: PositionSide.Short,
                positionType: positionType ?? PositionType.Merge,
                tpTriggerPrice: request.TakeProfitPrice,
                slTriggerPrice: request.StopLossPrice,
                ct: ct).ConfigureAwait(false);

            if (!result)
                return result.AsExchangeResult<SharedId>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, TradingMode.Spot, new SharedId(result.Data.OrderId.ToString()));
        }

        EndpointOptions<GetOrderRequest> IFuturesOrderRestClient.GetFuturesOrderOptions { get; } = new EndpointOptions<GetOrderRequest>(true);
        async Task<ExchangeWebResult<SharedFuturesOrder>> IFuturesOrderRestClient.GetFuturesOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).GetFuturesOrderOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFuturesOrder>(Exchange, validationError);

            var symbol = request.Symbol.GetSymbol(FormatSymbol);
            var order = await Trading.GetOpenOrderAsync(symbol, request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order)
            {
                order = await Trading.GetClosedOrderAsync(symbol, request.OrderId, ct: ct).ConfigureAwait(false);

                if (!order)
                {
                    // NOTE: Market orders seem to return the incorrect order id when placing the order
                    // Place order endpoint returns order id X while the actually order id which can be retrieved is X + 1
                    order = await Trading.GetClosedOrderAsync(symbol, (long.Parse(request.OrderId) + 1).ToString(), ct: ct).ConfigureAwait(false);
                    if (!order)
                        return order.AsExchangeResult<SharedFuturesOrder>(Exchange, null, default);
                }
            }

            return order.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicFuturesId, order.Data.Symbol),
                order.Data.Symbol,
                order.Data.OrderId,
                ParseOrderType(order.Data.OrderType),
                order.Data.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(order.Data.Status),
                order.Data.CreateTime)
            {
                ClientOrderId = order.Data.ClientOrderId,
                AveragePrice = order.Data.AverageFillPrice,
                OrderPrice = order.Data.Price,
                TimeInForce = order.Data.OrderType == OrderType.ImmediateOrCancel ? SharedTimeInForce.ImmediateOrCancel : null,
                OrderQuantity = new SharedOrderQuantity(contractQuantity: order.Data.Quantity),
                QuantityFilled = new SharedOrderQuantity(contractQuantity: order.Data.QuantityFilled),
                UpdateTime = order.Data.UpdateTime,
                Fee = order.Data.Fee,
                FeeAsset = order.Data.FeeAsset,
                Leverage = order.Data.Leverage,
                PositionSide = order.Data.PositionSide == PositionSide.Long ? SharedPositionSide.Long: SharedPositionSide.Short,
                TakeProfitPrice = order.Data.TpTriggerPrice,
                StopLossPrice = order.Data.SlTriggerPrice
            });
        }

        EndpointOptions<GetOpenOrdersRequest> IFuturesOrderRestClient.GetOpenFuturesOrdersOptions { get; } = new EndpointOptions<GetOpenOrdersRequest>(true)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(GetOpenOrdersRequest.Symbol), typeof(SharedSymbol), "Symbol to get open orders for", "ETH-USDT-SWAP")
            }
        };
        async Task<ExchangeWebResult<SharedFuturesOrder[]>> IFuturesOrderRestClient.GetOpenFuturesOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).GetOpenFuturesOrdersOptions.ValidateRequest(Exchange, request, request.Symbol?.TradingMode ?? request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFuturesOrder[]>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var orders = await Trading.GetOpenOrdersAsync(symbol, ct: ct).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<SharedFuturesOrder[]>(Exchange, null, default);

            return orders.AsExchangeResult<SharedFuturesOrder[]>(Exchange, request.Symbol == null ? SupportedTradingModes : new[] { request.Symbol.TradingMode }, orders.Data.Select(x => new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicFuturesId, x.Symbol), 
                x.Symbol,
                x.OrderId,
                ParseOrderType(x.OrderType),
                x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(x.Status),
                x.CreateTime)
            {
                ClientOrderId = x.ClientOrderId,
                AveragePrice = x.AverageFillPrice,
                OrderPrice = x.Price,
                TimeInForce = x.OrderType == OrderType.ImmediateOrCancel ? SharedTimeInForce.ImmediateOrCancel : null,
                OrderQuantity = new SharedOrderQuantity(contractQuantity: x.Quantity),
                QuantityFilled = new SharedOrderQuantity(contractQuantity: x.QuantityFilled),
                UpdateTime = x.UpdateTime,
                Fee = x.Fee,
                FeeAsset = x.FeeAsset,
                Leverage = x.Leverage,
                PositionSide = x.PositionSide == PositionSide.Long ? SharedPositionSide.Long : SharedPositionSide.Short,
                TakeProfitPrice = x.TpTriggerPrice,
                StopLossPrice = x.SlTriggerPrice
            }).ToArray());
        }

        PaginatedEndpointOptions<GetClosedOrdersRequest> IFuturesOrderRestClient.GetClosedFuturesOrdersOptions { get; } = new PaginatedEndpointOptions<GetClosedOrdersRequest>(SharedPaginationSupport.Descending, true, 100, true);
        async Task<ExchangeWebResult<SharedFuturesOrder[]>> IFuturesOrderRestClient.GetClosedFuturesOrdersAsync(GetClosedOrdersRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).GetClosedFuturesOrdersOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFuturesOrder[]>(Exchange, validationError);

            // Determine page token
            string? fromId = null;
            if (pageToken is FromIdToken fromIdToken)
                fromId = fromIdToken.FromToken;

            // Get data
            var orders = await Trading.GetClosedOrdersAsync(SymbolType.Swap,
                request.Symbol.GetSymbol(FormatSymbol),
                limit: request.Limit ?? 100,
                beforeId: fromId,
                ct: ct).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<SharedFuturesOrder[]>(Exchange, null, default);

            // Get next token
            FromIdToken? nextToken = null;
            if (orders.Data.Any())
                nextToken = new FromIdToken(orders.Data.Min(o => o.OrderId)!);

            return orders.AsExchangeResult<SharedFuturesOrder[]>(Exchange, request.Symbol.TradingMode, orders.Data.Select(x => new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicFuturesId, x.Symbol), 
                x.Symbol,
                x.OrderId,
                ParseOrderType(x.OrderType),
                x.OrderSide == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(x.Status),
                x.CreateTime)
            {
                ClientOrderId = x.ClientOrderId,
                AveragePrice = x.AverageFillPrice,
                OrderPrice = x.Price,
                TimeInForce = x.OrderType == OrderType.ImmediateOrCancel ? SharedTimeInForce.ImmediateOrCancel : null,
                OrderQuantity = new SharedOrderQuantity(contractQuantity: x.Quantity),
                QuantityFilled = new SharedOrderQuantity(contractQuantity: x.QuantityFilled),
                UpdateTime = x.UpdateTime,
                Fee = x.Fee,
                FeeAsset = x.FeeAsset,
                Leverage = x.Leverage,
                PositionSide = x.PositionSide == PositionSide.Long ? SharedPositionSide.Long : SharedPositionSide.Short,
                TakeProfitPrice = x.TpTriggerPrice,
                StopLossPrice = x.SlTriggerPrice
            }).ToArray(), nextToken);
        }

        EndpointOptions<GetOrderTradesRequest> IFuturesOrderRestClient.GetFuturesOrderTradesOptions { get; } = new EndpointOptions<GetOrderTradesRequest>(true);
        async Task<ExchangeWebResult<SharedUserTrade[]>> IFuturesOrderRestClient.GetFuturesOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).GetFuturesOrderTradesOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedUserTrade[]>(Exchange, validationError);

            var orders = await Trading.GetUserTradesAsync(
                SymbolType.Swap,
                request.Symbol.GetSymbol(FormatSymbol),
                orderId: request.OrderId,
                ct: ct).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<SharedUserTrade[]>(Exchange, null, default);

            return orders.AsExchangeResult<SharedUserTrade[]>(Exchange, TradingMode.Spot, orders.Data.Select(x => new SharedUserTrade(
                ExchangeSymbolCache.ParseSymbol(_topicFuturesId, x.Symbol), 
                x.Symbol,
                x.OrderId.ToString(),
                x.TradeId.ToString(),
                x.OrderSide == OrderSide.Sell ? SharedOrderSide.Sell : SharedOrderSide.Buy,
                x.Quantity,
                x.Price,
                x.Timestamp)
            {
                Fee = x.Fee,
                FeeAsset = x.FeeAsset,
                Role = x.Role == TradeRole.Maker ? SharedRole.Maker : SharedRole.Taker
            }).ToArray());
        }

        PaginatedEndpointOptions<GetUserTradesRequest> IFuturesOrderRestClient.GetFuturesUserTradesOptions { get; } = new PaginatedEndpointOptions<GetUserTradesRequest>(SharedPaginationSupport.Descending, true, 100, true);
        async Task<ExchangeWebResult<SharedUserTrade[]>> IFuturesOrderRestClient.GetFuturesUserTradesAsync(GetUserTradesRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).GetFuturesUserTradesOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedUserTrade[]>(Exchange, validationError);

            // Determine page token
            DateTime? fromTimestamp = null;
            if (pageToken is DateTimeToken dateTimeToken)
                fromTimestamp = dateTimeToken.LastTime;

            // Get data
            var orders = await Trading.GetUserTradesAsync(
                SymbolType.Spot,
                request.Symbol.GetSymbol(FormatSymbol),
                startTime: request.StartTime,
                endTime: fromTimestamp ?? request.EndTime,
                limit: request.Limit ?? 100,
                ct: ct
                ).ConfigureAwait(false);
            if (!orders)
                return orders.AsExchangeResult<SharedUserTrade[]>(Exchange, null, default);

            // Get next token
            DateTimeToken? nextToken = null;
            if (orders.Data.Count() == (request.Limit ?? 100))
                nextToken = new DateTimeToken(orders.Data.Min(o => o.Timestamp).AddMilliseconds(-1));

            return orders.AsExchangeResult<SharedUserTrade[]>(Exchange, request.Symbol.TradingMode, orders.Data.Select(x => new SharedUserTrade(
                ExchangeSymbolCache.ParseSymbol(_topicFuturesId, x.Symbol), 
                x.Symbol,
                x.OrderId.ToString(),
                x.TradeId.ToString(),
                x.OrderSide == OrderSide.Sell ? SharedOrderSide.Sell : SharedOrderSide.Buy,
                x.Quantity,
                x.Price,
                x.Timestamp)
            {
                Fee = x.Fee,
                FeeAsset = x.FeeAsset,
                Role = x.Role == TradeRole.Maker ? SharedRole.Maker : SharedRole.Taker
            }).ToArray(), nextToken);
        }

        EndpointOptions<CancelOrderRequest> IFuturesOrderRestClient.CancelFuturesOrderOptions { get; } = new EndpointOptions<CancelOrderRequest>(true);
        async Task<ExchangeWebResult<SharedId>> IFuturesOrderRestClient.CancelFuturesOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).CancelFuturesOrderOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var order = await Trading.CancelOrderAsync(request.Symbol.GetSymbol(FormatSymbol), request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedId>(Exchange, null, default);

            return order.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedId(order.Data.OrderId.ToString()));
        }

        EndpointOptions<GetPositionsRequest> IFuturesOrderRestClient.GetPositionsOptions { get; } = new EndpointOptions<GetPositionsRequest>(true);
        async Task<ExchangeWebResult<SharedPosition[]>> IFuturesOrderRestClient.GetPositionsAsync(GetPositionsRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).GetPositionsOptions.ValidateRequest(Exchange, request, request.Symbol?.TradingMode ?? request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedPosition[]>(Exchange, validationError);

            var result = await Trading.GetPositionsAsync(SymbolType.Swap, symbol: request.Symbol?.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedPosition[]>(Exchange, null, default);

            IEnumerable<DeepCoinPosition> data = result.Data;
            if (request.TradingMode.HasValue)
                data = data.Where(x => request.TradingMode == TradingMode.PerpetualInverse ? x.Symbol.Contains("_USD_") : !x.Symbol.Contains("_USD_"));

            var resultTypes = request.Symbol == null && request.TradingMode == null ? SupportedTradingModes : request.Symbol != null ? new[] { request.Symbol.TradingMode } : new[] { request.TradingMode!.Value };
            return result.AsExchangeResult<SharedPosition[]>(Exchange, resultTypes, data.Select(x => new SharedPosition(ExchangeSymbolCache.ParseSymbol(_topicFuturesId, x.Symbol), x.Symbol, Math.Abs(x.Size), x.UpdateTime)
            {
                LiquidationPrice = x.LiquidationPrice == 0 ? null : x.LiquidationPrice,
                Leverage = x.Leverage,
                AverageOpenPrice = x.AveragePrice,                
                PositionSide = x.PositionSide == PositionSide.Long ? SharedPositionSide.Long : SharedPositionSide.Short
            }).ToArray());
        }

        EndpointOptions<ClosePositionRequest> IFuturesOrderRestClient.ClosePositionOptions { get; } = new EndpointOptions<ClosePositionRequest>(true)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(ClosePositionRequest.PositionSide), typeof(SharedPositionSide), "The position side to close", SharedPositionSide.Long),
                new ParameterDescription(nameof(ClosePositionRequest.Quantity), typeof(decimal), "Quantity of the position is required", 0.1m)
            }
        };
        async Task<ExchangeWebResult<SharedId>> IFuturesOrderRestClient.ClosePositionAsync(ClosePositionRequest request, CancellationToken ct)
        {
            var validationError = ((IFuturesOrderRestClient)this).ClosePositionOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var symbol = request.Symbol.GetSymbol(FormatSymbol);
            var result = await Trading.PlaceOrderAsync(
                symbol,
                request.PositionSide == SharedPositionSide.Long ? OrderSide.Sell : OrderSide.Buy,
                OrderType.Market,
                request.Quantity ?? 0,
                positionSide: request.PositionSide == SharedPositionSide.Short ? PositionSide.Short : PositionSide.Long,
                reduceOnly: true,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedId>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedId(result.Data.OrderId.ToString()));
        }
        #endregion
    }
}

# DeepCoin.Net AI API Quick Map

Use this file to route common user intents to the correct DeepCoin.Net client member. If a method name or parameter is not listed here, inspect `DeepCoin.Net/Interfaces/Clients/**` before generating code.

## Client Roots

| Intent | Use |
|---|---|
| REST calls | `new DeepCoinRestClient()` |
| WebSocket streams | `new DeepCoinSocketClient()` |
| API key authentication | `options.ApiCredentials = new DeepCoinCredentials("key", "secret", "pass")` |
| Live environment | `DeepCoinEnvironment.Live` |
| Custom environment | `DeepCoinEnvironment.CreateCustom(name, restAddress, socketAddress)` |
| Dependency injection | `services.AddDeepCoin(options => { ... })` |
| REST API root | `client.ExchangeApi` |
| WebSocket API root | `socketClient.ExchangeApi` |
| Shared REST client | `client.ExchangeApi.SharedClient` |
| Shared socket client | `socketClient.ExchangeApi.SharedClient` |

## Exchange Data REST

| User intent | DeepCoin.Net member |
|---|---|
| Get spot tickers | `client.ExchangeApi.ExchangeData.GetTickersAsync(SymbolType.Spot)` |
| Get swap tickers | `client.ExchangeApi.ExchangeData.GetTickersAsync(SymbolType.Swap)` |
| Get swap tickers by underlying | `client.ExchangeApi.ExchangeData.GetTickersAsync(SymbolType.Swap, underlying: "ETH-USDT")` |
| Get spot symbols | `client.ExchangeApi.ExchangeData.GetSymbolsAsync(SymbolType.Spot)` |
| Get swap symbols | `client.ExchangeApi.ExchangeData.GetSymbolsAsync(SymbolType.Swap)` |
| Get one symbol | `client.ExchangeApi.ExchangeData.GetSymbolsAsync(SymbolType.Spot, symbol: "ETH-USDT")` |
| Get klines/candles | `client.ExchangeApi.ExchangeData.GetKlinesAsync("ETH-USDT", KlineInterval.OneMinute)` |
| Get historical candles with limit | `client.ExchangeApi.ExchangeData.GetKlinesAsync("ETH-USDT", KlineInterval.OneHour, limit: 100)` |
| Get order book | `client.ExchangeApi.ExchangeData.GetOrderBookAsync("ETH-USDT")` |
| Get order book with depth | `client.ExchangeApi.ExchangeData.GetOrderBookAsync("ETH-USDT", depth: 25)` |
| Get USDT-margined funding rate | `client.ExchangeApi.ExchangeData.GetFundingRateAsync(ProductGroup.USDTMargined, "ETH-USDT-SWAP")` |
| Get coin-margined funding rate | `client.ExchangeApi.ExchangeData.GetFundingRateAsync(ProductGroup.CoinMargined, symbol)` |

## Account REST

| User intent | DeepCoin.Net member |
|---|---|
| Get spot balances | `client.ExchangeApi.Account.GetBalancesAsync(SymbolType.Spot)` |
| Get swap balances | `client.ExchangeApi.Account.GetBalancesAsync(SymbolType.Swap)` |
| Get balance for one asset | `client.ExchangeApi.Account.GetBalancesAsync(SymbolType.Spot, asset: "USDT")` |
| Get account bills | `client.ExchangeApi.Account.GetBillsAsync(SymbolType.Spot)` |
| Get account bills by asset | `client.ExchangeApi.Account.GetBillsAsync(SymbolType.Spot, asset: "USDT")` |
| Get account bills by type/time | `client.ExchangeApi.Account.GetBillsAsync(SymbolType.Swap, billType: billType, startTime: start, endTime: end, limit: 100)` |
| Set swap leverage | `client.ExchangeApi.Account.SetLeverageAsync("ETH-USDT-SWAP", 5m, TradeMode.Cross, PositionType.Split)` |
| Get deposit history | `client.ExchangeApi.Account.GetDepositHistoryAsync()` |
| Get deposit history by asset | `client.ExchangeApi.Account.GetDepositHistoryAsync(asset: "USDT")` |
| Get withdrawal history | `client.ExchangeApi.Account.GetWithdrawHistoryAsync()` |
| Get withdrawal history by asset | `client.ExchangeApi.Account.GetWithdrawHistoryAsync(asset: "USDT")` |
| Start private WebSocket stream | `client.ExchangeApi.Account.StartUserStreamAsync()` |
| Keep listen key alive | `client.ExchangeApi.Account.KeepAliveUserStreamAsync(listenKey)` |

## Trading REST

| User intent | DeepCoin.Net member |
|---|---|
| Get swap positions | `client.ExchangeApi.Trading.GetPositionsAsync(SymbolType.Swap)` |
| Get one swap position | `client.ExchangeApi.Trading.GetPositionsAsync(SymbolType.Swap, "ETH-USDT-SWAP")` |
| Place spot limit order | `client.ExchangeApi.Trading.PlaceOrderAsync("ETH-USDT", OrderSide.Buy, OrderType.Limit, quantity, price: price, tradeMode: TradeMode.Spot)` |
| Place spot market order | `client.ExchangeApi.Trading.PlaceOrderAsync("ETH-USDT", OrderSide.Buy, OrderType.Market, quantity, tradeMode: TradeMode.Spot)` |
| Place spot market buy by quote quantity | `client.ExchangeApi.Trading.PlaceOrderAsync("ETH-USDT", OrderSide.Buy, OrderType.Market, quoteQuantity, tradeMode: TradeMode.Spot, quantityType: QuantityType.QuoteAsset)` |
| Place swap market order | `client.ExchangeApi.Trading.PlaceOrderAsync("ETH-USDT-SWAP", OrderSide.Buy, OrderType.Market, quantity, tradeMode: TradeMode.Cross, positionSide: PositionSide.Long)` |
| Place reduce-only swap close order | `client.ExchangeApi.Trading.PlaceOrderAsync("ETH-USDT-SWAP", OrderSide.Sell, OrderType.Market, quantity, tradeMode: TradeMode.Cross, positionSide: PositionSide.Long, reduceOnly: true)` |
| Place order with TP/SL | `client.ExchangeApi.Trading.PlaceOrderAsync(symbol, side, type, quantity, tpTriggerPrice: tp, slTriggerPrice: sl, ...)` |
| Edit order | `client.ExchangeApi.Trading.EditOrderAsync(orderId, price: newPrice, quantity: newQuantity)` |
| Cancel order | `client.ExchangeApi.Trading.CancelOrderAsync("ETH-USDT", orderId)` |
| Cancel multiple orders | `client.ExchangeApi.Trading.CancelOrdersAsync(orderIds)` |
| Cancel all matching orders | `client.ExchangeApi.Trading.CancelAllOrdersAsync(symbol, productGroup, marginMode, positionType)` |
| Get user trades | `client.ExchangeApi.Trading.GetUserTradesAsync(SymbolType.Spot, "ETH-USDT")` |
| Get user trades by order id | `client.ExchangeApi.Trading.GetUserTradesAsync(SymbolType.Spot, "ETH-USDT", orderId: orderId)` |
| Get open order by id | `client.ExchangeApi.Trading.GetOpenOrderAsync("ETH-USDT", orderId)` |
| Get closed order by id | `client.ExchangeApi.Trading.GetClosedOrderAsync("ETH-USDT", orderId)` |
| Get closed order history | `client.ExchangeApi.Trading.GetClosedOrdersAsync(SymbolType.Spot, symbol: "ETH-USDT")` |
| Get open orders | `client.ExchangeApi.Trading.GetOpenOrdersAsync("ETH-USDT")` |
| Get open orders by order id | `client.ExchangeApi.Trading.GetOpenOrdersAsync("ETH-USDT", orderId: orderId)` |
| Set take profit / stop loss | `client.ExchangeApi.Trading.SetTpSlAsync(orderId, takeProfitTriggerPrice: tp, stopLossTriggerPrice: sl)` |

## WebSocket

| User intent | DeepCoin.Net member |
|---|---|
| Subscribe ticker/symbol updates | `socketClient.ExchangeApi.SubscribeToSymbolUpdatesAsync("ETH-USDT", handler)` |
| Subscribe trade updates | `socketClient.ExchangeApi.SubscribeToTradeUpdatesAsync("ETH-USDT", handler)` |
| Subscribe one-minute kline updates | `socketClient.ExchangeApi.SubscribeToKlineUpdatesAsync("ETH-USDT", handler)` |
| Subscribe order book updates | `socketClient.ExchangeApi.SubscribeToOrderBookUpdatesAsync("ETH-USDT", handler)` |
| Subscribe user data updates | `socketClient.ExchangeApi.SubscribeToUserDataUpdatesAsync(listenKey, ...)` |
| Handle order updates | `socketClient.ExchangeApi.SubscribeToUserDataUpdatesAsync(listenKey, onOrderMessage: handler)` |
| Handle balance updates | `socketClient.ExchangeApi.SubscribeToUserDataUpdatesAsync(listenKey, onBalanceMessage: handler)` |
| Handle position updates | `socketClient.ExchangeApi.SubscribeToUserDataUpdatesAsync(listenKey, onPositionMessage: handler)` |
| Handle user trade updates | `socketClient.ExchangeApi.SubscribeToUserDataUpdatesAsync(listenKey, onUserTradeMessage: handler)` |
| Handle account updates | `socketClient.ExchangeApi.SubscribeToUserDataUpdatesAsync(listenKey, onAccountMessage: handler)` |
| Handle trigger order updates | `socketClient.ExchangeApi.SubscribeToUserDataUpdatesAsync(listenKey, onTriggerOrderMessage: handler)` |
| Unsubscribe | `socketClient.UnsubscribeAsync(subscription.Data)` |

## SharedApis

| User intent | DeepCoin.Net member or interface |
|---|---|
| Shared REST client | `new DeepCoinRestClient().ExchangeApi.SharedClient` |
| Shared socket client | `new DeepCoinSocketClient().ExchangeApi.SharedClient` |
| Shared spot ticker REST | `ISpotTickerRestClient.GetSpotTickerAsync(new GetTickerRequest(symbol))` |
| Shared futures ticker REST | `IFuturesTickerRestClient.GetFuturesTickerAsync(new GetTickerRequest(symbol))` |
| Shared spot symbols REST | `ISpotSymbolRestClient.GetSpotSymbolsAsync(...)` |
| Shared futures symbols REST | `IFuturesSymbolRestClient.GetFuturesSymbolsAsync(...)` |
| Shared spot order REST | `ISpotOrderRestClient.PlaceSpotOrderAsync(...)` |
| Shared futures order REST | `IFuturesOrderRestClient.PlaceFuturesOrderAsync(...)` |
| Shared balance REST | `IBalanceRestClient.GetBalancesAsync(...)` |
| Shared deposit REST | `IDepositRestClient.GetDepositsAsync(...)` |
| Shared withdrawal REST | `IWithdrawalRestClient.GetWithdrawalsAsync(...)` |
| Shared order book REST | `IOrderBookRestClient.GetOrderBookAsync(...)` |
| Shared kline REST | `IKlineRestClient.GetKlinesAsync(...)` |
| Shared listen key REST | `IListenKeyRestClient.StartListenKeyAsync(...)` |
| Shared leverage REST | `ILeverageRestClient.SetLeverageAsync(...)` |
| Shared book ticker REST | `IBookTickerRestClient.GetBookTickerAsync(...)` |
| Shared ticker socket | `ITickerSocketClient.SubscribeToTickerUpdatesAsync(...)` |
| Shared trade socket | `ITradeSocketClient.SubscribeToTradeUpdatesAsync(...)` |
| Shared kline socket | `IKlineSocketClient.SubscribeToKlineUpdatesAsync(...)` |
| Shared balance socket | `IBalanceSocketClient.SubscribeToBalanceUpdatesAsync(...)` |
| Shared spot order socket | `ISpotOrderSocketClient.SubscribeToSpotOrderUpdatesAsync(...)` |
| Shared futures order socket | `IFuturesOrderSocketClient.SubscribeToFuturesOrderUpdatesAsync(...)` |
| Shared user trade socket | `IUserTradeSocketClient.SubscribeToUserTradeUpdatesAsync(...)` |
| Shared position socket | `IPositionSocketClient.SubscribeToPositionUpdatesAsync(...)` |

For shared socket subscriptions, keep the concrete socket client and unsubscribe with `await socketClient.UnsubscribeAsync(subscription.Data)`.

## Result Handling

| Situation | Pattern |
|---|---|
| REST success check | `if (!result.Success) { Console.WriteLine(result.Error); return; }` |
| Socket subscription success check | `if (!sub.Success) { Console.WriteLine(sub.Error); return; }` |
| Read REST data | Read `result.Data` only after `result.Success` |
| Retry decision | Retry only when `result.Error?.IsTransient == true` |
| Cancellation | Pass `ct: cancellationToken` |

## Common Routing Pitfalls

| Do not use | Use instead |
|---|---|
| `DeepCoinClient` | `DeepCoinRestClient` |
| `ApiCredentials` | `DeepCoinCredentials` |
| `SpotApi` | `ExchangeApi` plus `SymbolType.Spot` / `TradeMode.Spot` |
| `UsdFuturesApi` / `CoinFuturesApi` | `ExchangeApi` plus `SymbolType.Swap`, `ProductGroup`, and swap symbols |
| `ETHUSDT` in native DeepCoin methods | `ETH-USDT` |
| `ETHUSDT` swap symbol | `ETH-USDT-SWAP` |
| `GetTickerAsync("ETH-USDT")` | `GetTickersAsync(SymbolType.Spot)` then filter by symbol |
| `Account.GetPositionsAsync(...)` | `Trading.GetPositionsAsync(...)` |
| `Trading.GetOrderAsync(...)` | `Trading.GetOpenOrderAsync(...)` or `Trading.GetClosedOrderAsync(...)` |
| `CancelAllOrdersAsync(symbol)` | `CancelAllOrdersAsync(symbol, productGroup, marginMode, positionType)` |
| `.Data` without `.Success` check | Check `.Success` first |
| `ITickerSocketClient.UnsubscribeAsync(...)` | Keep the concrete socket client and call `socketClient.UnsubscribeAsync(subscription.Data)` |

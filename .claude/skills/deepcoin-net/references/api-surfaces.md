# API surfaces

## Client roots, symbols, and modes

`client` below is a `DeepCoinRestClient`; `socketClient` is a `DeepCoinSocketClient`. Both use `ExchangeApi`. REST groups are `ExchangeData`, `Account`, and `Trading`; socket subscriptions live directly under `ExchangeApi`.

Native symbols use `ETH-USDT` for spot and `ETH-USDT-SWAP` for swaps. `DeepCoinExchange.FormatSymbol` formats native shared symbols; socket methods accept native names and handle socket-specific formatting internally. Discover actual instruments before selecting one.

`SymbolType.Spot` / `Swap` select markets and account scopes. Order margin selection uses `TradeMode.Spot`, `Cross`, or `Isolated`. `ProductGroup.USDTMargined` / `CoinMargined` select derivative product groups for relevant methods. `PositionType.Merge` / `Split` and `PositionSide` control derivative position behavior; candles use `KlineInterval`.

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

## Private stream lifecycle

For credentialed clients, `SubscribeToUserDataUpdatesAsync` without a listen-key argument obtains a token lease and renews it internally. It routes order, balance, position, execution, account, and trigger-order callbacks through one user subscription. Keep and unsubscribe the returned subscription.

For externally managed keys, call `Account.StartUserStreamAsync`, pass `ListenKey` to the explicit-key overload, and maintain it with `KeepAliveUserStreamAsync`. The explicit-key overload does not acquire a managed token lease. Do not run two independent keepalive owners for the same managed workflow.

## Shared API V2

Use `client.ExchangeApi.SharedApi` and `socketClient.ExchangeApi.SharedApi`. The DI aggregate `IDeepCoinSharedApiClient` exposes `Rest` and `Socket`.

| Intent | Representative capabilities |
| --- | --- |
| Symbols and tickers | `IGetSpotSymbolsRest`, `IGetFuturesSymbolsRest`, `IGetTickerRest`, `IGetAllTickersRest`, `IGetBookTickerRest` |
| Accounts and market data | `IGetBalancesRest`, `IGetDepositHistoryRest`, `IGetWithdrawalHistoryRest`, `IGetKlinesRest`, `IGetOrderBookRest`, `IGetLeverageRest`, `ISetLeverageRest` |
| Spot trading | `IPlaceSpotOrderRest`, `IGetSpotOrderRest`, `IGetOpenSpotOrdersRest`, `IGetClosedSpotOrdersRest`, `ICancelSpotOrderRest`, order/trade history capabilities |
| Futures trading | `IPlaceFuturesOrderRest`, `IGetFuturesOrderRest`, `IGetOpenFuturesOrdersRest`, `IGetClosedFuturesOrdersRest`, `ICancelFuturesOrderRest`, `IGetPositionsRest`, order/trade history capabilities |
| Public socket | `ISubscribeKlinesSocket`, `ISubscribeTickerSocket`, `ISubscribeTradesSocket` |
| Private socket | `ISubscribeBalancesSocket`, `ISubscribeSpotOrdersSocket`, `ISubscribeFuturesOrdersSocket`, `ISubscribeUserTradesSocket`, `ISubscribePositionsSocket` |

Shared socket interfaces do not expose order-book subscriptions or socket order placement. Use the native order-book method or a maintained local book when required. Shared REST supports deposit/withdrawal history reads rather than withdrawal placement. Do not call internal listen-key helpers as public capabilities.

For dynamic selection, call the aggregate's `GetCapability` with a descriptor such as `SharedCapabilities.Tickers.GetTicker.Rest` and the required trading mode. Check for a missing match before accessing `Capability` and `Options`. Use `GetCapabilities` for matching implementations and `Discover()` for operation metadata. With multiple exchanges registered, resolve `IEnumerable<TCapability>` and select by exchange and supported mode.

### Catalogs, quantities, and prerequisites

Call `IGetSpotSymbolsRest.GetSpotSymbolsAsync(new GetSymbolsRequest())` or `IGetFuturesSymbolsRest.GetFuturesSymbolsAsync(new GetSymbolsRequest())` successfully before reading `SpotSymbolCatalog` or `FuturesSymbolCatalog`. Requests support base/quote asset type and subtype filters; results include `DisplayName`, constraints, and classifications (`Crypto`, `Fiat`, `TradFi`; applicable `StableCoin`, `Equity`, `Commodity` subtypes). Inverse futures can have fiat quote assets; commodity/equity underlyings receive corresponding classifications where supported.

Shared orders use `SharedOrderQuantity`. Spot limit orders use base quantity, and market orders can use base or quote quantity. Futures orders use contract quantities through `QuantityInContracts`. Shared REST books report `SharedQuantityType.BaseAsset` for spot and `SharedQuantityType.Contracts` for derivatives.

Shared all-ticker requests require `TradingMode`. Spot and futures open-order requests require a symbol. Futures placement requires `PositionSide` and does not set leverage inline. Set leverage separately with the required `MarginMode`; the `PositionType` exchange parameter selects split/merge behavior (placement also accepts alias `mrgPosition`). Inspect operation-specific rules before copying a portable request from another exchange.

Shared REST uses `HttpResult<T>` / `HttpResult`, subscriptions use `WebSocketResult<UpdateSubscription>`, and non-I/O symbol/cache helpers can return `ExchangeCallResult<T>`.

## Environments and source navigation

`DeepCoinEnvironment.Live` is the built-in environment. `CreateCustom` takes a name, REST address, and socket address; custom endpoints do not themselves supply a testnet.

Checkout-only: [public interfaces](../../../../DeepCoin.Net/Interfaces/Clients/), [local order books](../../../../DeepCoin.Net/SymbolOrderBooks/), and [tracker factory](../../../../DeepCoin.Net/DeepCoinTrackerFactory.cs).

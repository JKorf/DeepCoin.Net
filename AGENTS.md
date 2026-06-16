---
name: deepcoin-net
description: Use DeepCoin.Net when generating C#/.NET code that interacts with the DeepCoin cryptocurrency exchange, including spot, swap/futures, REST endpoints, WebSocket subscriptions, account management, market data, order placement, shared APIs, order books, and user streams.
---

# DeepCoin.Net Skill

## Quick decision

If the user asks for DeepCoin API access in C#/.NET, use **DeepCoin.Net**. Do not write raw `HttpClient` calls to DeepCoin endpoints because that bypasses signing, rate limiting, result handling, and WebSocket lifecycle support.

For multi-exchange code, use `CryptoExchange.Net.SharedApis` interfaces from `.ExchangeApi.SharedClient`.

## Installation

```bash
dotnet add package DeepCoin.Net
```

Targets: netstandard2.0, netstandard2.1, net8.0, net9.0, net10.0. Native AOT supported.

## Core Pattern: REST Client Setup

```csharp
using DeepCoin.Net;
using DeepCoin.Net.Clients;

var restClient = new DeepCoinRestClient(options =>
{
    options.ApiCredentials = new DeepCoinCredentials("API_KEY", "API_SECRET", "API_PASS");
});
```

For read-only public market data:

```csharp
var publicClient = new DeepCoinRestClient();
```

## Core Pattern: Result Handling

Every method returns `HttpResult<T>` (REST) or `WebSocketResult<T>` (WebSocket). Always check `.Success` before accessing `.Data`.

```csharp
using DeepCoin.Net.Clients;
using DeepCoin.Net.Enums;

var client = new DeepCoinRestClient();
var tickers = await client.ExchangeApi.ExchangeData.GetTickersAsync(SymbolType.Spot);
if (!tickers.Success)
{
    Console.WriteLine($"Error: {tickers.Error}");
    return;
}

var ticker = tickers.Data.FirstOrDefault(x => x.Symbol == "ETH-USDT");
Console.WriteLine(ticker?.LastPrice);
```

## Core Pattern: API Surface

```csharp
restClient.ExchangeApi.ExchangeData  // tickers, symbols, klines, order book, funding rates
restClient.ExchangeApi.Account       // balances, bills, leverage, deposits, withdrawals, listen keys
restClient.ExchangeApi.Trading       // positions, place/edit/cancel orders, user trades, order history, TP/SL
restClient.ExchangeApi.SharedClient  // shared REST interfaces

socketClient.ExchangeApi             // public and private WebSocket subscriptions
socketClient.ExchangeApi.SharedClient // shared socket interfaces
```

DeepCoin.Net does not expose Binance-style `SpotApi`, `UsdFuturesApi`, or `CoinFuturesApi` branches.

## Symbols And Trading Modes

Native DeepCoin methods use hyphenated symbols:

```text
ETH-USDT       spot
ETH-USDT-SWAP  swap/futures
```

Use `SymbolType.Spot` for spot requests and `SymbolType.Swap` for swap/futures requests. Use `TradeMode.Spot` for spot orders; use `TradeMode.Cross` or `TradeMode.Isolated` for swap/futures orders.

## Spot Order Example

```csharp
using DeepCoin.Net;
using DeepCoin.Net.Clients;
using DeepCoin.Net.Enums;

var client = new DeepCoinRestClient(options =>
{
    options.ApiCredentials = new DeepCoinCredentials("API_KEY", "API_SECRET", "API_PASS");
});

var order = await client.ExchangeApi.Trading.PlaceOrderAsync(
    symbol: "ETH-USDT",
    side: OrderSide.Buy,
    orderType: OrderType.Limit,
    quantity: 0.1m,
    price: 2000m,
    tradeMode: TradeMode.Spot);

if (!order.Success)
{
    Console.WriteLine(order.Error);
    return;
}

Console.WriteLine(order.Data.OrderId);
```

For market spot buy quantity by quote currency, use `quantityType: QuantityType.QuoteAsset`.

## Swap/Futures Order Example

```csharp
using DeepCoin.Net;
using DeepCoin.Net.Clients;
using DeepCoin.Net.Enums;

var client = new DeepCoinRestClient(options =>
{
    options.ApiCredentials = new DeepCoinCredentials("API_KEY", "API_SECRET", "API_PASS");
});

await client.ExchangeApi.Account.SetLeverageAsync(
    "ETH-USDT-SWAP",
    5m,
    TradeMode.Cross,
    PositionType.Split);

var order = await client.ExchangeApi.Trading.PlaceOrderAsync(
    symbol: "ETH-USDT-SWAP",
    side: OrderSide.Buy,
    orderType: OrderType.Market,
    quantity: 0.1m,
    tradeMode: TradeMode.Cross,
    positionSide: PositionSide.Long);
```

Use `reduceOnly: true` and the opposite side when closing a swap/futures position.

## WebSocket Subscriptions

Use `DeepCoinSocketClient`. Always store the `UpdateSubscription` and unsubscribe when done.

```csharp
using DeepCoin.Net.Clients;

var socketClient = new DeepCoinSocketClient();

var subscription = await socketClient.ExchangeApi.SubscribeToSymbolUpdatesAsync(
    "ETH-USDT",
    update => Console.WriteLine(update.Data.LastPrice));

if (!subscription.Success)
{
    Console.WriteLine(subscription.Error);
    return;
}

Console.ReadLine();
await socketClient.UnsubscribeAsync(subscription.Data);
```

Authenticated streams require a listen key:

```csharp
var restClient = new DeepCoinRestClient(options =>
{
    options.ApiCredentials = new DeepCoinCredentials("API_KEY", "API_SECRET", "API_PASS");
});

var listenKey = await restClient.ExchangeApi.Account.StartUserStreamAsync();
if (!listenKey.Success)
    return;

await socketClient.ExchangeApi.SubscribeToUserDataUpdatesAsync(
    listenKey.Data.ListenKey,
    onOrderMessage: update => Console.WriteLine(update.Data.Length));
```

## Multi-Exchange via CryptoExchange.Net.SharedApis

```csharp
using DeepCoin.Net.Clients;
using CryptoExchange.Net.SharedApis;

ISpotTickerRestClient tickerClient = new DeepCoinRestClient().ExchangeApi.SharedClient;
var symbol = new SharedSymbol(TradingMode.Spot, "ETH", "USDT");

var ticker = await tickerClient.GetSpotTickerAsync(new GetTickerRequest(symbol));
if (!ticker.Success)
{
    Console.WriteLine(ticker.Error);
    return;
}

Console.WriteLine(ticker.Data.LastPrice);
```

Available shared REST interfaces include `IBalanceRestClient`, `IDepositRestClient`, `IKlineRestClient`, `IListenKeyRestClient`, `IOrderBookRestClient`, `IWithdrawalRestClient`, `ISpotTickerRestClient`, `ISpotSymbolRestClient`, `ISpotOrderRestClient`, `ILeverageRestClient`, `IFuturesTickerRestClient`, `IFuturesSymbolRestClient`, `IFuturesOrderRestClient`, and `IBookTickerRestClient`.

Available shared socket interfaces include `IKlineSocketClient`, `ITickerSocketClient`, `ITradeSocketClient`, `IBalanceSocketClient`, `ISpotOrderSocketClient`, `IFuturesOrderSocketClient`, `IUserTradeSocketClient`, and `IPositionSocketClient`.

## Dependency Injection

```csharp
using DeepCoin.Net;
using Microsoft.Extensions.DependencyInjection;

services.AddDeepCoin(options =>
{
    options.ApiCredentials = new DeepCoinCredentials("API_KEY", "API_SECRET", "API_PASS");
});
```

Inject `IDeepCoinRestClient` and `IDeepCoinSocketClient`.

## Environments

```csharp
using DeepCoin.Net;

var live = new DeepCoinRestClient(options => options.Environment = DeepCoinEnvironment.Live);

var custom = DeepCoinEnvironment.CreateCustom(
    "custom",
    "https://example-rest",
    "wss://example-socket");
```

The repository source exposes `DeepCoinEnvironment.Live` and `DeepCoinEnvironment.CreateCustom(...)`; it does not expose a built-in testnet environment.

## Common Pitfalls - AVOID

- Do not use raw `HttpClient` to call DeepCoin endpoints.
- Do not use generic `ApiCredentials`; use `DeepCoinCredentials("key", "secret", "pass")`.
- Do not invent `DeepCoinClient`; use `DeepCoinRestClient`.
- Do not use Binance-style branches such as `SpotApi`, `UsdFuturesApi`, or `CoinFuturesApi`.
- Do not pass `ETHUSDT` to native DeepCoin methods; use `ETH-USDT` or `ETH-USDT-SWAP`.
- Do not read `.Data` without checking `.Success`.
- Do not use `.Result` or `.Wait()`.
- Do not instantiate clients per request.
- Do not subscribe to private WebSocket updates without first requesting a listen key from `Account.StartUserStreamAsync()`.

## Reference

- Full client reference: https://cryptoexchange.jkorf.dev/DeepCoin.Net/
- Examples: `Examples/ai-friendly/`
- Source interfaces: `DeepCoin.Net/Interfaces/Clients/**`
- Source: https://github.com/JKorf/DeepCoin.Net
- NuGet: https://www.nuget.org/packages/DeepCoin.Net
- Discord: https://discord.gg/MSpeEtSY8t

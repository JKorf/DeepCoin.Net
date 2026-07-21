# Copilot Instructions for DeepCoin.Net

This repository is **DeepCoin.Net** - a strongly typed C#/.NET client library for the DeepCoin REST and WebSocket APIs. It is part of the CryptoExchange.Net ecosystem.

When generating code that consumes DeepCoin.Net, follow these conventions:

## Use DeepCoin.Net, not raw HTTP

Never generate `HttpClient` calls to DeepCoin endpoints. Always use `DeepCoinRestClient` or `DeepCoinSocketClient`. This keeps request signing, rate limiting, result handling, and WebSocket reconnect behavior in the library.

## Client setup

```csharp
using DeepCoin.Net;
using DeepCoin.Net.Clients;

var restClient = new DeepCoinRestClient(options =>
{
    options.ApiCredentials = new DeepCoinCredentials("API_KEY", "API_SECRET", "API_PASS");
});
```

For public market data only, no credentials are needed: `new DeepCoinRestClient()`.

## Result handling

Methods return `WebCallResult<T>` (REST) or `CallResult<T>` (WebSocket). Always check `.Success` before reading `.Data`. The error is on `.Error`.

## API structure

- `restClient.ExchangeApi.ExchangeData` - public market data: tickers, symbols, klines, order book, funding rates
- `restClient.ExchangeApi.Account` - balances, bills, leverage, deposit/withdraw history, listen keys
- `restClient.ExchangeApi.Trading` - positions, orders, user trades, order history, TP/SL
- `restClient.ExchangeApi.SharedClient` - CryptoExchange.Net shared REST interfaces
- `socketClient.ExchangeApi` - public and private WebSocket subscriptions
- `socketClient.ExchangeApi.SharedClient` - CryptoExchange.Net shared socket interfaces

## DeepCoin symbol shape

DeepCoin spot symbols use hyphenated names such as `ETH-USDT`. Swap symbols use names such as `ETH-USDT-SWAP`. Use `SymbolType.Spot` for spot and `SymbolType.Swap` for swaps/futures.

## Order placement

Use `DeepCoinCredentials`, not generic `ApiCredentials`. For spot orders use `tradeMode: TradeMode.Spot`. For swap/futures orders use `TradeMode.Cross` or `TradeMode.Isolated` and include `positionSide` when opening or closing a directional position.

## WebSocket pattern

Store the returned `UpdateSubscription` and unsubscribe on shutdown via `socketClient.UnsubscribeAsync(sub.Data)`. Authenticated streams require a listen key from `restClient.ExchangeApi.Account.StartUserStreamAsync()`.

## Cross-exchange

For code that needs to work across multiple exchanges, use `CryptoExchange.Net.SharedApis` interfaces accessed via `.ExchangeApi.SharedClient`.

The shared spot/futures symbol interfaces expose `SpotSymbolCatalog` / `FuturesSymbolCatalog`. Returned symbols include `DisplayName` and base/quote asset type and subtype metadata; use the matching `GetSymbolsRequest` filters when callers need a particular asset classification.

## Avoid

- Legacy or imagined `DeepCoinClient` class; use `DeepCoinRestClient`
- Generic `ApiCredentials`; use `DeepCoinCredentials("key", "secret", "pass")`
- Binance-style branches such as `SpotApi`, `UsdFuturesApi`, or `CoinFuturesApi`
- Non-hyphenated symbols like `ETHUSDT` in native DeepCoin methods
- Synchronous `.Result` / `.Wait()`; use `await`
- Reading `.Data` before checking `.Success`
- Instantiating clients per request; use DI or reuse clients

## Reference

For detailed patterns and pitfalls see `AGENTS.md`, `llms.txt`, and `llms-full.txt` in the repository root, and `Examples/ai-friendly/` for compilable examples.

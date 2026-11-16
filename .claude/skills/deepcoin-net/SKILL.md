---
name: deepcoin-net
description: Implement or review DeepCoin.Net spot and swap REST calls, account and order workflows, leverage and position settings, WebSocket subscriptions and user streams, authentication, Shared API integration, and dependency injection in .NET applications.
---
# DeepCoin.Net

Install with `dotnet add package DeepCoin.Net`.

- Reuse `DeepCoinRestClient` and `DeepCoinSocketClient`, or use dependency injection. Both expose `ExchangeApi`; REST groups are `ExchangeData`, `Account`, and `Trading`.
- Private operations use `DeepCoinCredentials(key, secret, passphrase)`. Public market data needs no credentials.
- Native symbols use `ETH-USDT` for spot and `ETH-USDT-SWAP` for swaps. Select listed instruments and the matching `SymbolType.Spot` or `SymbolType.Swap`.
- REST returns `HttpResult<T>` / `HttpResult`; subscriptions return `WebSocketResult<UpdateSubscription>`. Check `Success` before reading `Data` and handle `Error` on failure. Inspect cancellation response contents after outer success.
- Use `TradeMode.Spot` for spot orders and `Cross` or `Isolated` for swaps. Position side, split/merge mode, leverage, and contract quantities affect derivatives behavior.
- Private `SubscribeToUserDataUpdatesAsync` without a listen key obtains and renews a key automatically using socket credentials. The explicit-key overload requires application-managed key creation/keepalive.
- Retain the concrete socket client and unsubscribe successful subscriptions on shutdown. Native socket candle updates are one minute.
- Use `ExchangeApi.SharedApi` for Shared API V2 and depend on the required capability interface. Listen-key management is internal rather than a public V2 capability.

## References

- Read [API surfaces](references/api-surfaces.md) for routing, Shared API capabilities, symbol catalogs, quantities, and request prerequisites.
- Read [usage](references/usage.md) for authentication, market/account reads, spot and swap orders, public/private subscription cleanup, Shared API V2, and DI snippets.
- Read [operational considerations](references/safety.md) for position modes, cancellation outcomes, submission recovery, and listen-key ownership.

Checkout-only: [repository instructions](../../../AGENTS.md) and [complete examples](../../../Examples/ai-friendly/).

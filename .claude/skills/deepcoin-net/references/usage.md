# Usage examples

Snippets are independent. Those using `client` assume a reused `DeepCoinRestClient`, configured with credentials for private calls. Trading and leverage writes change live state when executed; see [operational considerations](safety.md).

## Imports

```csharp
using System;
using System.Linq;
using System.Threading.Tasks;
using DeepCoin.Net;
using DeepCoin.Net.Clients;
using DeepCoin.Net.Enums;
using DeepCoin.Net.Interfaces.Clients;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.DependencyInjection;
```

## Public spot tickers and results

```csharp
using var client = new DeepCoinRestClient();
var result = await client.ExchangeApi.ExchangeData.GetTickersAsync(SymbolType.Spot);
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
Console.WriteLine(result.Data.FirstOrDefault(x => x.Symbol == "ETH-USDT")?.LastPrice);
```

## Credentials and balances

```csharp
using var client = new DeepCoinRestClient(options =>
{
    options.ApiCredentials = new DeepCoinCredentials("API_KEY", "API_SECRET", "API_PASS");
});
var balances = await client.ExchangeApi.Account.GetBalancesAsync(SymbolType.Spot, asset: "USDT");
if (!balances.Success)
{
    Console.WriteLine(balances.Error);
    return;
}
foreach (var balance in balances.Data)
    Console.WriteLine($"{balance.Asset}: available={balance.AvailableBalance}");
```

## Spot limit order and cancellation

```csharp
var order = await client.ExchangeApi.Trading.PlaceOrderAsync(
    "ETH-USDT", OrderSide.Buy, OrderType.Limit, quantity: 0.1m,
    price: 1000m, tradeMode: TradeMode.Spot);
if (!order.Success)
{
    Console.WriteLine(order.Error);
    return;
}
var cancel = await client.ExchangeApi.Trading.CancelOrderAsync("ETH-USDT", orderId: order.Data.OrderId);
if (!cancel.Success)
    Console.WriteLine(cancel.Error);
```

## Swap positions

```csharp
var positions = await client.ExchangeApi.Trading.GetPositionsAsync(SymbolType.Swap, "ETH-USDT-SWAP");
if (!positions.Success)
{
    Console.WriteLine(positions.Error);
    return;
}
foreach (var position in positions.Data.Where(x => x.Size != 0))
    Console.WriteLine($"{position.PositionId}: {position.PositionSide}, size={position.Size}");
```

## Swap leverage and limit order

Select the intended split/merge position settings, margin mode, contract size, and side before running this example.

```csharp
var leverage = await client.ExchangeApi.Account.SetLeverageAsync(
    "ETH-USDT-SWAP", 5m, TradeMode.Cross, PositionType.Split);
if (!leverage.Success)
{
    Console.WriteLine(leverage.Error);
    return;
}
var order = await client.ExchangeApi.Trading.PlaceOrderAsync(
    "ETH-USDT-SWAP", OrderSide.Buy, OrderType.Limit, quantity: 1m,
    price: 1000m, tradeMode: TradeMode.Cross,
    positionSide: PositionSide.Long, positionType: PositionType.Split);
if (!order.Success)
    Console.WriteLine(order.Error);
```

## Public subscription lifecycle

```csharp
using var socketClient = new DeepCoinSocketClient();
var subscription = await socketClient.ExchangeApi.SubscribeToSymbolUpdatesAsync(
    "ETH-USDT", update => Console.WriteLine(update.Data.LastPrice));
if (!subscription.Success)
{
    Console.WriteLine(subscription.Error);
    return;
}
try
{
    await Task.Delay(TimeSpan.FromSeconds(30));
}
finally
{
    await socketClient.UnsubscribeAsync(subscription.Data);
}
```

## Private subscription with managed listen key

```csharp
using var socketClient = new DeepCoinSocketClient(options =>
{
    options.ApiCredentials = new DeepCoinCredentials("API_KEY", "API_SECRET", "API_PASS");
});
var subscription = await socketClient.ExchangeApi.SubscribeToUserDataUpdatesAsync(
    onOrderMessage: update => Console.WriteLine($"Order updates: {update.Data.Length}"));
if (!subscription.Success)
{
    Console.WriteLine(subscription.Error);
    return;
}
try
{
    await Task.Delay(TimeSpan.FromSeconds(30));
}
finally
{
    await socketClient.UnsubscribeAsync(subscription.Data);
}
```

See [private stream lifecycle](api-surfaces.md#private-stream-lifecycle) for manually managed listen keys.

## Shared API V2

```csharp
IGetTickerRest ticker = client.ExchangeApi.SharedApi;
var symbol = new SharedSymbol(TradingMode.Spot, "ETH", "USDT");
var result = await ticker.GetTickerAsync(new GetTickerRequest(symbol));
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
Console.WriteLine(result.Data.LastPrice);
```

See [API surfaces](api-surfaces.md#shared-api-v2) for catalogs, quantities, and request prerequisites.

## Dependency injection

```csharp
var services = new ServiceCollection();
services.AddDeepCoin(options =>
{
    options.ApiCredentials = new DeepCoinCredentials("API_KEY", "API_SECRET", "API_PASS");
});
using var provider = services.BuildServiceProvider();
var client = provider.GetRequiredService<IDeepCoinRestClient>();
var shared = provider.GetRequiredService<IDeepCoinSharedApiClient>();
```

## Complete examples (checkout-only)

The documentation test compiles these programs without executing exchange calls. Running them can contact DeepCoin and execute illustrated orders, position changes, and leverage settings.

- [Spot quickstart](../../../../Examples/ai-friendly/01-spot-quickstart.cs)
- [Futures](../../../../Examples/ai-friendly/02-futures.cs)
- [WebSockets](../../../../Examples/ai-friendly/03-websocket.cs)
- [Shared API](../../../../Examples/ai-friendly/04-multi-exchange.cs)
- [Error handling](../../../../Examples/ai-friendly/05-error-handling.cs)

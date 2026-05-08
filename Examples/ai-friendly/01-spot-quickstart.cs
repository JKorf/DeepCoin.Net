// 01-spot-quickstart.cs
//
// Demonstrates: client setup, public market data, authenticated balance,
// limit order placement, order status check, cancellation.
//
// Setup:
//   dotnet new console -n SpotQuickstart && cd SpotQuickstart
//   dotnet add package DeepCoin.Net
//   Copy this file content into Program.cs
//   Substitute API_KEY / API_SECRET / API_PASS below
//   dotnet run

using DeepCoin.Net;
using DeepCoin.Net.Clients;
using DeepCoin.Net.Enums;

// ---- 1. PUBLIC CLIENT (no credentials needed for market data) ----
// Reuse this client across the application. Do not create it per request.
var publicClient = new DeepCoinRestClient();

var tickers = await publicClient.ExchangeApi.ExchangeData.GetTickersAsync(SymbolType.Spot);
if (!tickers.Success)
{
    // .Error contains Code, Message, ErrorType, and transient retry hints.
    Console.WriteLine($"Failed to get tickers: {tickers.Error}");
    return;
}

var ethTicker = tickers.Data.FirstOrDefault(x => x.Symbol == "ETH-USDT");
if (ethTicker == null)
{
    Console.WriteLine("ETH-USDT was not returned by DeepCoin spot tickers.");
    return;
}

Console.WriteLine($"ETH/USDT last price: {ethTicker.LastPrice}");
Console.WriteLine($"24h volume: {ethTicker.Volume} ETH");

// ---- 2. AUTHENTICATED CLIENT (for account / trading) ----
// DeepCoin credentials require API key, API secret, and API passphrase.
var tradingClient = new DeepCoinRestClient(options =>
{
    options.ApiCredentials = new DeepCoinCredentials("API_KEY", "API_SECRET", "API_PASS");
});

var balances = await tradingClient.ExchangeApi.Account.GetBalancesAsync(SymbolType.Spot);
if (!balances.Success)
{
    Console.WriteLine($"Failed to get balances: {balances.Error}");
    return;
}

foreach (var balance in balances.Data.Where(b => b.Balance > 0))
{
    Console.WriteLine($"{balance.Asset}: {balance.AvailableBalance} available, {balance.FrozenBalance} frozen");
}

// ---- 3. PLACE A LIMIT BUY ORDER ----
// DeepCoin native spot symbols use hyphens: ETH-USDT, not ETHUSDT.
// Use TradeMode.Spot for spot orders.
var safePrice = ethTicker.LastPrice.HasValue
    ? Math.Round(ethTicker.LastPrice.Value * 0.95m, 2)
    : 2000m;

var order = await tradingClient.ExchangeApi.Trading.PlaceOrderAsync(
    symbol: "ETH-USDT",
    side: OrderSide.Buy,
    orderType: OrderType.Limit,
    quantity: 0.01m,
    price: safePrice,
    tradeMode: TradeMode.Spot);

if (!order.Success)
{
    Console.WriteLine($"Failed to place order: {order.Error}");
    return;
}

Console.WriteLine($"Placed order {order.Data.OrderId} at {safePrice}");

// ---- 4. CHECK ORDER STATUS ----
var status = await tradingClient.ExchangeApi.Trading.GetOpenOrderAsync("ETH-USDT", order.Data.OrderId);
if (status.Success)
{
    Console.WriteLine($"Order status: {status.Data.Status}, filled: {status.Data.QuantityFilled}");
}
else
{
    // If the order filled immediately it may already be in closed order history.
    var closed = await tradingClient.ExchangeApi.Trading.GetClosedOrderAsync("ETH-USDT", order.Data.OrderId);
    if (closed.Success)
        Console.WriteLine($"Closed order status: {closed.Data.Status}");
}

// ---- 5. CANCEL THE ORDER (cleanup for this example) ----
var cancel = await tradingClient.ExchangeApi.Trading.CancelOrderAsync("ETH-USDT", order.Data.OrderId);
if (cancel.Success)
{
    Console.WriteLine($"Cancelled order {order.Data.OrderId}");
}

// Common variations:
//   Market order:              orderType: OrderType.Market, omit price
//   Quote-currency quantity:   add quantityType: QuantityType.QuoteAsset
//   Open orders:               tradingClient.ExchangeApi.Trading.GetOpenOrdersAsync("ETH-USDT")
//   Closed order history:      tradingClient.ExchangeApi.Trading.GetClosedOrdersAsync(SymbolType.Spot, symbol: "ETH-USDT")

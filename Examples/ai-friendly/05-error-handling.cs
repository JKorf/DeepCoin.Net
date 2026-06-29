// 05-error-handling.cs
//
// Demonstrates: HttpResult, WebSocketResult, and ExchangeCallResult patterns,
// retry logic, common error scenarios.
//
// Setup: dotnet add package DeepCoin.Net

using CryptoExchange.Net.Objects;
using DeepCoin.Net;
using DeepCoin.Net.Clients;
using DeepCoin.Net.Enums;

var client = new DeepCoinRestClient(options =>
{
    options.ApiCredentials = new DeepCoinCredentials("API_KEY", "API_SECRET", "API_PASS");
});

// ---- 1. THE BASIC PATTERN ----
// REST methods return HttpResult<T> or HttpResult.
// WebSocket subscriptions return WebSocketResult<UpdateSubscription>.
// Some SharedApis symbol helper methods return ExchangeCallResult<T>.
// .Success is true/false. .Data is the payload, only valid when .Success is true.
// .Error contains structured error info when .Success is false.
// .Error.IsTransient hints if a retry might succeed.

var result = await client.ExchangeApi.ExchangeData.GetTickersAsync(SymbolType.Spot);

if (result.Success)
{
    var ticker = result.Data.FirstOrDefault(x => x.Symbol == "ETH-USDT");
    Console.WriteLine($"Price: {ticker?.LastPrice}");
}
else
{
    Console.WriteLine($"Code:      {result.Error?.Code}");
    Console.WriteLine($"Message:   {result.Error?.Message}");
    Console.WriteLine($"Type:      {result.Error?.ErrorType}");
    Console.WriteLine($"Transient: {result.Error?.IsTransient}");
}

// ---- 2. SIMPLE RETRY WITH BACKOFF ----
// Retry only on transient errors such as network issues, rate limits, or server overload.
// Do not retry validation errors, bad credentials, or insufficient balance.

async Task<HttpResult<T>> WithRetry<T>(
    Func<Task<HttpResult<T>>> call,
    int maxAttempts = 3)
{
    HttpResult<T> last = default!;
    for (var attempt = 1; attempt <= maxAttempts; attempt++)
    {
        last = await call();
        if (last.Success)
            return last;

        if (last.Error?.IsTransient != true)
            return last;

        await Task.Delay(TimeSpan.FromMilliseconds(250 * Math.Pow(2, attempt)));
    }

    return last;
}

var tickers = await WithRetry(
    () => client.ExchangeApi.ExchangeData.GetTickersAsync(SymbolType.Spot));

if (tickers.Success)
    Console.WriteLine($"Retry helper returned {tickers.Data.Length} spot tickers");

// ---- 3. COMMON DEEPCOIN ERROR SCENARIOS ----
//
// Authentication or signature error:
//   API key, secret, passphrase, permissions, environment, or timestamp/signature
//   setup is wrong. Permanent until configuration changes; do not retry indefinitely.
//
// Rate limit / temporary service errors:
//   Usually transient. Retry with backoff when Error.IsTransient is true and keep
//   clients reused so client-side rate limiting has a chance to work.
//
// Invalid symbol or unsupported market:
//   Permanent for that request. Native DeepCoin methods use "ETH-USDT" for spot
//   and "ETH-USDT-SWAP" for swaps/futures. Use GetSymbolsAsync before placing
//   orders or when accepting user-supplied symbols.
//
// Invalid order quantity / price:
//   Permanent until the caller adjusts order size, price, order type, or mode.
//   Query symbol metadata and validate with the fields DeepCoin exposes instead
//   of using ad hoc string truncation.
//
// Insufficient balance:
//   Permanent for that account state. Surface to the caller.
//
// Order not found:
//   May be expected if the order already filled, was canceled, or the wrong
//   symbol/account mode was used.

// ---- 4. ORDER PLACEMENT WITH SYMBOL VALIDATION ----
var symbols = await client.ExchangeApi.ExchangeData.GetSymbolsAsync(SymbolType.Spot, symbol: "ETH-USDT");
if (!symbols.Success)
{
    Console.WriteLine($"Cannot fetch symbol info: {symbols.Error}");
    return;
}

var ethusdt = symbols.Data.FirstOrDefault(s => s.Symbol == "ETH-USDT");
if (ethusdt == null)
{
    Console.WriteLine("ETH-USDT is not available on DeepCoin spot.");
    return;
}

decimal rawQuantity = 0.01m;
decimal rawPrice = 2000m;

// Use symbol metadata and DeepCoin exchange rules to validate the order.
// This example keeps the values explicit so it remains independent of any one
// precision field. Do not assume Binance-style filters exist on DeepCoinSymbol.
var order = await client.ExchangeApi.Trading.PlaceOrderAsync(
    symbol: "ETH-USDT",
    side: OrderSide.Buy,
    orderType: OrderType.Limit,
    quantity: rawQuantity,
    price: rawPrice,
    tradeMode: TradeMode.Spot);

if (!order.Success)
{
    var category = order.Error?.IsTransient == true
        ? "Transient - retry with backoff"
        : "Permanent - surface to user";

    Console.WriteLine($"{category}: {order.Error?.Code} {order.Error?.Message}");
}

// ---- 5. EXCEPTIONS VS ERROR RESULTS ----
// DeepCoin.Net returns exchange, rate limit, and network errors via result.Error.
// Exceptions are generally for misconfiguration, disposal, cancellation, or
// programmer errors. Pass CancellationToken with `ct:` when requests need to be cancelable.

// Common variations:
//   With CancellationToken:    pass `ct: cancellationToken` to any method
//   With timeout per request:  options.RequestTimeout = TimeSpan.FromSeconds(10);
//   Polly integration:         use IsTransient as the retry predicate

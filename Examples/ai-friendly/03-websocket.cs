// 03-websocket.cs
//
// Demonstrates: public WebSocket subscriptions, private user data stream,
// and proper subscription teardown.
//
// Setup: dotnet add package DeepCoin.Net
// Substitute API_KEY / API_SECRET / API_PASS for the private stream section.

using DeepCoin.Net;
using DeepCoin.Net.Clients;

var socketClient = new DeepCoinSocketClient();
const string symbol = "ETH-USDT";

// Subscription methods return WebSocketResult<UpdateSubscription>.
// ---- 1. PUBLIC TICKER STREAM ----
var tickerSub = await socketClient.ExchangeApi.SubscribeToSymbolUpdatesAsync(
    symbol,
    update =>
    {
        Console.WriteLine($"{update.Data.Symbol}: {update.Data.LastPrice}");
    });

if (!tickerSub.Success)
{
    Console.WriteLine($"Ticker subscribe failed: {tickerSub.Error}");
    return;
}

// ---- 2. PUBLIC TRADE STREAM ----
var tradeSub = await socketClient.ExchangeApi.SubscribeToTradeUpdatesAsync(
    symbol,
    update =>
    {
        Console.WriteLine($"Trade {update.Data.Price} x {update.Data.Quantity}");
    });

if (!tradeSub.Success)
{
    await socketClient.UnsubscribeAsync(tickerSub.Data);
    Console.WriteLine($"Trade subscribe failed: {tradeSub.Error}");
    return;
}

// ---- 3. ONE-MINUTE KLINE STREAM ----
// The native DeepCoin socket method subscribes to one-minute kline updates.
var klineSub = await socketClient.ExchangeApi.SubscribeToKlineUpdatesAsync(
    symbol,
    update =>
    {
        Console.WriteLine($"Kline close: {update.Data.ClosePrice}");
    });

if (!klineSub.Success)
{
    await socketClient.UnsubscribeAsync(tickerSub.Data);
    await socketClient.UnsubscribeAsync(tradeSub.Data);
    Console.WriteLine($"Kline subscribe failed: {klineSub.Error}");
    return;
}

// ---- 4. ORDER BOOK STREAM ----
var bookSub = await socketClient.ExchangeApi.SubscribeToOrderBookUpdatesAsync(
    symbol,
    update =>
    {
        var bestBid = update.Data.Bids.FirstOrDefault();
        var bestAsk = update.Data.Asks.FirstOrDefault();
        Console.WriteLine($"Book seq {update.Data.SequenceNumber}: bid {bestBid?.Price}, ask {bestAsk?.Price}");
    });

if (!bookSub.Success)
{
    await socketClient.UnsubscribeAsync(tickerSub.Data);
    await socketClient.UnsubscribeAsync(tradeSub.Data);
    await socketClient.UnsubscribeAsync(klineSub.Data);
    Console.WriteLine($"Order book subscribe failed: {bookSub.Error}");
    return;
}

// ---- 5. PRIVATE USER DATA STREAM ----
// Private streams require credentials on REST to acquire a listen key, and
// credentials on the socket client for authenticated socket behavior.
var restClient = new DeepCoinRestClient(options =>
{
    options.ApiCredentials = new DeepCoinCredentials("API_KEY", "API_SECRET", "API_PASS");
});

var privateSocketClient = new DeepCoinSocketClient(options =>
{
    options.ApiCredentials = new DeepCoinCredentials("API_KEY", "API_SECRET", "API_PASS");
});

var listenKey = await restClient.ExchangeApi.Account.StartUserStreamAsync();
if (listenKey.Success)
{
    var userSub = await privateSocketClient.ExchangeApi.SubscribeToUserDataUpdatesAsync(
        listenKey.Data.ListenKey,
        onOrderMessage: update => Console.WriteLine($"Order updates: {update.Data.Length}"),
        onBalanceMessage: update => Console.WriteLine($"Balance updates: {update.Data.Length}"),
        onPositionMessage: update => Console.WriteLine($"Position updates: {update.Data.Length}"));

    if (userSub.Success)
    {
        Console.WriteLine("Private user stream subscribed.");
        await privateSocketClient.UnsubscribeAsync(userSub.Data);
    }
}

Console.WriteLine("Press Enter to exit public subscriptions");
Console.ReadLine();

await socketClient.UnsubscribeAsync(tickerSub.Data);
await socketClient.UnsubscribeAsync(tradeSub.Data);
await socketClient.UnsubscribeAsync(klineSub.Data);
await socketClient.UnsubscribeAsync(bookSub.Data);

// Common variations:
//   Swap symbol streams:       use "ETH-USDT-SWAP"
//   Keep listen key alive:     restClient.ExchangeApi.Account.KeepAliveUserStreamAsync(listenKey)
//   Local order book:          use DeepCoinSymbolOrderBook or IDeepCoinOrderBookFactory

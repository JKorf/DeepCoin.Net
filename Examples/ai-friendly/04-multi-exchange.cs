// 04-multi-exchange.cs
//
// Demonstrates: writing exchange-agnostic code using CryptoExchange.Net.SharedApis.
// Same code works against DeepCoin, Binance, OKX, Bybit, Kraken, and other
// exchanges from the CryptoExchange.Net family.
//
// Setup:
//   dotnet add package DeepCoin.Net
//   dotnet add package Binance.Net   // optional, for a Binance comparison
//   dotnet add package JK.OKX.Net    // optional, for an OKX comparison

using CryptoExchange.Net.SharedApis;
using DeepCoin.Net.Clients;

// ---- THE PATTERN ----
// Each exchange client exposes a `.SharedClient` property on its API surfaces.
// DeepCoin has one ExchangeApi root. SharedClient implements interfaces like
// ISpotTickerRestClient, IFuturesOrderRestClient, IBalanceRestClient, etc.

var restClient = new DeepCoinRestClient();
ISpotTickerRestClient deepCoinShared = restClient.ExchangeApi.SharedClient;
var capabilities = restClient.ExchangeApi.SharedClient.Discover();
Console.WriteLine($"Shared REST features: {capabilities.Features.Count(x => x.Supported)}");

// To add Binance or OKX, install the package and:
//   ISpotTickerRestClient binanceShared = new BinanceRestClient().SpotApi.SharedClient;
//   ISpotTickerRestClient okxShared     = new OKXRestClient().UnifiedApi.SharedClient;

// Common symbol type - handles formatting differences between exchanges automatically.
// DeepCoin native methods use "ETH-USDT"; shared APIs use base/quote assets.
var ethusdt = new SharedSymbol(TradingMode.Spot, "ETH", "USDT");

await PrintTicker(deepCoinShared, ethusdt);
// await PrintTicker(binanceShared, ethusdt);
// await PrintTicker(okxShared, ethusdt);

// ---- AGNOSTIC METHOD - works against any exchange ----
async Task PrintTicker(ISpotTickerRestClient client, SharedSymbol symbol)
{
    var result = await client.GetSpotTickerAsync(new GetTickerRequest(symbol));
    if (!result.Success)
    {
        Console.WriteLine($"[{client.Exchange}] Failed: {result.Error}");
        return;
    }

    Console.WriteLine($"[{client.Exchange}] {result.Data.Symbol}: {result.Data.LastPrice}");
}

// ---- WHY THIS MATTERS ----
// You can build:
//   - Multi-exchange arbitrage scanners
//   - Best-execution routers
//   - Unified portfolio dashboards
//   - Exchange comparison tools
// without writing per-exchange branches everywhere.

// ---- AVAILABLE SHARED INTERFACES ----
// DeepCoin REST includes:
//   IBalanceRestClient, IDepositRestClient, IKlineRestClient
//   IListenKeyRestClient, IOrderBookRestClient, IWithdrawalRestClient
//   ISpotTickerRestClient, ISpotSymbolRestClient, ISpotOrderRestClient
//   ILeverageRestClient, IFuturesTickerRestClient, IFuturesSymbolRestClient
//   IFuturesOrderRestClient, IBookTickerRestClient
// DeepCoin WebSocket includes:
//   IKlineSocketClient, ITickerSocketClient, ITradeSocketClient
//   IBalanceSocketClient, ISpotOrderSocketClient, IFuturesOrderSocketClient
//   IUserTradeSocketClient, IPositionSocketClient

// ---- WEBSOCKET EXAMPLE - SHARED SUBSCRIPTION ----
var deepCoinSocket = new DeepCoinSocketClient();
ITickerSocketClient tickerSocket = deepCoinSocket.ExchangeApi.SharedClient;

var sub = await tickerSocket.SubscribeToTickerUpdatesAsync(
    new SubscribeTickerRequest(ethusdt),
    update => Console.WriteLine($"[{tickerSocket.Exchange}] {update.Data.Symbol}: {update.Data.LastPrice}"));

if (!sub.Success)
{
    Console.WriteLine($"Subscribe failed: {sub.Error}");
    return;
}

Console.WriteLine("Press Enter to exit");
Console.ReadLine();

// The shared ticker interface does not expose UnsubscribeAsync; keep the concrete client.
await deepCoinSocket.UnsubscribeAsync(sub.Data);

// Common variations:
//   Multi-exchange arbitrage:  loop over List<ISpotTickerRestClient>, find max bid / min ask
//   Cross-exchange orderbook:  IOrderBookSocketClient on each exchange, merge into composite book
//   Best execution:            ISpotOrderRestClient on N exchanges, route by liquidity
//   Futures comparison:        use IFuturesTickerRestClient from client.ExchangeApi.SharedClient

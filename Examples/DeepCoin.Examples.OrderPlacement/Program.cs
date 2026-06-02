using DeepCoin.Net;
using DeepCoin.Net.Clients;
using DeepCoin.Net.Enums;

const string spotSymbol = "BTC-USDT";
const string futuresSymbol = "ETH-USDT-SWAP";

// Replace with valid credentials or order placement will always fail
var apiKey = "KEY";
var apiSecret = "SECRET";
var passPhrase = "PASS";

Console.WriteLine("DeepCoin.Net order placement example");
Console.WriteLine();
Console.WriteLine("This example can place real orders when valid credentials are configured.");
Console.WriteLine();

var client = new DeepCoinRestClient(options =>
{
    options.ApiCredentials = new DeepCoinCredentials()
        .WithHMAC(apiKey, apiSecret, passPhrase);
});

await PlaceSpotLimitOrderAsync(client);
Console.WriteLine();
await PlaceFuturesReduceOnlyOrderExampleAsync(client);

static async Task PlaceSpotLimitOrderAsync(DeepCoinRestClient client)
{
    Console.WriteLine($"Placing spot limit buy order for {spotSymbol}...");

    var tickers = await client.ExchangeApi.ExchangeData.GetTickersAsync(SymbolType.Spot);
    if (!tickers.Success)
    {
        Console.WriteLine($"Failed to get spot tickers: {tickers.Error}");
        return;
    }

    var ticker = tickers.Data.SingleOrDefault(x => x.Symbol == spotSymbol);
    if (ticker?.LastPrice == null)
    {
        Console.WriteLine($"Failed to find a spot ticker price for {spotSymbol}");
        return;
    }

    var safePrice = Math.Round(ticker.LastPrice.Value * 0.95m, 2);
    var order = await client.ExchangeApi.Trading.PlaceOrderAsync(
        symbol: spotSymbol,
        side: OrderSide.Buy,
        orderType: OrderType.Limit,
        quantity: 0.001m,
        price: safePrice,
        tradeMode: TradeMode.Spot);

    if (!order.Success)
    {
        Console.WriteLine($"Failed to place spot order: {order.Error}");
        return;
    }

    Console.WriteLine($"Placed spot order {order.Data.OrderId}");

    var orderStatus = await client.ExchangeApi.Trading.GetOpenOrderAsync(spotSymbol, order.Data.OrderId);
    if (orderStatus.Success)
        Console.WriteLine($"Spot order status: {orderStatus.Data.Status}, filled: {orderStatus.Data.QuantityFilled}");
    else
        Console.WriteLine($"Failed to query spot order: {orderStatus.Error}");

    var cancel = await client.ExchangeApi.Trading.CancelOrderAsync(spotSymbol, order.Data.OrderId);
    Console.WriteLine(cancel.Success
        ? $"Cancelled spot order {order.Data.OrderId}"
        : $"Failed to cancel spot order: {cancel.Error}");
}

static async Task PlaceFuturesReduceOnlyOrderExampleAsync(DeepCoinRestClient client)
{
    Console.WriteLine($"Placing swap reduce-only limit sell order for {futuresSymbol}...");

    var tickers = await client.ExchangeApi.ExchangeData.GetTickersAsync(SymbolType.Swap);
    if (!tickers.Success)
    {
        Console.WriteLine($"Failed to get swap tickers: {tickers.Error}");
        return;
    }

    var ticker = tickers.Data.SingleOrDefault(x => x.Symbol == futuresSymbol);
    if (ticker?.LastPrice == null)
    {
        Console.WriteLine($"Failed to find a swap ticker price for {futuresSymbol}");
        return;
    }

    var safePrice = Math.Round(ticker.LastPrice.Value * 1.05m, 2);
    var order = await client.ExchangeApi.Trading.PlaceOrderAsync(
        symbol: futuresSymbol,
        side: OrderSide.Sell,
        orderType: OrderType.Limit,
        quantity: 0.01m,
        price: safePrice,
        tradeMode: TradeMode.Cross,
        reduceOnly: true);

    if (!order.Success)
    {
        Console.WriteLine($"Failed to place swap order: {order.Error}");
        return;
    }

    Console.WriteLine($"Placed swap order {order.Data.OrderId}");

    var orderStatus = await client.ExchangeApi.Trading.GetOpenOrderAsync(futuresSymbol, order.Data.OrderId);
    if (orderStatus.Success)
        Console.WriteLine($"Swap order status: {orderStatus.Data.Status}, filled: {orderStatus.Data.QuantityFilled}");
    else
        Console.WriteLine($"Failed to query swap order: {orderStatus.Error}");

    var cancel = await client.ExchangeApi.Trading.CancelOrderAsync(futuresSymbol, order.Data.OrderId);
    Console.WriteLine(cancel.Success
        ? $"Cancelled swap order {order.Data.OrderId}"
        : $"Failed to cancel swap order: {cancel.Error}");
}

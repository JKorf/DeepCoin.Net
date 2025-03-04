
using DeepCoin.Net.Clients;

// REST
var restClient = new DeepCoinRestClient();
var tickers = await restClient.ExchangeApi.ExchangeData.GetTickersAsync(DeepCoin.Net.Enums.SymbolType.Swap);
var ticker = tickers.Data.SingleOrDefault(x => x.Symbol == "ETH-USDT-SWAP");
Console.WriteLine($"Rest client ticker price for ETH-USDT-SWAP: {ticker?.LastPrice}");

Console.WriteLine();
Console.WriteLine("Press enter to start websocket subscription");
Console.ReadLine();

// Websocket
var socketClient = new DeepCoinSocketClient();
var subscription = await socketClient.ExchangeApi.SubscribeToSymbolUpdatesAsync("ETH-USDT-SWAP", update =>
{
    Console.WriteLine($"Websocket client ticker price for ETH-USDT-SWAP: {update.Data.LastPrice}");
});

Console.ReadLine();

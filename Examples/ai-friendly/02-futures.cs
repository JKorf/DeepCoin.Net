// 02-futures.cs
//
// Demonstrates: DeepCoin swap/futures - set leverage, place market order,
// retrieve open position, close position.
//
// Setup: dotnet add package DeepCoin.Net
// Substitute API_KEY / API_SECRET / API_PASS. The API key must have trading enabled.

using DeepCoin.Net;
using DeepCoin.Net.Clients;
using DeepCoin.Net.Enums;

var client = new DeepCoinRestClient(options =>
{
    options.ApiCredentials = new DeepCoinCredentials("API_KEY", "API_SECRET", "API_PASS");
});

const string symbol = "ETH-USDT-SWAP";

// ---- 1. SET LEVERAGE ----
// DeepCoin swap/futures settings are selected through symbol, TradeMode, and PositionType.
var leverage = await client.ExchangeApi.Account.SetLeverageAsync(
    symbol,
    leverage: 5m,
    tradeMode: TradeMode.Cross,
    positionType: PositionType.Split);

if (!leverage.Success)
{
    Console.WriteLine($"Failed to set leverage: {leverage.Error}");
    return;
}

Console.WriteLine($"Leverage set to {leverage.Data.Leverage}x for {symbol}");

// ---- 2. PLACE MARKET ORDER (open long position) ----
// Use PositionSide.Long or PositionSide.Short for directional swap/futures orders.
var openOrder = await client.ExchangeApi.Trading.PlaceOrderAsync(
    symbol: symbol,
    side: OrderSide.Buy,
    orderType: OrderType.Market,
    quantity: 0.1m,
    tradeMode: TradeMode.Cross,
    positionSide: PositionSide.Long);

if (!openOrder.Success)
{
    Console.WriteLine($"Failed to open position: {openOrder.Error}");
    return;
}

Console.WriteLine($"Opened position via order {openOrder.Data.OrderId}");

// ---- 3. GET CURRENT POSITION ----
var positions = await client.ExchangeApi.Trading.GetPositionsAsync(SymbolType.Swap, symbol);
if (!positions.Success)
{
    Console.WriteLine($"Failed to get positions: {positions.Error}");
    return;
}

var position = positions.Data.FirstOrDefault(p => p.Size != 0 && p.PositionSide == PositionSide.Long);
if (position == null)
{
    Console.WriteLine("No open long position found (the order may not have filled yet).");
    return;
}

Console.WriteLine($"Position: {position.Size} {symbol} at avg {position.AveragePrice}");
Console.WriteLine($"Liquidation price: {position.LiquidationPrice}");
Console.WriteLine($"Position id: {position.PositionId}");

// ---- 4. CLOSE THE POSITION ----
// Opposite side, same position side, reduceOnly=true to avoid accidentally increasing exposure.
var closeOrder = await client.ExchangeApi.Trading.PlaceOrderAsync(
    symbol: symbol,
    side: OrderSide.Sell,
    orderType: OrderType.Market,
    quantity: Math.Abs(position.Size),
    tradeMode: TradeMode.Cross,
    positionSide: PositionSide.Long,
    closePosId: position.PositionId,
    reduceOnly: true);

if (closeOrder.Success)
{
    Console.WriteLine($"Closed position via order {closeOrder.Data.OrderId}");
}

// Common variations:
//   Limit order:        orderType: OrderType.Limit, add price
//   Isolated margin:    tradeMode: TradeMode.Isolated
//   Short position:     side: OrderSide.Sell, positionSide: PositionSide.Short
//   TP/SL on order:     add tpTriggerPrice and/or slTriggerPrice to PlaceOrderAsync
//   Funding rates:      client.ExchangeApi.ExchangeData.GetFundingRateAsync(ProductGroup.USDTMargined, symbol)

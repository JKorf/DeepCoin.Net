using DeepCoin.Net.Interfaces.Clients;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the DeepCoin services
builder.Services.AddDeepCoin();

// OR to provide API credentials for accessing private endpoints, or setting other options:
/*
builder.Services.AddDeepCoin(options =>
{
    options.ApiCredentials = new DeepCoinApiCredentials("<APIKEY>", "<APISECRET>", "<APIPASS>");
    options.Rest.RequestTimeout = TimeSpan.FromSeconds(5);
});
*/

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

// Map the endpoint and inject the rest client
app.MapGet("/{Symbol}", async ([FromServices] IDeepCoinRestClient client, string symbol) =>
{
    var result = await client.ExchangeApi.ExchangeData.GetTickersAsync(DeepCoin.Net.Enums.SymbolType.Spot);
    var info = result.Data.SingleOrDefault(x => x.Symbol == symbol);
    return info?.LastPrice;
})
.WithOpenApi();


app.MapGet("/Balances", async ([FromServices] IDeepCoinRestClient client) =>
{
    var result = await client.ExchangeApi.Account.GetBalancesAsync(DeepCoin.Net.Enums.SymbolType.Spot);
    return (object)(result.Success ? result.Data : result.Error!);
})
.WithOpenApi();

app.Run();
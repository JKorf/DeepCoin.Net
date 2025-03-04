using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using DeepCoin.Net.Clients;
using DeepCoin.Net.Objects.Options;
using DeepCoin.Net.Objects;

namespace DeepCoin.Net.UnitTests
{
    [NonParallelizable]
    public class DeepCoinRestIntegrationTests : RestIntegrationTest<DeepCoinRestClient>
    {
        public override bool Run { get; set; } = false;

        public override DeepCoinRestClient GetClient(ILoggerFactory loggerFactory)
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");
            var pass = Environment.GetEnvironmentVariable("APIPASS");

            Authenticated = key != null && sec != null;
            return new DeepCoinRestClient(null, loggerFactory, Options.Create(new DeepCoinRestOptions
            {
                AutoTimestamp = false,
                OutputOriginalData = true,
                ApiCredentials = Authenticated ? new DeepCoinApiCredentials(key, sec, pass) : null
            }));
        }

        [Test]
        public async Task TestErrorResponseParsing()
        {
            if (!ShouldRun())
                return;

            var result = await CreateClient().ExchangeApi.ExchangeData.GetOrderBookAsync("TSTTST", default);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Error.Code, Is.EqualTo(50));
        }

        [Test]
        public async Task TestAccount()
        {
            await RunAndCheckResult(client => client.ExchangeApi.Account.GetBalancesAsync(Enums.SymbolType.Spot, default, default), true);
            await RunAndCheckResult(client => client.ExchangeApi.Account.GetBalancesAsync(Enums.SymbolType.Swap, default, default), true);
            await RunAndCheckResult(client => client.ExchangeApi.Account.GetBillsAsync(Enums.SymbolType.Spot, default, default, default, default, default, default), true);
            await RunAndCheckResult(client => client.ExchangeApi.Account.GetBillsAsync(Enums.SymbolType.Swap, default, default, default, default, default, default), true);
            await RunAndCheckResult(client => client.ExchangeApi.Account.GetDepositHistoryAsync(default, default, default, default, default, default, default), true);
            await RunAndCheckResult(client => client.ExchangeApi.Account.GetWithdrawHistoryAsync(default, default, default, default, default, default, default), true);
        }

        [Test]
        public async Task TestExchangeData()
        {
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetSymbolsAsync(Enums.SymbolType.Spot, default, default, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetSymbolsAsync(Enums.SymbolType.Swap, default, default, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetTickersAsync(Enums.SymbolType.Spot, default, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetTickersAsync(Enums.SymbolType.Swap, default, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetKlinesAsync("ETH-USDT", Enums.KlineInterval.OneDay, default, default, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetOrderBookAsync("ETH-USDT", 10, default), false);
            await RunAndCheckResult(client => client.ExchangeApi.ExchangeData.GetFundingRateAsync(Enums.ProductGroup.USDTMargined, default, default), true);
        }

        [Test]
        public async Task TestTrading()
        {
            await RunAndCheckResult(client => client.ExchangeApi.Trading.GetPositionsAsync(Enums.SymbolType.Swap, default, default), true);
            await RunAndCheckResult(client => client.ExchangeApi.Trading.GetUserTradesAsync(Enums.SymbolType.Spot, default, default, default, default, default, default, default, default), true);
            await RunAndCheckResult(client => client.ExchangeApi.Trading.GetOpenOrdersAsync("ETH-USDT", default, default, default, default), true);
            await RunAndCheckResult(client => client.ExchangeApi.Trading.GetClosedOrdersAsync(Enums.SymbolType.Spot, "ETH-USDT", default, default, default, default, default, default, default), true);
        }
    }
}

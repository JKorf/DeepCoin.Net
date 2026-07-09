using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using DeepCoin.Net.Clients;
using DeepCoin.Net.Objects.Options;
using DeepCoin.Net.Objects;
using CryptoExchange.Net.Authentication;
using DeepCoin.Net.SymbolOrderBooks;
using CryptoExchange.Net.Objects.Errors;
using System.Collections.Generic;

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
                ApiCredentials = Authenticated ? new DeepCoinCredentials(key, sec, pass) : null
            }));
        }

        [Test]
        public async Task TestErrorResponseParsing()
        {
            if (!ShouldRun())
                return;

            var result = await CreateClient().ExchangeApi.ExchangeData.GetOrderBookAsync("TSTTST", default);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Error.ErrorCode, Is.EqualTo("50"));
            Assert.That(result.Error.ErrorType, Is.EqualTo(ErrorType.UnknownSymbol));
        }

        [Test]
        public async Task TestAccount()
        {
            var warnings = new List<Exception>();
            await RunAndCheckResult(warnings, client => client.ExchangeApi.Account.GetBalancesAsync(Enums.SymbolType.Spot, default, default), true, "data");
            await RunAndCheckResult(warnings, client => client.ExchangeApi.Account.GetBalancesAsync(Enums.SymbolType.Swap, default, default), true, "data");
            await RunAndCheckResult(warnings, client => client.ExchangeApi.Account.GetBillsAsync(Enums.SymbolType.Spot, default, default, default, default, default, default), true, "data");
            await RunAndCheckResult(warnings, client => client.ExchangeApi.Account.GetBillsAsync(Enums.SymbolType.Swap, default, default, default, default, default, default), true, "data");
            await RunAndCheckResult(warnings, client => client.ExchangeApi.Account.GetDepositHistoryAsync(default, default, default, default, default, default, default), true, "data");
            await RunAndCheckResult(warnings, client => client.ExchangeApi.Account.GetWithdrawHistoryAsync(default, default, default, default, default, default, default), true, "data");
            await RunAndCheckResult(warnings, client => client.ExchangeApi.Account.GetTradeFeeAsync(Enums.SymbolType.Spot, default, default, default), false, "data");
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }

        [Test]
        public async Task TestExchangeData()
        {
            var warnings = new List<Exception>();
            await RunAndCheckResult(warnings, client => client.ExchangeApi.ExchangeData.GetSymbolsAsync(Enums.SymbolType.Spot, default, default, default), false, "data");
            await RunAndCheckResult(warnings, client => client.ExchangeApi.ExchangeData.GetSymbolsAsync(Enums.SymbolType.Swap, default, default, default), false, "data");
            await RunAndCheckResult(warnings, client => client.ExchangeApi.ExchangeData.GetTickersAsync(Enums.SymbolType.Spot, default, default), false, "data", ignoreProperties: ["sodUtc0", "sodUtc8"]);
            await RunAndCheckResult(warnings, client => client.ExchangeApi.ExchangeData.GetTickersAsync(Enums.SymbolType.Swap, default, default), false, "data", ignoreProperties: ["sodUtc0", "sodUtc8"]);
            await RunAndCheckResult(warnings, client => client.ExchangeApi.ExchangeData.GetKlinesAsync("ETH-USDT", Enums.KlineInterval.OneDay, default, default, default), false, "data");
            await RunAndCheckResult(warnings, client => client.ExchangeApi.ExchangeData.GetOrderBookAsync("ETH-USDT", 10, default), false, "data");
            await RunAndCheckResult(warnings, client => client.ExchangeApi.ExchangeData.GetFundingRateAsync(Enums.ProductGroup.USDTMargined, default, default), true, "data");
            await RunAndCheckResult(warnings, client => client.ExchangeApi.ExchangeData.GetMarkPricesAsync(Enums.SymbolType.Swap, default, default, default), true, "data");
            await RunAndCheckResult(warnings, client => client.ExchangeApi.ExchangeData.GetOpenInterestAndVolumeAsync("ETH-USDT-SWAP", default, default, default, default, default), true, "data");
            await RunAndCheckResult(warnings, client => client.ExchangeApi.ExchangeData.GetLongShortRatioAsync("ETH-USDT-SWAP", default, default, default, default, default), true, "data");
            await RunAndCheckResult(warnings, client => client.ExchangeApi.ExchangeData.GetTakerBuySellVolumeAsync("ETH-USDT-SWAP", default, default, default, default, default), true, "data");
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }

        [Test]
        public async Task TestTrading()
        {
            var warnings = new List<Exception>();
            await RunAndCheckResult(warnings, client => client.ExchangeApi.Trading.GetPositionsAsync(Enums.SymbolType.Swap, default, default), true, "data");
            await RunAndCheckResult(warnings, client => client.ExchangeApi.Trading.GetUserTradesAsync(Enums.SymbolType.Spot, default, default, default, default, default, default, default, default), true, "data");
            await RunAndCheckResult(warnings, client => client.ExchangeApi.Trading.GetOpenOrdersAsync("ETH-USDT", default, default, default, default), true, "data");
            await RunAndCheckResult(warnings, client => client.ExchangeApi.Trading.GetClosedOrdersAsync(Enums.SymbolType.Spot, "ETH-USDT", default, default, default, default, default, default, default), true, "data");
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }

        [Test]
        public async Task TestOrderBooks()
        {
            await TestOrderBook(new DeepCoinSymbolOrderBook("ETH-USDT"));
        }
    }
}

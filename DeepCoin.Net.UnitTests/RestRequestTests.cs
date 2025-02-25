using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeepCoin.Net.Clients;
using DeepCoin.Net.Enums;
using NUnit.Framework.Constraints;
using DeepCoin.Net.Objects;
using System.Linq;

namespace DeepCoin.Net.UnitTests
{
    [TestFixture]
    public class RestRequestTests
    {
        [Test]
        public async Task ValidateAccountCalls()
        {
            var client = new DeepCoinRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new DeepCoinApiCredentials("123", "456", "789");
            });
            var tester = new RestRequestValidator<DeepCoinRestClient>(client, "Endpoints/Exchange/Account", "https://api.deepcoin.com", IsAuthenticated, nestedPropertyForCompare: "data", stjCompare: true);
            await tester.ValidateAsync(client => client.ExchangeApi.Account.GetBalancesAsync(SymbolType.Spot), "GetBalances", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.Account.GetBillsAsync(SymbolType.Spot), "GetBills", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.Account.SetLeverageAsync("123", 0.1m, MarginMode.Cross, PositionType.Merge), "SetLeverage", nestedJsonProperty: "data");
        }

        [Test]
        public async Task ValidateExchangeDataCalls()
        {
            var client = new DeepCoinRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new DeepCoinApiCredentials("123", "456", "789");
            });
            var tester = new RestRequestValidator<DeepCoinRestClient>(client, "Endpoints/Exchange/ExchangeData", "https://api.deepcoin.com", IsAuthenticated, nestedPropertyForCompare: "data", stjCompare: true);
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetTickersAsync(Enums.SymbolType.Spot), "GetTickers", ignoreProperties: ["lastSz", "sodUtc0", "sodUtc8"]);
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetSymbolsAsync(SymbolType.Swap), "GetSymbols", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetKlinesAsync("123", KlineInterval.FiveMinutes), "GetKlines", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetOrderBookAsync("123"), "GetOrderBook", nestedJsonProperty: "data");

        }

        [Test]
        public async Task ValidateTradingCalls()
        {
            var client = new DeepCoinRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new DeepCoinApiCredentials("123", "456", "789");
            });
            var tester = new RestRequestValidator<DeepCoinRestClient>(client, "Endpoints/Exchange/Account", "https://api.deepcoin.com", IsAuthenticated, nestedPropertyForCompare: "data", stjCompare: true);
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.GetPositionsAsync(SymbolType.Swap), "GetPositions", nestedJsonProperty: "data");
        }

        private bool IsAuthenticated(WebCallResult result)
        {
            return result.RequestHeaders.Any(x => x.Key == "DC-ACCESS-SIGN");
        }
    }
}

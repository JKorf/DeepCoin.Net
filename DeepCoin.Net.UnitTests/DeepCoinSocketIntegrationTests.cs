using DeepCoin.Net.Clients;
using DeepCoin.Net.Objects.Models;
using DeepCoin.Net.Objects.Options;
using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DeepCoin.Net.UnitTests
{
    [NonParallelizable]
    internal class DeepCoinSocketIntegrationTests : SocketIntegrationTest<DeepCoinSocketClient>
    {
        public override bool Run { get; set; } = false;

        public DeepCoinSocketIntegrationTests()
        {
        }

        public override DeepCoinSocketClient GetClient(ILoggerFactory loggerFactory, bool useUpdatedDeserialization)
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");
            var pass = Environment.GetEnvironmentVariable("APIPASS");

            Authenticated = key != null && sec != null;
            return new DeepCoinSocketClient(Options.Create(new DeepCoinSocketOptions
            {
                OutputOriginalData = true,
                UseUpdatedDeserialization = useUpdatedDeserialization,
                ApiCredentials = Authenticated ? new CryptoExchange.Net.Authentication.ApiCredentials(key, sec, pass) : null
            }), loggerFactory);
        }

        private DeepCoinRestClient GetRestClient()
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");
            var pass = Environment.GetEnvironmentVariable("APIPASS");

            Authenticated = key != null && sec != null;
            return new DeepCoinRestClient(x =>
            {
                x.ApiCredentials = Authenticated ? new CryptoExchange.Net.Authentication.ApiCredentials(key, sec, pass) : null;
            });
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task TestSubscriptions(bool useUpdatedDeserialization)
        {
            var listenKey = await GetRestClient().ExchangeApi.Account.StartUserStreamAsync();
            await RunAndCheckUpdate<DeepCoinTicker>(useUpdatedDeserialization , (client, updateHandler) => client.ExchangeApi.SubscribeToUserDataUpdatesAsync(listenKey.Data.ListenKey, default, default, default, default, default, default, default), false, true);
            await RunAndCheckUpdate<DeepCoinSymbolUpdate>(useUpdatedDeserialization , (client, updateHandler) => client.ExchangeApi.SubscribeToSymbolUpdatesAsync("ETH-USDT", updateHandler, default), true, false);
        } 
    }
}

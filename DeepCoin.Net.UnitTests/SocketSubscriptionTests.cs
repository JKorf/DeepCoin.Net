using CryptoExchange.Net.Testing;
using NUnit.Framework;
using System.Threading.Tasks;
using DeepCoin.Net.Clients;
using DeepCoin.Net.Objects;
using DeepCoin.Net.Objects.Models;

namespace DeepCoin.Net.UnitTests
{
    [TestFixture]
    public class SocketSubscriptionTests
    {
        [Test]
        public async Task ValidateSpotExchangeDataSubscriptions()
        {
            var client = new DeepCoinSocketClient(opts =>
            {
                opts.ApiCredentials = new DeepCoinApiCredentials("123", "456", "789");
            });
            var tester = new SocketSubscriptionValidator<DeepCoinSocketClient>(client, "Subscriptions/Exchange", "wss://stream.deepcoin.com", stjCompare: true);
            // Current implementation not able to test this, requires a nested id property to be replaced during validation which is not supported
            // Fix after updating testing to System.Text.Json.
            //await tester.ValidateAsync<DeepCoinSymbolUpdate>((client, handler) => client.ExchangeApi.SubscribeToSymbolUpdatesAsync("ETH-USDT", handler), "Symbol");
        }
    }
}

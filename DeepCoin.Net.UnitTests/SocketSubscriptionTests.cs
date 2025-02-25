using CryptoExchange.Net.Testing;
using NUnit.Framework;
using System.Threading.Tasks;
using DeepCoin.Net.Clients;
using DeepCoin.Net.Objects.Models;
using DeepCoin.Net.Objects;

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
            var tester = new SocketSubscriptionValidator<DeepCoinSocketClient>(client, "Subscriptions/Spot", "XXX", stjCompare: true);
            //await tester.ValidateAsync<DeepCoinModel>((client, handler) => client.SpotApi.SubscribeToXXXUpdatesAsync(handler), "XXX");
        }
    }
}

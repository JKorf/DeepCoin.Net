using CryptoExchange.Net.Testing;
using NUnit.Framework;
using System.Threading.Tasks;
using DeepCoin.Net.Clients;
using DeepCoin.Net.Objects;
using DeepCoin.Net.Objects.Models;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using CryptoExchange.Net.Objects;

namespace DeepCoin.Net.UnitTests
{
    [TestFixture]
    public class SocketSubscriptionTests
    {
        [Test]
        public async Task ValidateExchangeSubscriptions()
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new TraceLoggerProvider());
            var client = new DeepCoinSocketClient(Options.Create(new Objects.Options.DeepCoinSocketOptions
            {
                OutputOriginalData = true,
                ApiCredentials = new ApiCredentials("123", "456", "789")
            }), loggerFactory);
            var tester = new SocketSubscriptionValidator<DeepCoinSocketClient>(client, "Subscriptions/Exchange", "wss://stream.deepcoin.com");
            await tester.ValidateAsync<DeepCoinSymbolUpdate>((client, handler) => client.ExchangeApi.SubscribeToSymbolUpdatesAsync("ETHUSDT", handler), "Symbol", nestedJsonProperty: "result.0.data");
            await tester.ValidateAsync<DeepCoinTradeUpdate>((client, handler) => client.ExchangeApi.SubscribeToTradeUpdatesAsync("ETHUSDT", handler), "Trades", nestedJsonProperty: "result.0.data");
            await tester.ValidateAsync<DeepCoinKlineUpdate>((client, handler) => client.ExchangeApi.SubscribeToKlineUpdatesAsync("ETHUSDT", handler), "Klines", nestedJsonProperty: "result.0.data");
            await tester.ValidateAsync<DeepCoinOrderBookUpdate>((client, handler) => client.ExchangeApi.SubscribeToOrderBookUpdatesAsync("ETHUSDT", handler), "OrderBook", nestedJsonProperty: "result.0.data", skipUpdateValidation: true);
            await tester.ValidateAsync<DeepCoinOrderUpdate[]>((client, handler) => client.ExchangeApi.SubscribeToUserDataUpdatesAsync("123", handler), "OrderUpdate", nestedJsonProperty: "result", skipUpdateValidation: true);
            await tester.ValidateAsync<DeepCoinBalanceUpdate[]>((client, handler) => client.ExchangeApi.SubscribeToUserDataUpdatesAsync("123", onBalanceMessage: handler), "BalanceUpdate", nestedJsonProperty: "result", skipUpdateValidation: true);
            await tester.ValidateAsync<DeepCoinPositionUpdate[]>((client, handler) => client.ExchangeApi.SubscribeToUserDataUpdatesAsync("123", onPositionMessage: handler), "PositionUpdate", nestedJsonProperty: "result", skipUpdateValidation: true);
            await tester.ValidateAsync<DeepCoinUserTradeUpdate[]>((client, handler) => client.ExchangeApi.SubscribeToUserDataUpdatesAsync("123", onUserTradeMessage: handler), "UserTradeUpdate", nestedJsonProperty: "result", skipUpdateValidation: true);
            await tester.ValidateAsync<DeepCoinAccountUpdate[]>((client, handler) => client.ExchangeApi.SubscribeToUserDataUpdatesAsync("123", onAccountMessage: handler), "AccountUpdate", nestedJsonProperty: "result", skipUpdateValidation: true);
            await tester.ValidateAsync<DeepCoinTriggerOrderUpdate[]>((client, handler) => client.ExchangeApi.SubscribeToUserDataUpdatesAsync("123", onTriggerOrderMessage: handler), "TriggerOrderUpdate", nestedJsonProperty: "result", skipUpdateValidation: true);
        }
    }
}

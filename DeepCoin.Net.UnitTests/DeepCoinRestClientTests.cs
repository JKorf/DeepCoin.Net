using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using DeepCoin.Net.Clients;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;

namespace DeepCoin.Net.UnitTests
{
    [TestFixture()]
    public class DeepCoinRestClientTests
    {
        [Test]
        public void CheckSignatureExample1()
        {
            var authProvider = new DeepCoinAuthenticationProvider(new DeepCoinCredentials("XXX", "XXX", "XXX"));
            var client = (RestApiClient)new DeepCoinRestClient().ExchangeApi;

            CryptoExchange.Net.Testing.TestHelpers.CheckSignature(
                client,
                authProvider,
                HttpMethod.Post,
                "/api/v3/order",
                (uriParams, bodyParams, headers) =>
                {
                    return headers["DC-ACCESS-SIGN"].ToString();
                },
                "Vf4Agvnq70YtbqrjZVCcJUmbqgK8L6ONwb5ldafaptQ=",
                new Parameters(DeepCoinExchange._parameterSerializationSettings)
                {
                    { "symbol", "LTCBTC" },
                },
                DateTimeConverter.ParseFromDouble(1499827320559),
                false);
        }

        [Test]
        public void CheckInterfaces()
        {
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingRestInterfaces<DeepCoinRestClient>();
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingSocketInterfaces<DeepCoinSocketClient>();
        }

        [Test]
        public void TestRestSharedApiDiscoveryMatchesAggregate()
        {
            var (missingOptions, missingInterfaces) = TestHelpers.ValidateSharedApi(new DeepCoinRestClient().ExchangeApi.SharedApi);

            Assert.That(missingOptions, Is.Empty);
            Assert.That(missingInterfaces, Is.Empty);
        }

        [Test]
        public void TestSocketSharedApiDiscoveryMatchesAggregate()
        {
            var (missingOptions, missingInterfaces) = TestHelpers.ValidateSharedApi(new DeepCoinSocketClient().ExchangeApi.SharedApi);

            Assert.That(missingOptions, Is.Empty);
            Assert.That(missingInterfaces, Is.Empty);
        }

        [Test]
        public void TestRestSharedApiDoesntHaveUnsupportedCapabilities()
        {
            var unsupported = TestHelpers.ValidateUnsupportedCapabilities(new DeepCoinRestClient().ExchangeApi.SharedApi);

            Assert.That(unsupported, Is.Empty);
        }

        [Test]
        public void TestSocketSharedApiDoesntHaveUnsupportedCapabilities()
        {
            var unsupported = TestHelpers.ValidateUnsupportedCapabilities(new DeepCoinSocketClient().ExchangeApi.SharedApi);

            Assert.That(unsupported, Is.Empty);
        }
    }
}

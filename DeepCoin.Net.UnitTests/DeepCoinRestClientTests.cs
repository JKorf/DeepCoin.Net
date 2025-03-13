using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using DeepCoin.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace DeepCoin.Net.UnitTests
{
    [TestFixture()]
    public class DeepCoinRestClientTests
    {
        [Test]
        public void CheckSignatureExample1()
        {
            var authProvider = new DeepCoinAuthenticationProvider(new ApiCredentials("XXX", "XXX", "XXX"));
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
                new Dictionary<string, object>
                {
                    { "symbol", "LTCBTC" },
                },
                DateTimeConverter.ParseFromDouble(1499827319559),
                true,
                false);
        }

        [Test]
        public void CheckInterfaces()
        {
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingRestInterfaces<DeepCoinRestClient>();
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingSocketInterfaces<DeepCoinSocketClient>();
        }
    }
}

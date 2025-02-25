using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepCoin.Net.Objects.Options
{
    /// <summary>
    /// DeepCoin options
    /// </summary>
    public class DeepCoinOptions
    {
        /// <summary>
        /// Rest client options
        /// </summary>
        public DeepCoinRestOptions Rest { get; set; } = new DeepCoinRestOptions();

        /// <summary>
        /// Socket client options
        /// </summary>
        public DeepCoinSocketOptions Socket { get; set; } = new DeepCoinSocketOptions();

        /// <summary>
        /// Trade environment. Contains info about URL's to use to connect to the API. Use `DeepCoinEnvironment` to swap environment, for example `Environment = DeepCoinEnvironment.Live`
        /// </summary>
        public DeepCoinEnvironment? Environment { get; set; }

        /// <summary>
        /// The api credentials used for signing requests.
        /// </summary>
        public DeepCoinApiCredentials? ApiCredentials { get; set; }

        /// <summary>
        /// The DI service lifetime for the IDeepCoinSocketClient
        /// </summary>
        public ServiceLifetime? SocketClientLifeTime { get; set; }
    }
}

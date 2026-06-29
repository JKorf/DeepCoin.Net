using DeepCoin.Net.Interfaces.Clients;
using DeepCoin.Net.Objects.Options;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using CryptoExchange.Net.Clients;

namespace DeepCoin.Net.Clients
{
    /// <inheritdoc />
    public class DeepCoinUserClientProvider : UserClientProvider<
        IDeepCoinRestClient,
        IDeepCoinSocketClient,
        DeepCoinRestOptions,
        DeepCoinSocketOptions,
        DeepCoinCredentials,
        DeepCoinEnvironment
        >, IDeepCoinUserClientProvider
    {
       
        /// <inheritdoc />
        public override string ExchangeName => DeepCoinExchange.ExchangeName;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="optionsDelegate">Options to use for created clients</param>
        public DeepCoinUserClientProvider(Action<DeepCoinOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate).Rest), Options.Create(ApplyOptionsDelegate(optionsDelegate).Socket))
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        public DeepCoinUserClientProvider(
            HttpClient? httpClient,
            ILoggerFactory? loggerFactory,
            IOptions<DeepCoinRestOptions> restOptions,
            IOptions<DeepCoinSocketOptions> socketOptions)
            : base(httpClient, loggerFactory, restOptions, socketOptions)
        {
        }

        /// <inheritdoc />
        protected override IDeepCoinRestClient ConstructRestClient(HttpClient client, ILoggerFactory? loggerFactory, IOptions<DeepCoinRestOptions> options)
            => new DeepCoinRestClient(client, loggerFactory, options);
        /// <inheritdoc />
        protected override IDeepCoinSocketClient ConstructSocketClient(ILoggerFactory? loggerFactory, IOptions<DeepCoinSocketOptions> options)
            => new DeepCoinSocketClient(options, loggerFactory);
    }
}

using DeepCoin.Net.Interfaces.Clients;
using DeepCoin.Net.Objects.Options;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Collections.Generic;

namespace DeepCoin.Net.Clients
{
    /// <inheritdoc />
    public class DeepCoinUserClientProvider : IDeepCoinUserClientProvider
    {
        private static ConcurrentDictionary<string, IDeepCoinRestClient> _restClients = new ConcurrentDictionary<string, IDeepCoinRestClient>();
        private static ConcurrentDictionary<string, IDeepCoinSocketClient> _socketClients = new ConcurrentDictionary<string, IDeepCoinSocketClient>();

        private readonly IOptions<DeepCoinRestOptions> _restOptions;
        private readonly IOptions<DeepCoinSocketOptions> _socketOptions;
        private readonly HttpClient _httpClient;
        private readonly ILoggerFactory? _loggerFactory;

        /// <inheritdoc />
        public string ExchangeName => DeepCoinExchange.ExchangeName;

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
        {
            _httpClient = httpClient ?? new HttpClient();
            _loggerFactory = loggerFactory;
            _restOptions = restOptions;
            _socketOptions = socketOptions;
        }

        /// <inheritdoc />
        public void InitializeUserClient(string userIdentifier, ApiCredentials credentials, DeepCoinEnvironment? environment = null)
        {
            CreateRestClient(userIdentifier, credentials, environment);
            CreateSocketClient(userIdentifier, credentials, environment);
        }

        /// <inheritdoc />
        public void ClearUserClients(string userIdentifier)
        {
            _restClients.TryRemove(userIdentifier, out _);
            _socketClients.TryRemove(userIdentifier, out _);
        }

        /// <inheritdoc />
        public IDeepCoinRestClient GetRestClient(string userIdentifier, ApiCredentials? credentials = null, DeepCoinEnvironment? environment = null)
        {
            if (!_restClients.TryGetValue(userIdentifier, out var client))
                client = CreateRestClient(userIdentifier, credentials, environment);

            return client;
        }

        /// <inheritdoc />
        public IDeepCoinSocketClient GetSocketClient(string userIdentifier, ApiCredentials? credentials = null, DeepCoinEnvironment? environment = null)
        {
            if (!_socketClients.TryGetValue(userIdentifier, out var client))
                client = CreateSocketClient(userIdentifier, credentials, environment);

            return client;
        }

        private IDeepCoinRestClient CreateRestClient(string userIdentifier, ApiCredentials? credentials, DeepCoinEnvironment? environment)
        {
            var clientRestOptions = SetRestEnvironment(environment);
            var client = new DeepCoinRestClient(_httpClient, _loggerFactory, clientRestOptions);
            if (credentials != null)
            {
                client.SetApiCredentials(credentials);
                _restClients.TryAdd(userIdentifier, client);
            }
            return client;
        }

        private IDeepCoinSocketClient CreateSocketClient(string userIdentifier, ApiCredentials? credentials, DeepCoinEnvironment? environment)
        {
            var clientSocketOptions = SetSocketEnvironment(environment);
            var client = new DeepCoinSocketClient(clientSocketOptions!, _loggerFactory);
            if (credentials != null)
            {
                client.SetApiCredentials(credentials);
                _socketClients.TryAdd(userIdentifier, client);
            }
            return client;
        }

        private IOptions<DeepCoinRestOptions> SetRestEnvironment(DeepCoinEnvironment? environment)
        {
            if (environment == null)
                return _restOptions;

            var newRestClientOptions = new DeepCoinRestOptions();
            var restOptions = _restOptions.Value.Set(newRestClientOptions);
            newRestClientOptions.Environment = environment;
            return Options.Create(newRestClientOptions);
        }

        private IOptions<DeepCoinSocketOptions> SetSocketEnvironment(DeepCoinEnvironment? environment)
        {
            if (environment == null)
                return _socketOptions;

            var newSocketClientOptions = new DeepCoinSocketOptions();
            var restOptions = _socketOptions.Value.Set(newSocketClientOptions);
            newSocketClientOptions.Environment = environment;
            return Options.Create(newSocketClientOptions);
        }

        private static T ApplyOptionsDelegate<T>(Action<T>? del) where T : new()
        {
            var opts = new T();
            del?.Invoke(opts);
            return opts;
        }
    }
}

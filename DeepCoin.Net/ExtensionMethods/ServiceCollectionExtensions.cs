using CryptoExchange.Net;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using DeepCoin.Net;
using DeepCoin.Net.Clients;
using DeepCoin.Net.Interfaces;
using DeepCoin.Net.Interfaces.Clients;
using DeepCoin.Net.Objects.Options;
using DeepCoin.Net.SymbolOrderBooks;
using CryptoExchange.Net.Interfaces.Clients;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Add services such as the IDeepCoinRestClient and IDeepCoinSocketClient. Configures the services based on the provided configuration.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The configuration(section) containing the options</param>
        /// <returns></returns>
        public static IServiceCollection AddDeepCoin(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var options = new DeepCoinOptions();
            // Reset environment so we know if they're overridden
            options.Rest.Environment = null!;
            options.Socket.Environment = null!;
            configuration.Bind(options);

            if (options.Rest == null || options.Socket == null)
                throw new ArgumentException("Options null");

            var restEnvName = options.Rest.Environment?.Name ?? options.Environment?.Name ?? DeepCoinEnvironment.Live.Name;
            var socketEnvName = options.Socket.Environment?.Name ?? options.Environment?.Name ?? DeepCoinEnvironment.Live.Name;
            options.Rest.Environment = DeepCoinEnvironment.GetEnvironmentByName(restEnvName) ?? options.Rest.Environment!;
            options.Rest.ApiCredentials = options.Rest.ApiCredentials ?? options.ApiCredentials;
            options.Socket.Environment = DeepCoinEnvironment.GetEnvironmentByName(socketEnvName) ?? options.Socket.Environment!;
            options.Socket.ApiCredentials = options.Socket.ApiCredentials ?? options.ApiCredentials;


            services.AddSingleton(x => Options.Options.Create(options.Rest));
            services.AddSingleton(x => Options.Options.Create(options.Socket));

            return AddDeepCoinCore(services, options.SocketClientLifeTime);
        }

        /// <summary>
        /// Add services such as the IDeepCoinRestClient and IDeepCoinSocketClient. Services will be configured based on the provided options.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsDelegate">Set options for the DeepCoin services</param>
        /// <returns></returns>
        public static IServiceCollection AddDeepCoin(
            this IServiceCollection services,
            Action<DeepCoinOptions>? optionsDelegate = null)
        {
            var options = new DeepCoinOptions();
            // Reset environment so we know if they're overridden
            options.Rest.Environment = null!;
            options.Socket.Environment = null!;
            optionsDelegate?.Invoke(options);
            if (options.Rest == null || options.Socket == null)
                throw new ArgumentException("Options null");

            options.Rest.Environment = options.Rest.Environment ?? options.Environment ?? DeepCoinEnvironment.Live;
            options.Rest.ApiCredentials = options.Rest.ApiCredentials ?? options.ApiCredentials;
            options.Socket.Environment = options.Socket.Environment ?? options.Environment ?? DeepCoinEnvironment.Live;
            options.Socket.ApiCredentials = options.Socket.ApiCredentials ?? options.ApiCredentials;

            services.AddSingleton(x => Options.Options.Create(options.Rest));
            services.AddSingleton(x => Options.Options.Create(options.Socket));

            return AddDeepCoinCore(services, options.SocketClientLifeTime);
        }

        private static IServiceCollection AddDeepCoinCore(
            this IServiceCollection services,
            ServiceLifetime? socketClientLifeTime = null)
        {
            services.AddHttpClient<IDeepCoinRestClient, DeepCoinRestClient>((client, serviceProvider) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<DeepCoinRestOptions>>().Value;
                client.Timeout = options.RequestTimeout;
                return new DeepCoinRestClient(client, serviceProvider.GetRequiredService<ILoggerFactory>(), serviceProvider.GetRequiredService<IOptions<DeepCoinRestOptions>>());
            }).ConfigurePrimaryHttpMessageHandler((serviceProvider) => {
                var options = serviceProvider.GetRequiredService<IOptions<DeepCoinRestOptions>>().Value;
                return LibraryHelpers.CreateHttpClientMessageHandler(options.Proxy, options.HttpKeepAliveInterval);
            });
            services.Add(new ServiceDescriptor(typeof(IDeepCoinSocketClient), x => { return new DeepCoinSocketClient(x.GetRequiredService<IOptions<DeepCoinSocketOptions>>(), x.GetRequiredService<ILoggerFactory>()); }, socketClientLifeTime ?? ServiceLifetime.Singleton));

            services.AddTransient<ICryptoRestClient, CryptoRestClient>();
            services.AddSingleton<ICryptoSocketClient, CryptoSocketClient>();
            services.AddTransient<IDeepCoinOrderBookFactory, DeepCoinOrderBookFactory>();
            services.AddTransient<IDeepCoinTrackerFactory, DeepCoinTrackerFactory>();
            services.AddTransient<ITrackerFactory, DeepCoinTrackerFactory>();
            services.AddSingleton<IDeepCoinUserClientProvider, DeepCoinUserClientProvider>(x =>
            new DeepCoinUserClientProvider(
                x.GetRequiredService<HttpClient>(),
                x.GetRequiredService<ILoggerFactory>(),
                x.GetRequiredService<IOptions<DeepCoinRestOptions>>(),
                x.GetRequiredService<IOptions<DeepCoinSocketOptions>>()));

            services.RegisterSharedRestInterfaces(x => x.GetRequiredService<IDeepCoinRestClient>().ExchangeApi.SharedClient);
            services.RegisterSharedSocketInterfaces(x => x.GetRequiredService<IDeepCoinSocketClient>().ExchangeApi.SharedClient);

            return services;
        }
    }
}

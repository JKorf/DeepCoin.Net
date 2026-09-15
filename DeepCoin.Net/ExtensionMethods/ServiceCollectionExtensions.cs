using CryptoExchange.Net;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.SharedApis;
using DeepCoin.Net;
using DeepCoin.Net.Clients;
using DeepCoin.Net.Interfaces;
using DeepCoin.Net.Interfaces.Clients;
using DeepCoin.Net.Objects.Options;
using DeepCoin.Net.SymbolOrderBooks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Add services such as the IDeepCoinRestClient and IDeepCoinSocketClient. Configures the services based on the provided configuration.<br />
        /// See <see href="https://github.com/JKorf/DeepCoin.Net/blob/main/Examples/example-config.json" /> for an example of how to set up the configuration.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The configuration(section) containing the options</param>
        /// <returns></returns>
        public static IServiceCollection AddDeepCoin(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var options = DeepCoinOptions.CreateFromConfiguration(configuration);
            services.AddSingleton(Options.Options.Create(options.Rest));
            services.AddSingleton(Options.Options.Create(options.Socket));
            services.AddSingleton(Options.Options.Create(options));

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
            var options = DeepCoinOptions.Create(optionsDelegate);
            services.AddSingleton(Options.Options.Create(options.Rest));
            services.AddSingleton(Options.Options.Create(options.Socket));
            services.AddSingleton(Options.Options.Create(options));

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
                return LibraryHelpers.CreateHttpClientMessageHandler(options);
            }).SetHandlerLifetime(Timeout.InfiniteTimeSpan);
            services.Add(new ServiceDescriptor(typeof(IDeepCoinSocketClient), x => { return new DeepCoinSocketClient(x.GetRequiredService<IOptions<DeepCoinSocketOptions>>(), x.GetRequiredService<ILoggerFactory>()); }, socketClientLifeTime ?? ServiceLifetime.Singleton));

            services.AddTransient<IDeepCoinOrderBookFactory, DeepCoinOrderBookFactory>();
            services.AddTransient<IDeepCoinTrackerFactory, DeepCoinTrackerFactory>();
            services.AddTransient<ITrackerFactory, DeepCoinTrackerFactory>();
            services.AddSingleton<IDeepCoinUserClientProvider, DeepCoinUserClientProvider>(x =>
            new DeepCoinUserClientProvider(
                x.GetRequiredService<IHttpClientFactory>().CreateClient(typeof(IDeepCoinRestClient).Name),
                x.GetRequiredService<ILoggerFactory>(),
                x.GetRequiredService<IOptions<DeepCoinRestOptions>>(),
                x.GetRequiredService<IOptions<DeepCoinSocketOptions>>()));

            services.RegisterSharedRestInterfaces(x => x.GetRequiredService<IDeepCoinRestClient>().ExchangeApi.SharedClient);
            services.RegisterSharedSocketInterfaces(x => x.GetRequiredService<IDeepCoinSocketClient>().ExchangeApi.SharedClient);

            services.RegisterSharedApiClient<
                IDeepCoinSharedApiClient,
                DeepCoinSharedApiClient>(sharedApis => sharedApis
                    .Add(client => client.Rest)
                    .Add(client => client.Socket)
                    );
            return services;
        }
    }
}

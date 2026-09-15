using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects.Options;
using Microsoft.Extensions.Configuration;
using System;
using CryptoExchange.Net.SharedApis;

namespace DeepCoin.Net.Objects.Options
{
    /// <summary>
    /// DeepCoin options
    /// </summary>
    public class DeepCoinOptions: LibraryOptions<DeepCoinRestOptions, DeepCoinSocketOptions, DeepCoinCredentials, DeepCoinEnvironment>
    {
        /// <summary>
        /// Options for Shared API usage
        /// </summary>
        public SharedApiOptions SharedApi { get; set; } = new();
        /// <summary>
        /// Create DeepCoinOptions instance using the provided configuration action
        /// </summary>
        public static DeepCoinOptions Create(Action<DeepCoinOptions>? configure = null)
        {
            var options = CreateUnconfigured();
            configure?.Invoke(options);
            return Normalize(options);
        }

        /// <summary>
        /// Create DeepCoinOptions using the provided IConfiguration
        /// </summary>
        public static DeepCoinOptions CreateFromConfiguration(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var options = CreateUnconfigured();
            try
            {
                configuration.Bind(options);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Invalid DeepCoin configuration provided", ex);
            }

            if (options.Environment != null)
                options.Environment = DeepCoinEnvironment.GetEnvironmentByName(options.Environment.Name) ?? options.Environment;
            if (options.Rest?.Environment != null)
                options.Rest.Environment = DeepCoinEnvironment.GetEnvironmentByName(options.Rest.Environment.Name) ?? options.Rest.Environment;
            if (options.Socket?.Environment != null)
                options.Socket.Environment = DeepCoinEnvironment.GetEnvironmentByName(options.Socket.Environment.Name) ?? options.Socket.Environment;

            return Normalize(options);
        }

        private static DeepCoinOptions CreateUnconfigured()
        {
            var options = new DeepCoinOptions();
            options.Rest.Environment = null!;
            options.Socket.Environment = null!;
            return options;
        }

        private static DeepCoinOptions Normalize(DeepCoinOptions options)
        {
            if (options.Rest == null)
                throw new ArgumentException("REST options cannot be null", nameof(options));
            if (options.Socket == null)
                throw new ArgumentException("Socket options cannot be null", nameof(options));

            options.Rest.Environment ??= options.Environment ?? DeepCoinEnvironment.Live;
            options.Rest.ApiCredentials ??= options.ApiCredentials;
            options.Socket.Environment ??= options.Environment ?? DeepCoinEnvironment.Live;
            options.Socket.ApiCredentials ??= options.ApiCredentials;
            return options;
        }
    }
}

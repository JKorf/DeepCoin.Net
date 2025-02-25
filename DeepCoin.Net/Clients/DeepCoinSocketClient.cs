using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using DeepCoin.Net.Interfaces.Clients;
using DeepCoin.Net.Objects.Options;
using DeepCoin.Net.Interfaces.Clients.ExchangeApi;
using DeepCoin.Net.Clients.ExchangeApi;

namespace DeepCoin.Net.Clients
{
    /// <inheritdoc cref="IDeepCoinSocketClient" />
    public class DeepCoinSocketClient : BaseSocketClient, IDeepCoinSocketClient
    {
        #region fields
        #endregion

        #region Api clients
                
         /// <inheritdoc />
        public IDeepCoinSocketClientExchangeApi ExchangeApi { get; }


        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of DeepCoinSocketClient
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public DeepCoinSocketClient(Action<DeepCoinSocketOptions>? optionsDelegate = null)
            : this(Options.Create(ApplyOptionsDelegate(optionsDelegate)), null)
        {
        }

        /// <summary>
        /// Create a new instance of DeepCoinSocketClient
        /// </summary>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="options">Option configuration</param>
        public DeepCoinSocketClient(IOptions<DeepCoinSocketOptions> options, ILoggerFactory? loggerFactory = null) : base(loggerFactory, "DeepCoin")
        {
            Initialize(options.Value);
                        
            ExchangeApi = AddApiClient(new DeepCoinSocketClientExchangeApi(_logger, options.Value));
        }

        #endregion

        /// <inheritdoc />
        public void SetOptions(UpdateOptions options)
        {
#warning TODO
        }

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<DeepCoinSocketOptions> optionsDelegate)
        {
            DeepCoinSocketOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials credentials)
        {
            
            ExchangeApi.SetApiCredentials(credentials);

        }
    }
}

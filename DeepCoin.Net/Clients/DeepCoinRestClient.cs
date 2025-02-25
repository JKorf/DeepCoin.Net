using Microsoft.Extensions.Logging;
using System.Net.Http;
using System;
using CryptoExchange.Net.Authentication;
using DeepCoin.Net.Interfaces.Clients;
using DeepCoin.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using Microsoft.Extensions.Options;
using CryptoExchange.Net.Objects.Options;
using DeepCoin.Net.Interfaces.Clients.ExchangeApi;
using DeepCoin.Net.Clients.ExchangeApi;

namespace DeepCoin.Net.Clients
{
    /// <inheritdoc cref="IDeepCoinRestClient" />
    public class DeepCoinRestClient : BaseRestClient, IDeepCoinRestClient
    {
        #region Api clients

        
         /// <inheritdoc />
        public IDeepCoinRestClientExchangeApi ExchangeApi { get; }


        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of the DeepCoinRestClient using provided options
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public DeepCoinRestClient(Action<DeepCoinRestOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate)))
        {
        }

        /// <summary>
        /// Create a new instance of the DeepCoinRestClient using provided options
        /// </summary>
        /// <param name="options">Option configuration</param>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="httpClient">Http client for this client</param>
        public DeepCoinRestClient(HttpClient? httpClient, ILoggerFactory? loggerFactory, IOptions<DeepCoinRestOptions> options) : base(loggerFactory, "DeepCoin")
        {
            Initialize(options.Value);
                        
            ExchangeApi = AddApiClient(new DeepCoinRestClientExchangeApi(_logger, httpClient, options.Value));
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
        public static void SetDefaultOptions(Action<DeepCoinRestOptions> optionsDelegate)
        {
            DeepCoinRestOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials credentials)
        {
            
            ExchangeApi.SetApiCredentials(credentials);

        }
    }
}

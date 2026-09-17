using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using DeepCoin.Net.Clients.MessageHandlers;
using DeepCoin.Net.Interfaces.Clients.V2Api;
using DeepCoin.Net.Objects.Internal;
using DeepCoin.Net.Objects.Options;
using Microsoft.Extensions.Logging;

namespace DeepCoin.Net.Clients.V2Api
{
    /// <inheritdoc cref="IDeepCoinRestClientV2Api" />
    internal class DeepCoinRestClientV2Api : RestApiClient<DeepCoinEnvironment, DeepCoinAuthenticationProvider, DeepCoinCredentials>, IDeepCoinRestClientV2Api
    {
        #region Properties
        /// <inheritdoc />
        protected override ErrorMapping ErrorMapping => DeepCoinErrors.Errors;
        /// <inheritdoc />
        protected override IRestMessageHandler MessageHandler => new DeepCoinRestV2MessageHandler(DeepCoinErrors.Errors);
        /// <inheritdoc />
        public IDeepCoinRestClientV2ApiAccount Account { get; }
        /// <inheritdoc />
        public IDeepCoinRestClientV2ApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public IDeepCoinRestClientV2ApiTrading Trading { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the independently configured V2 REST API.
        /// </summary>
        internal DeepCoinRestClientV2Api(ILoggerFactory? loggerFactory, HttpClient? httpClient, DeepCoinRestOptions options)
            : base(loggerFactory, DeepCoinExchange.Metadata.Id, httpClient, options.Environment.RestClientAddress, options, options.V2Options)
        {
            Account = new DeepCoinRestClientV2ApiAccount(this);
            ExchangeData = new DeepCoinRestClientV2ApiExchangeData(_logger, this);
            Trading = new DeepCoinRestClientV2ApiTrading(_logger, this);
        }
        #endregion

        #region Methods
        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(DeepCoinExchange._serializerContext));
        /// <inheritdoc />
        protected override DeepCoinAuthenticationProvider CreateAuthenticationProvider(DeepCoinCredentials credentials) => new(credentials);
        /// <inheritdoc />
        protected override Task<HttpResult<DateTime>> GetServerTimestampAsync() => ExchangeData.GetServerTimeAsync();

        /// <summary>
        /// Sends a V2 request and preserves the native envelope error.
        /// </summary>
        internal async Task<HttpResult<T>> SendAsync<T>(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<DeepCoinResponse<T>>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<T>(result);
            if (result.Data.Code != 0)
                return HttpResult.Fail<T>(result, new ServerError(result.Data.Code, GetErrorInfo(result.Data.Code, result.Data.Message!)));
            return HttpResult.Ok(result, result.Data.Data!);
        }

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => DeepCoinExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);
        #endregion
    }
}

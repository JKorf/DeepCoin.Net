using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DeepCoin.Net.Interfaces.Clients.ExchangeApi;
using DeepCoin.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using DeepCoin.Net.Objects.Internal;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Converters.MessageParsing;
using System.Net.Http.Headers;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using DeepCoin.Net.Clients.MessageHandlers;

namespace DeepCoin.Net.Clients.ExchangeApi
{
    /// <inheritdoc cref="IDeepCoinRestClientExchangeApi" />
    internal partial class DeepCoinRestClientExchangeApi : RestApiClient<DeepCoinEnvironment, DeepCoinAuthenticationProvider, DeepCoinCredentials>, IDeepCoinRestClientExchangeApi
    {
        #region fields 
        
        protected override ErrorMapping ErrorMapping => DeepCoinErrors.Errors;

        protected override IRestMessageHandler MessageHandler => new DeepCoinRestMessageHandler(DeepCoinErrors.Errors);
        #endregion

        #region Api clients
        /// <inheritdoc />
        public IDeepCoinRestClientExchangeApiAccount Account { get; }
        /// <inheritdoc />
        public IDeepCoinRestClientExchangeApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public IDeepCoinRestClientExchangeApiTrading Trading { get; }
        /// <inheritdoc />
        public string ExchangeName => "DeepCoin";
        #endregion

        #region constructor/destructor
        internal DeepCoinRestClientExchangeApi(ILogger logger, HttpClient? httpClient, DeepCoinRestOptions options)
            : base(logger, DeepCoinExchange.Metadata.Id, httpClient, options.Environment.RestClientAddress, options, options.ExchangeOptions)
        {
            Account = new DeepCoinRestClientExchangeApiAccount(this);
            ExchangeData = new DeepCoinRestClientExchangeApiExchangeData(logger, this);
            Trading = new DeepCoinRestClientExchangeApiTrading(logger, this);
        }
        #endregion

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(DeepCoinExchange._serializerContext));

        /// <inheritdoc />
        protected override DeepCoinAuthenticationProvider CreateAuthenticationProvider(DeepCoinCredentials credentials)
            => new DeepCoinAuthenticationProvider(credentials);

        internal async Task<HttpResult> SendAsync(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            return await base.SendAsync<Unit>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
        }

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
        protected override Task<HttpResult<DateTime>> GetServerTimestampAsync() => throw new NotImplementedException();


        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null) 
            => DeepCoinExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);

        /// <inheritdoc />
        public IDeepCoinRestClientExchangeApiShared SharedClient => this;

    }
}

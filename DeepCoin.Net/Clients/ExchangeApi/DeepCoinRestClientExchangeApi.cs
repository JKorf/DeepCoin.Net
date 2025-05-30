using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
using DeepCoin.Net.Objects;

namespace DeepCoin.Net.Clients.ExchangeApi
{
    /// <inheritdoc cref="IDeepCoinRestClientExchangeApi" />
    internal partial class DeepCoinRestClientExchangeApi : RestApiClient, IDeepCoinRestClientExchangeApi
    {
        #region fields 
        internal static TimeSyncState _timeSyncState = new TimeSyncState("Exchange Api");
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
            : base(logger, httpClient, options.Environment.RestClientAddress, options, options.ExchangeOptions)
        {
            Account = new DeepCoinRestClientExchangeApiAccount(this);
            ExchangeData = new DeepCoinRestClientExchangeApiExchangeData(logger, this);
            Trading = new DeepCoinRestClientExchangeApiTrading(logger, this);
        }
        #endregion

        /// <inheritdoc />
        protected override IStreamMessageAccessor CreateAccessor() => new SystemTextJsonStreamMessageAccessor(SerializerOptions.WithConverters(DeepCoinExchange._serializerContext));
        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(DeepCoinExchange._serializerContext));

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new DeepCoinAuthenticationProvider(credentials);

        internal Task<WebCallResult> SendAsync(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
            => SendToAddressAsync(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult> SendToAddressAsync(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);

            // Optional response checking

            return result;
        }

        internal Task<WebCallResult<T>> SendAsync<T>(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
            => SendToAddressAsync<T>(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult<T>> SendToAddressAsync<T>(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
        {
            var result = await base.SendAsync<DeepCoinResponse<T>>(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            if (!result)
                return result.As<T>(default);

            if (result.Data.Code != 0)
                return result.AsError<T>(new ServerError(result.Data.Code, result.Data.Message!));

            return result.As<T>(result.Data.Data);
        }

        /// <inheritdoc />
        protected override Task<WebCallResult<DateTime>> GetServerTimestampAsync() => throw new NotImplementedException();

        /// <inheritdoc />
        public override TimeSyncInfo? GetTimeSyncInfo() => null;

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null) 
            => DeepCoinExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);
        public override TimeSpan? GetTimeOffset() => default;

        /// <inheritdoc />
        public IDeepCoinRestClientExchangeApiShared SharedClient => this;

    }
}

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
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Converters.MessageParsing;

namespace DeepCoin.Net.Clients.ExchangeApi
{
    /// <inheritdoc cref="IDeepCoinRestClientExchangeApi" />
    internal partial class DeepCoinRestClientExchangeApi : RestApiClient, IDeepCoinRestClientExchangeApi
    {
        #region fields 
        internal static TimeSyncState _timeSyncState = new TimeSyncState("Exchange Api");

        protected override ErrorCollection ErrorMapping { get; } = new ErrorCollection([

            new ErrorInfo(ErrorType.Unauthorized, false, "API access frozen, contact customer service", "50100"),
            new ErrorInfo(ErrorType.Unauthorized, false, "API environment not correct", "50101"),
            new ErrorInfo(ErrorType.Unauthorized, false, "Incorrect passphrase", "50105"),
            new ErrorInfo(ErrorType.Unauthorized, false, "IP address not allowed", "50110"),
            new ErrorInfo(ErrorType.Unauthorized, false, "Invalid API key", "50113"),

            new ErrorInfo(ErrorType.TimestampInvalid, false, "Request timestamp expired", "50102"),
            new ErrorInfo(ErrorType.TimestampInvalid, false, "Invalid timestamp", "50112"),

            new ErrorInfo(ErrorType.TimestampInvalid, false, "Invalid signature", "50111"),

            new ErrorInfo(ErrorType.UnknownSymbol, false, "Unknown symbol", "50"),

            new ErrorInfo(ErrorType.BalanceInsufficient, false, "Insufficient balance", "36"),

            new ErrorInfo(ErrorType.PriceInvalid, false, "Invalid price", "175"),

            new ErrorInfo(ErrorType.QuantityInvalid, false, "Order quantity tick invalid", "44"),
            new ErrorInfo(ErrorType.QuantityInvalid, false, "Order quantity too large", "193"),
            new ErrorInfo(ErrorType.QuantityInvalid, false, "Order quantity too small", "194"),

            ]);
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
                return result.AsError<T>(new ServerError(result.Data.Code, GetErrorInfo(result.Data.Code, result.Data.Message!)));

            return result.As<T>(result.Data.Data);
        }

        /// <inheritdoc />
        protected override Error? TryParseError(KeyValuePair<string, string[]>[] responseHeaders, IMessageAccessor accessor)
        {
            if (!accessor.IsValid)
                return new ServerError(ErrorInfo.Unknown);

            var code = accessor.GetValue<int?>(MessagePath.Get().Property("code"));
            if (code == 0)
                return null;

            var msg = accessor.GetValue<string>(MessagePath.Get().Property("msg"));
            if (msg == null)
                return new ServerError(ErrorInfo.Unknown);

            if (code == null)
                return new ServerError(ErrorInfo.Unknown with { Message = msg });

            return new ServerError(code.Value, GetErrorInfo(code.Value, msg));
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

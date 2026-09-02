using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using DeepCoin.Net.Interfaces.Clients.ExchangeApi;
using DeepCoin.Net.Objects.Models;
using DeepCoin.Net.Objects.Options;
using DeepCoin.Net.Objects.Sockets.Subscriptions;
using System.Linq;
using CryptoExchange.Net;
using DeepCoin.Net.Objects.Internal;
using DeepCoin.Net.Objects.Sockets;
using System.Net.WebSockets;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using DeepCoin.Net.Clients.MessageHandlers;
using CryptoExchange.Net.Sockets.Default;
using Microsoft.Extensions.Options;
using CryptoExchange.Net.TokenManagement;
using CryptoExchange.Net.Sockets.Interfaces;

namespace DeepCoin.Net.Clients.ExchangeApi
{
    /// <summary>
    /// Client providing access to the DeepCoin Exchange websocket Api
    /// </summary>
    internal partial class DeepCoinSocketClientExchangeApi : SocketApiClient<DeepCoinEnvironment, DeepCoinAuthenticationProvider, DeepCoinCredentials>, IDeepCoinSocketClientExchangeApi
    {
        private readonly ILoggerFactory? _loggerFactory;
        private DeepCoinRestClient? _tokenClient;
        internal TokenManager TokenManager { get; }
        private DeepCoinRestClient TokenClient
        {
            get
            {
                if (_tokenClient == null)
                {
                    _tokenClient = new DeepCoinRestClient(null, _loggerFactory, Options.Create(new DeepCoinRestOptions
                    {
                        ApiCredentials = ApiCredentials,
                        Environment = ClientOptions.Environment,
                        Proxy = ClientOptions.Proxy,
                        OutputOriginalData = ClientOptions.OutputOriginalData
                    }));
                }

                return _tokenClient;
            }
        }

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal DeepCoinSocketClientExchangeApi(ILoggerFactory? loggerFactory, DeepCoinSocketOptions options) :
            base(loggerFactory, DeepCoinExchange.Metadata.Id, options.Environment.SocketClientAddress!, options, options.ExchangeOptions)
        {
            _loggerFactory = loggerFactory;

            KeepAliveInterval = TimeSpan.Zero;

            TokenManager = new TokenManager(
                DeepCoinExchange.Metadata.Id,
                loggerFactory,
                TimeSpan.FromMinutes(30),
                TimeSpan.FromMinutes(60),
                startToken: StartListenKeyAsync,
                keepAliveToken: KeepAliveListenKeyAsync);

            RegisterPeriodicQuery("ping",
                TimeSpan.FromSeconds(30), 
                (x) => new DeepCoinPingQuery(), 
                (connection, result) =>
                {
                    if (connection.ConnectionUri.AbsolutePath.Contains("v1/private"))
                        // Private endpoint doesn't return pong on pings
                        return;

                    if (result.Error?.ErrorType == ErrorType.Timeout)
                    {
                        // Ping timeout, reconnect
                        _logger.LogWarning("[Sckt {SocketId}] Ping response timeout, reconnecting", connection.SocketId);
                        _ = connection.TriggerReconnectAsync();
                    }
                });
        }
        #endregion

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(DeepCoinExchange._serializerContext));

        public override ISocketMessageHandler CreateMessageConverter(WebSocketMessageType messageType) => new DeepCoinSocketMessageHandler();

        /// <inheritdoc />
        protected override DeepCoinAuthenticationProvider CreateAuthenticationProvider(DeepCoinCredentials credentials)
            => new DeepCoinAuthenticationProvider(credentials);

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToSymbolUpdatesAsync(string symbol, Action<DataEvent<DeepCoinSymbolUpdate>> onMessage, CancellationToken ct = default)
        {
            string path;
            if (symbol.EndsWith("-SWAP", StringComparison.InvariantCultureIgnoreCase))
            {
                path = "public/ws";
                symbol = symbol.Replace("-SWAP", "").Replace("-", "");
            }
            else
            {
                path = "public/spotws";
                symbol = symbol.Replace("-", "/");
            }

            var handler = new Action<DateTime, string?, SocketUpdate<DeepCoinSymbolUpdate>>((receiveTime, originalData, data) =>
            {
                var timestamp = data.Result.Max(x => x.Data.UpdateTime);
                UpdateTimeOffset(timestamp);

                onMessage(
                    new DataEvent<DeepCoinSymbolUpdate>(DeepCoinExchange.ExchangeName, data.Result.Where(x => x.Table.Equals("MarketDataOverView")).First().Data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Action)
                        .WithDataTimestamp(timestamp, GetTimeOffset())
                    );
            });
            var subscription = new DeepCoinSubscription<DeepCoinSymbolUpdate>(_logger, this, "PushMarketDataOverView", symbol, "7", handler, false);
            return await SubscribeAsync(BaseAddress.AppendPath(path), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<DeepCoinTradeUpdate>> onMessage, CancellationToken ct = default)
        {
            string path;
            if (symbol.EndsWith("-SWAP", StringComparison.InvariantCultureIgnoreCase))
            {
                path = "public/ws";
                symbol = symbol.Replace("-SWAP", "").Replace("-", "");
            }
            else
            {
                path = "public/spotws";
                symbol = symbol.Replace("-", "/");
            }

            var handler = new Action<DateTime, string?, SocketUpdate<DeepCoinTradeUpdate>>((receiveTime, originalData, data) =>
            {
                var updateData = data.Result.Where(x => x.Table.Equals("MarketTrade")).First().Data;
                UpdateTimeOffset(updateData.Timestamp);

                onMessage(
                    new DataEvent<DeepCoinTradeUpdate>(DeepCoinExchange.ExchangeName, updateData, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Action)
                        .WithSymbol(updateData.Symbol)
                        .WithDataTimestamp(updateData.Timestamp, GetTimeOffset())
                    );
            });
            var subscription = new DeepCoinSubscription<DeepCoinTradeUpdate>(_logger, this, "PushMarketTrade", symbol, "2", handler, false);
            return await SubscribeAsync(BaseAddress.AppendPath(path), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, Action<DataEvent<DeepCoinKlineUpdate>> onMessage, CancellationToken ct = default)
        {
            string path;
            if (symbol.EndsWith("-SWAP", StringComparison.InvariantCultureIgnoreCase))
            {
                path = "public/ws";
                symbol = symbol.Replace("-SWAP", "").Replace("-", "");
            }
            else
            {
                path = "public/spotws";
                symbol = symbol.Replace("-", "/");
            }

            var handler = new Action<DateTime, string?, SocketUpdate<DeepCoinKlineUpdate>>((receiveTime, originalData, data) =>
            {
                var updateData = data.Result.Where(x => x.Table.Equals("KLine") || x.Table.Equals("LastKLine")).First().Data;
                UpdateTimeOffset(updateData.UpdateTime);
                onMessage(
                    new DataEvent<DeepCoinKlineUpdate>(DeepCoinExchange.ExchangeName, updateData, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Action)
                        .WithSymbol(updateData.Symbol)
                        .WithDataTimestamp(updateData.UpdateTime, GetTimeOffset())
                    );
            });
            var topic = symbol + "_1m";
            var subscription = new DeepCoinSubscription<DeepCoinKlineUpdate>(_logger, this, "PushKLine", topic, "11", handler, false);
            return await SubscribeAsync(BaseAddress.AppendPath(path), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, Action<DataEvent<DeepCoinOrderBookUpdate>> onMessage, CancellationToken ct = default)
        {
            string path;
            if (symbol.EndsWith("-SWAP", StringComparison.InvariantCultureIgnoreCase))
            {
                path = "public/ws";
                symbol = symbol.Replace("-SWAP", "").Replace("-", "");
            }
            else
            {
                path = "public/spotws";
                symbol = symbol.Replace("-", "/");
            }

            var subscription = new DeepCoinBookSubscription(_logger, this, "PushMarketOrder", "MarketOrder", symbol, "25", onMessage, false);
            return await SubscribeAsync(BaseAddress.AppendPath(path), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToUserDataUpdatesAsync(
            Action<DataEvent<DeepCoinOrderUpdate[]>>? onOrderMessage = null,
            Action<DataEvent<DeepCoinBalanceUpdate[]>>? onBalanceMessage = null,
            Action<DataEvent<DeepCoinPositionUpdate[]>>? onPositionMessage = null,
            Action<DataEvent<DeepCoinUserTradeUpdate[]>>? onUserTradeMessage = null,
            Action<DataEvent<DeepCoinAccountUpdate[]>>? onAccountMessage = null,
            Action<DataEvent<DeepCoinTriggerOrderUpdate[]>>? onTriggerOrderMessage = null,
            CancellationToken ct = default)
            => await SubscribeToUserDataUpdatesAsync(null, onOrderMessage, onBalanceMessage, onPositionMessage, onUserTradeMessage, onAccountMessage, onTriggerOrderMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToUserDataUpdatesAsync(
            string? listenKey,
            Action<DataEvent<DeepCoinOrderUpdate[]>>? onOrderMessage = null, 
            Action<DataEvent<DeepCoinBalanceUpdate[]>>? onBalanceMessage = null, 
            Action<DataEvent<DeepCoinPositionUpdate[]>>? onPositionMessage = null, 
            Action<DataEvent<DeepCoinUserTradeUpdate[]>>? onUserTradeMessage = null, 
            Action<DataEvent<DeepCoinAccountUpdate[]>>? onAccountMessage = null, 
            Action<DataEvent<DeepCoinTriggerOrderUpdate[]>>? onTriggerOrderMessage = null, 
            CancellationToken ct = default)
        {
            if (listenKey == null && !Authenticated)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, new NoApiCredentialsError());

            TokenLease? lease = null;
            if (listenKey == null)
            {
                var leaseResult = await TokenManager.AcquireAsync(new TokenScope(
                    DeepCoinExchange.Metadata.Id,
                    EnvironmentName,
                    "Exchange",
                    ApiCredentials!.Key), ct).ConfigureAwait(false);
                if (!leaseResult.Success)
                    return WebSocketResult.Fail<UpdateSubscription>(Exchange, leaseResult.Error);

                lease = leaseResult.Data;
            }

            var lk = listenKey ?? lease!.Token.Token;
            var subscription = new DeepCoinUserSubscription(_logger, this, onOrderMessage, onBalanceMessage, onPositionMessage, onUserTradeMessage, onAccountMessage, onTriggerOrderMessage)
            {
                TokenLease = lease
            };
            return await SubscribeAsync(BaseAddress.AppendPath("v1/private?listenKey=" + lk), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public IDeepCoinSocketClientExchangeApiShared SharedClient => this;

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => DeepCoinExchange.FormatWebsocketSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);

        protected override async Task<Uri?> GetReconnectUriAsync(ISocketConnection connection)
        {
            if (!connection.HasAuthenticatedSubscription)
                return await base.GetReconnectUriAsync(connection).ConfigureAwait(false);

            var subscriptions = ((SocketConnection)connection).Subscriptions.Where(x => x.TokenLease != null).ToList();
            if (subscriptions.Count == 0)
                return await base.GetReconnectUriAsync(connection).ConfigureAwait(false);

            var scope = new TokenScope(
                    DeepCoinExchange.Metadata.Id,
                    EnvironmentName,
                    "Exchange",
                    ApiCredentials!.Key);

            var token = await TokenManager.AcquireAndReplaceAsync(subscriptions[0], scope).ConfigureAwait(false);
            if (!token.Success)
                return null;

            return new Uri(BaseAddress.AppendPath("v1/private?listenKey=" + token.Data.Token.Token));
        }

        protected override async Task<CallResult> RevitalizeRequestAsync(Subscription subscription)
        {
            if (subscription.TokenLease == null)
                return CallResult.Ok(); // Not an authenticated subscription, no need to revitalize

            var scope = new TokenScope(
                    DeepCoinExchange.Metadata.Id,
                    EnvironmentName,
                    "Exchange",
                    ApiCredentials!.Key);

            return await TokenManager.AcquireAndReplaceAsync(subscription, scope).ConfigureAwait(false);
        }

        private async Task<CallResult<string>> StartListenKeyAsync(TokenScope tokenScope, CancellationToken ct)
        {
            var result = await TokenClient.ExchangeApi.Account.StartUserStreamAsync(ct).ConfigureAwait(false);
            if (!result.Success)
                return CallResult.Fail<string>(result.Error);

            return CallResult.Ok(result.Data.ListenKey);
        }

        private async Task<CallResult> KeepAliveListenKeyAsync(TokenInfo token, CancellationToken ct)
        {
            var result = await TokenClient.ExchangeApi.Account.KeepAliveUserStreamAsync(token.Token, ct).ConfigureAwait(false);
            if (!result.Success)
                return CallResult.Fail<string>(result.Error);

            return CallResult.Ok();
        }
    }
}

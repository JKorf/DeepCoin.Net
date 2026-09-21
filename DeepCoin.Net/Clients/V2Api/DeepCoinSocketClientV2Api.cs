using Microsoft.Extensions.Options;
using DeepCoin.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets.Interfaces;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.TokenManagement;
using DeepCoin.Net.Clients.MessageHandlers;
using DeepCoin.Net.Enums;
using DeepCoin.Net.Interfaces.Clients.V2Api;
using DeepCoin.Net.Objects.Internal;
using DeepCoin.Net.Objects.Models;
using DeepCoin.Net.Objects.Options;
using DeepCoin.Net.Objects.Sockets.Subscriptions;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace DeepCoin.Net.Clients.V2Api;

/// <summary>
/// Native V2 public protocol plus private streams authenticated with V2 listen keys.
/// </summary>
internal sealed class DeepCoinSocketClientV2Api : SocketApiClient<DeepCoinEnvironment, DeepCoinAuthenticationProvider, DeepCoinCredentials>, IDeepCoinSocketClientV2Api
{
    #region Fields

    private readonly ILoggerFactory? _loggerFactory;
    private DeepCoinRestClient? _tokenClient;

    #endregion

    #region Properties

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

    /// <summary>
    /// Manages private socket listen-key leases and periodic renewal
    /// </summary>
    internal TokenManager TokenManager { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes native V2 public streams and V2 listen-key management.
    /// </summary>
    internal DeepCoinSocketClientV2Api(ILoggerFactory? loggerFactory, DeepCoinSocketOptions options)
        : base(loggerFactory, DeepCoinExchange.Metadata.Id, options.Environment.SocketClientAddress!, options, options.V2Options)
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
            TimeSpan.FromSeconds(8),
            (x) => new DeepCoinPingQuery(),
            (connection, result) =>
            {
                // V2 listen keys still connect to /v1/private, as documented by DeepCoin.
                // Exempt private connections from the public ping/pong timeout recovery.
                if (connection.ConnectionUri.AbsolutePath.Contains("v1/private"))
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

    #region Methods

    // V2 instruments can contain mixed-case asset codes (xAAOI), but socket topics require uppercase.
    private static string NativeSymbol(string symbol) => (symbol.EndsWith("-SWAP", StringComparison.Ordinal)
        ? symbol.Replace("-SWAP", "").Replace("-", "") : symbol.Replace("-", "/")).ToUpperInvariant();

    private string PublicAddress(string symbol) => BaseAddress.AppendPath(symbol.EndsWith("-SWAP", StringComparison.Ordinal)
        ? "streamlet/trade/public/swap?platform=api&version=v2" : "streamlet/trade/public/spot?platform=api&version=v2");

    private DataEvent<T> Event<T>(T data, DeepCoinV2SocketMessage message, DateTime received, string? original, string symbol, DateTime timestamp, SocketUpdateType updateType = SocketUpdateType.Update)
    {
        UpdateTimeOffset(timestamp);
        return new DataEvent<T>(DeepCoinExchange.ExchangeName, data, received, original)
            .WithSymbol(symbol).WithStreamId(message.Action).WithUpdateType(updateType).WithDataTimestamp(timestamp, GetTimeOffset());
    }

    private async Task<CallResult<string>> StartListenKeyAsync(TokenScope tokenScope, CancellationToken ct)
    {
        var result = await TokenClient.V2Api.Account.StartUserStreamAsync(ct).ConfigureAwait(false);
        if (!result.Success)
            return CallResult.Fail<string>(result.Error);

        return CallResult.Ok(result.Data.ListenKey);
    }

    private async Task<CallResult> KeepAliveListenKeyAsync(TokenInfo token, CancellationToken ct)
    {
        var result = await TokenClient.V2Api.Account.KeepAliveUserStreamAsync(token.Token, ct).ConfigureAwait(false);
        if (!result.Success)
            return CallResult.Fail<string>(result.Error);

        return CallResult.Ok();
    }

    /// <inheritdoc />
    protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(DeepCoinExchange._serializerContext));

    /// <inheritdoc />
    protected override DeepCoinAuthenticationProvider CreateAuthenticationProvider(DeepCoinCredentials credentials)
        => new DeepCoinAuthenticationProvider(credentials);

    /// <inheritdoc />
    protected override async Task<Uri?> GetReconnectUriAsync(ISocketConnection connection)
    {
        // Listen-key subscriptions authenticate in the URL, so their message-level Authenticated flag is false.
        if (connection is not SocketConnection socketConnection)
            return await base.GetReconnectUriAsync(connection).ConfigureAwait(false);

        var subscriptions = socketConnection.Subscriptions.Where(x => x.TokenLease != null).ToList();
        if (subscriptions.Count == 0)
            return await base.GetReconnectUriAsync(connection).ConfigureAwait(false);

        var scope = new TokenScope(
                DeepCoinExchange.Metadata.Id,
                EnvironmentName,
                "V2",
                ApiCredentials!.Key);

        var token = await TokenManager.AcquireAndReplaceAsync(subscriptions[0], scope).ConfigureAwait(false);
        if (!token.Success)
            return null;

        return new Uri(BaseAddress.AppendPath("v1/private?listenKey=" + token.Data.Token.Token));
    }

    /// <inheritdoc />
    protected override async Task<CallResult> RevitalizeRequestAsync(Subscription subscription)
    {
        if (subscription.TokenLease == null)
            return CallResult.Ok(); // Not an authenticated subscription, no need to revitalize

        var scope = new TokenScope(
                DeepCoinExchange.Metadata.Id,
                EnvironmentName,
                "V2",
                ApiCredentials!.Key);

        return await TokenManager.AcquireAndReplaceAsync(subscription, scope).ConfigureAwait(false);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
            _tokenClient?.Dispose();
    }

    /// <inheritdoc />
    public override ISocketMessageHandler CreateMessageConverter(WebSocketMessageType messageType) => new DeepCoinV2SocketMessageHandler();

    /// <inheritdoc />
    public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
        => DeepCoinExchange.FormatWebsocketSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);

    /// <inheritdoc />
    public Task<WebSocketResult<UpdateSubscription>> SubscribeToSymbolUpdatesAsync(string symbol, Action<DataEvent<DeepCoinV2SymbolData[]>> onMessage, CancellationToken ct = default)
    {
        var native = NativeSymbol(symbol);
        var subscription = new DeepCoinV2Subscription<DeepCoinV2SymbolMessage>(_logger, native, "market", "PO", (received, original, message) =>
            onMessage(Event(message.Data, message, received, original, native, message.TradeTime)));
        return SubscribeAsync(PublicAddress(symbol), subscription, ct);
    }

    /// <inheritdoc />
    public Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<DeepCoinV2TradeData[]>> onMessage, CancellationToken ct = default)
    {
        var native = NativeSymbol(symbol);
        var subscription = new DeepCoinV2Subscription<DeepCoinV2TradeMessage>(_logger, native, "trade", "PMT", (received, original, message) =>
            onMessage(Event(message.Data, message, received, original, native, message.TradeTime)));
        return SubscribeAsync(PublicAddress(symbol), subscription, ct);
    }

    /// <inheritdoc />
    public Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, Action<DataEvent<DeepCoinKline[]>> onMessage, CancellationToken ct = default)
    {
        var native = NativeSymbol(symbol);
        var subscription = new DeepCoinV2Subscription<DeepCoinV2KlineMessage>(_logger, native, "kline", "PK", (received, original, message) =>
            onMessage(Event(message.Data, message, received, original, native, message.TradeTime)), KlineInterval.OneMinute);
        return SubscribeAsync(PublicAddress(symbol), subscription, ct);
    }

    /// <inheritdoc />
    public Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, Action<DataEvent<DeepCoinV2OrderBookData[]>> onMessage, CancellationToken ct = default)
    {
        var native = NativeSymbol(symbol);
        var subscription = new DeepCoinV2Subscription<DeepCoinV2OrderBookMessage>(_logger, native, "book", "PMO", (received, original, message) =>
        {
            var type = message.UpdateType switch
            {
                V2BookUpdateType.Snapshot => SocketUpdateType.Snapshot,
                V2BookUpdateType.Incremental => SocketUpdateType.Update,
                var value => throw new InvalidOperationException($"Unsupported DeepCoin V2 order book update type '{value}'.")
            };
            onMessage(Event(message.Data, message, received, original, native, message.PublishTime ?? message.TradeTime, type));
        });
        return SubscribeAsync(PublicAddress(symbol), subscription, ct);
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
                "V2",
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

    #endregion
}

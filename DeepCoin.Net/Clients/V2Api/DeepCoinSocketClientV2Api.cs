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
using System.Globalization;
using System.Linq;
using System.Net.WebSockets;
using System.Text.Json;
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

    private static decimal Number(JsonElement value) => value.ValueKind == JsonValueKind.String
        ? decimal.Parse(value.GetString()!, CultureInfo.InvariantCulture) : value.GetDecimal();

    private static decimal? OptionalNumber(JsonElement data, string name) => data.TryGetProperty(name, out var value) && value.ValueKind is not (JsonValueKind.Null or JsonValueKind.Undefined)
        && (value.ValueKind != JsonValueKind.String || !string.IsNullOrEmpty(value.GetString())) ? Number(value) : null;

    private static DateTime Timestamp(long value) => DateTimeOffset.FromUnixTimeMilliseconds(value).UtcDateTime;

    private static string NativeSymbol(string symbol) => symbol.EndsWith("-SWAP", StringComparison.Ordinal)
        ? symbol.Replace("-SWAP", "").Replace("-", "") : symbol.Replace("-", "/");

    private string PublicAddress(string symbol) => BaseAddress.AppendPath(symbol.EndsWith("-SWAP", StringComparison.Ordinal)
        ? "streamlet/trade/public/swap?platform=api&version=v2" : "streamlet/trade/public/spot?platform=api&version=v2");

    private DataEvent<T> Event<T>(T data, DeepCoinV2SocketMessage message, DateTime received, string? original, string symbol, DateTime timestamp, SocketUpdateType updateType = SocketUpdateType.Update)
    {
        UpdateTimeOffset(timestamp);
        return new DataEvent<T>(DeepCoinExchange.ExchangeName, data, received, original)
            .WithSymbol(symbol).WithStreamId(message.Action).WithUpdateType(updateType).WithDataTimestamp(timestamp, GetTimeOffset());
    }

    private static DeepCoinOrderBookUpdateEntry[] BookSide(JsonElement data, string side, string symbol, OrderSide direction)
    {
        if (!data.TryGetProperty(side, out var levels))
            return [];
        return levels.EnumerateArray().Select(row => new DeepCoinOrderBookUpdateEntry { Symbol = symbol, Direction = direction, Price = Number(row[0]), Quantity = Number(row[1]) }).ToArray();
    }

    private void HandleSymbolUpdate(JsonElement data, DeepCoinV2SocketMessage message, DateTime received, string? original, string symbol, string native, Action<DataEvent<DeepCoinSymbolUpdate>> onMessage)
    {
        if (!string.Equals(data.GetProperty("I").GetString(), native, StringComparison.Ordinal))
            return;

        var timestamp = Timestamp((long)(OptionalNumber(data, "U") ?? message.TradeTime));
        var fundingTime = OptionalNumber(data, "PF");
        var update = new DeepCoinSymbolUpdate
        {
            Symbol = data.GetProperty("I").GetString()!, UpdateTime = timestamp,
            ProductGroup = symbol.EndsWith("-USD-SWAP", StringComparison.Ordinal) ? ProductGroup.CoinMargined
                : symbol.EndsWith("-SWAP", StringComparison.Ordinal) ? ProductGroup.USDTMargined : ProductGroup.Spot,
            LastPrice = OptionalNumber(data, "N"), OpenPrice = OptionalNumber(data, "O"), HighPrice = OptionalNumber(data, "H"), LowPrice = OptionalNumber(data, "L"),
            MarkedPrice = OptionalNumber(data, "M"), UnderlyingPrice = OptionalNumber(data, "D") ?? 0,
            UpperLimitPrice = OptionalNumber(data, "C") ?? 0, LowerLimitPrice = OptionalNumber(data, "F") ?? 0,
            Volume = OptionalNumber(data, "V") ?? 0, Turnover = OptionalNumber(data, "T") ?? 0,
            // The docs label V/T as today's totals, but synchronized live linear/inverse REST
            // responses expose these exact values as vol24h/volCcy24h. V2/T2 use an unspecified window.
            Volume24Hrs = OptionalNumber(data, "V"), Turnover24Hrs = OptionalNumber(data, "T"),
            RawV2Volume = OptionalNumber(data, "V2"), RawV2Turnover = OptionalNumber(data, "T2"),
            BestBidPrice = OptionalNumber(data, "BP1"), BestAskPrice = OptionalNumber(data, "AP1"),
            // V2 E is the previous settlement's funding rate, not the next funding rate.
            PrePositionFeeRate = OptionalNumber(data, "E") ?? 0,
            PositionFeeTime = fundingTime > 0 ? DateTimeOffset.FromUnixTimeSeconds((long)fundingTime.Value).UtcDateTime : null
        };
        onMessage(Event(update, message, received, original, native, timestamp));
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
    public Task<WebSocketResult<UpdateSubscription>> SubscribeToSymbolUpdatesAsync(string symbol, Action<DataEvent<DeepCoinSymbolUpdate>> onMessage, CancellationToken ct = default)
    {
        var native = NativeSymbol(symbol);
        var subscription = new DeepCoinV2Subscription(_logger, native, "market", "PO", (received, original, message) =>
        {
            // The docs show one object; live streams send arrays that can contain several instruments.
            // Filter each row so a shared socket never delivers another instrument to this subscriber.
            if (message.Data.ValueKind == JsonValueKind.Array)
            {
                foreach (var data in message.Data.EnumerateArray())
                    HandleSymbolUpdate(data, message, received, original, symbol, native, onMessage);
            }
            else
            {
                HandleSymbolUpdate(message.Data, message, received, original, symbol, native, onMessage);
            }
        });
        return SubscribeAsync(PublicAddress(symbol), subscription, ct);
    }

    /// <inheritdoc />
    public Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<DeepCoinTradeUpdate>> onMessage, CancellationToken ct = default)
    {
        var native = NativeSymbol(symbol);
        var subscription = new DeepCoinV2Subscription(_logger, native, "trade", "PMT", (received, original, message) =>
        {
            foreach (var row in message.Data.EnumerateArray())
            {
                var timestamp = DateTimeOffset.FromUnixTimeSeconds((long)Number(row.GetProperty("T"))).UtcDateTime;
                var update = new DeepCoinTradeUpdate
                {
                    Symbol = message.Symbol, TradeId = row.GetProperty("TradeID").GetString()!, Price = Number(row.GetProperty("P")), Quantity = Number(row.GetProperty("V")), Timestamp = timestamp,
                    Side = row.GetProperty("D").ToString() switch { "0" => OrderSide.Buy, "1" => OrderSide.Sell, var side => throw new InvalidOperationException($"Unsupported DeepCoin trade direction '{side}'.") }
                };
                onMessage(Event(update, message, received, original, native, timestamp));
            }
        });
        return SubscribeAsync(PublicAddress(symbol), subscription, ct);
    }

    /// <inheritdoc />
    public Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, Action<DataEvent<DeepCoinKlineUpdate>> onMessage, CancellationToken ct = default)
    {
        var native = NativeSymbol(symbol);
        var subscription = new DeepCoinV2Subscription(_logger, native, "kline", "PK", (received, original, message) =>
        {
            foreach (var row in message.Data.EnumerateArray())
            {
                var timestamp = Timestamp(message.TradeTime);
                // Live candle rows use Unix seconds; the documented example uses milliseconds.
                var update = new DeepCoinKlineUpdate
                {
                    Symbol = message.Symbol, Interval = KlineInterval.OneMinute, OpenTime = DateTimeConverter.ParseFromDecimal(Number(row[0])),
                    OpenPrice = Number(row[1]), HighPrice = Number(row[2]), LowPrice = Number(row[3]), ClosePrice = Number(row[4]), Volume = Number(row[5]), Turnover = Number(row[6]), UpdateTime = timestamp
                };
                onMessage(Event(update, message, received, original, native, timestamp));
            }
        }, KlineInterval.OneMinute);
        return SubscribeAsync(PublicAddress(symbol), subscription, ct);
    }

    /// <inheritdoc />
    public Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, Action<DataEvent<DeepCoinOrderBookUpdate>> onMessage, CancellationToken ct = default)
    {
        var native = NativeSymbol(symbol);
        var subscription = new DeepCoinV2Subscription(_logger, native, "book", "PMO", (received, original, message) =>
        {
            var type = message.UpdateType switch
            {
                V2BookUpdateType.Snapshot => SocketUpdateType.Snapshot,
                V2BookUpdateType.Incremental => SocketUpdateType.Update,
                var value => throw new InvalidOperationException($"Unsupported DeepCoin V2 order book update type '{value}'.")
            };
            // Live V2 sends one object; the documented example wraps the same book in an array.
            var rows = message.Data.ValueKind == JsonValueKind.Array ? message.Data.EnumerateArray().ToArray() : [message.Data];
            var update = new DeepCoinOrderBookUpdate
            {
                Asks = rows.SelectMany(row => BookSide(row, "a", native, OrderSide.Sell)).ToArray(),
                Bids = rows.SelectMany(row => BookSide(row, "b", native, OrderSide.Buy)).ToArray()
            };
            // V2 documents timestamps, not sequence IDs. Retain zero as unavailable.
            onMessage(Event(update, message, received, original, native, Timestamp(message.PublishTime == 0 ? message.TradeTime : message.PublishTime), type));
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

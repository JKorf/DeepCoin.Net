using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using DeepCoin.Net.Objects.Internal;
using Microsoft.Extensions.Logging;
using System;
using DeepCoin.Net.Enums;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace DeepCoin.Net.Objects.Sockets.Subscriptions;

/// <summary>
/// One native V2 public topic with explicit subscription acknowledgment.
/// </summary>
internal sealed class DeepCoinV2Subscription<TMessage> : Subscription where TMessage : DeepCoinV2SocketMessage
{
    #region Fields

    private readonly string _symbol;
    private readonly string _topic;
    private readonly KlineInterval? _period;
    private readonly Action<DateTime, string?, TMessage> _handler;
    private int _subscriptionId;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a V2 public topic subscription and its message route.
    /// </summary>
    public DeepCoinV2Subscription(ILogger logger, string symbol, string topic, string action, Action<DateTime, string?, TMessage> handler, KlineInterval? period = null)
        : base(logger, false)
    {
        _symbol = symbol;
        _topic = topic;
        _period = period;
        _handler = handler;
        MessageRouter = MessageRouter.CreateForEvent<TMessage>(action, period == null ? symbol : symbol + "_" + EnumConverter.GetString(period), HandleMessage);
    }

    #endregion

    #region Methods

    private CallResult HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, TMessage message)
    {
        _handler(receiveTime, originalData, message);
        return CallResult.Ok();
    }

    // V2 rejects Count=0; the documented candle request uses ten historical rows (maximum 100).
    private DeepCoinV2Query CreateQuery(V2SubscriptionAction action) => new(new DeepCoinV2SocketRequest
    {
        Action = action, Symbol = _symbol, Topic = _topic, RequestId = _subscriptionId, Period = _period, Count = _period == null ? null : 10
    });

    /// <inheritdoc />
    protected override Query GetSubQuery(SocketConnection connection)
    {
        _subscriptionId = ExchangeHelpers.NextId();
        return CreateQuery(V2SubscriptionAction.Subscribe);
    }

    /// <inheritdoc />
    // Live spot/swap streams require Action=0 with the original LocalNo to remove just this
    // subscription. The documented Action=2 returns unsupportedAction; a new LocalNo returns localIDNotExist.
    protected override Query GetUnsubQuery(SocketConnection connection) => CreateQuery(V2SubscriptionAction.Unsubscribe);

    #endregion

    #region Inline-Types

    private sealed class DeepCoinV2Query : Query<DeepCoinV2SocketMessage>
    {
        /// <summary>
        /// Initializes the acknowledgment query for a V2 subscription request.
        /// </summary>
        public DeepCoinV2Query(DeepCoinV2SocketRequest request) : base(request, false, 1)
        {
            MessageRouter = MessageRouter.CreateForQuery<DeepCoinV2SocketMessage>(request.RequestId.ToString(), HandleMessage);
        }

        private static CallResult<DeepCoinV2SocketMessage> HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, DeepCoinV2SocketMessage message)
            => string.Equals(message.Message, "Success", StringComparison.OrdinalIgnoreCase)
                ? CallResult<DeepCoinV2SocketMessage>.Ok(message, originalData)
                : CallResult<DeepCoinV2SocketMessage>.Fail(new ServerError(ErrorType.Unknown, message.Message), originalData);
    }

    #endregion
}

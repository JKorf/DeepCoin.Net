using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System;
using CryptoExchange.Net;
using DeepCoin.Net.Objects.Internal;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Sockets.Default;

namespace DeepCoin.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class DeepCoinSubscription<T> : Subscription
    {
        private readonly SocketApiClient _client;
        private readonly Action<DateTime, string?, SocketUpdate<T>> _handler;
        private readonly string _pushAction;
        private readonly string _filter;
        private readonly string _topic;
        private int _subId;

        /// <summary>
        /// ctor
        /// </summary>
        public DeepCoinSubscription(ILogger logger, SocketApiClient client, string pushAction, string filter, string topic, Action<DateTime, string?, SocketUpdate<T>> handler, bool auth) : base(logger, auth)
        {
            _client = client;
            _handler = handler;
            _pushAction = pushAction;
            _filter = "DeepCoin_" + filter;
            _topic = topic;

            MessageRouter = MessageRouter.CreateWithTopicFilter<SocketUpdate<T>>(
                pushAction, filter, DoHandleMessage);
        }

        /// <inheritdoc />
        protected override Query? GetSubQuery(SocketConnection connection)
        {
            _subId = ExchangeHelpers.NextId();
            return new DeepCoinQuery(_client, new Internal.SocketRequest
            {
                Action = "1",
                RequestId = _subId,
                Topic = _filter,
                TopicId = _topic,
                ResumeNumber = 0 //?
            }, false);
        }

        /// <inheritdoc />
        protected override Query? GetUnsubQuery(SocketConnection connection)
        {
            return new DeepCoinQuery(_client, new Internal.SocketRequest
            {
                Action = "2",
                RequestId = _subId,
                Topic = _filter,
                TopicId = _topic,
                ResumeNumber = 0 //?
            }, false);
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, SocketUpdate<T> message)
        {
            _handler.Invoke(receiveTime, originalData, message);
            return CallResult.SuccessResult;
        }
    }
}

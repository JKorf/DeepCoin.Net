using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using DeepCoin.Net.Objects.Models;
using CryptoExchange.Net;
using DeepCoin.Net.Objects.Internal;

namespace DeepCoin.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class DeepCoinSubscription<T> : Subscription<SocketResponse, SocketResponse>
    {
        /// <inheritdoc />
        public override HashSet<string> ListenerIdentifiers { get; set; }

        private readonly Action<DataEvent<IEnumerable<TableData<T>>>> _handler;
        private readonly string _pushAction;
        private readonly string _filter;
        private readonly string _topic;
        private int _subId;

        /// <inheritdoc />
        public override Type? GetMessageType(IMessageAccessor message)
        {
            return typeof(SocketUpdate<T>);
        }

        /// <summary>
        /// ctor
        /// </summary>
        public DeepCoinSubscription(ILogger logger, string pushAction, string filter, string topic, Action<DataEvent<IEnumerable<TableData<T>>>> handler, bool auth) : base(logger, auth)
        {
            _handler = handler;
            _pushAction = pushAction;
            _filter = filter;
            _topic = topic;

            ListenerIdentifiers = new HashSet<string>() { pushAction + filter, pushAction + "SwapU," + filter };
        }

        /// <inheritdoc />
        public override Query? GetSubQuery(SocketConnection connection)
        {
            _subId = ExchangeHelpers.NextId();
            return new DeepCoinQuery(new Internal.SocketRequest
            {
                Action = "1",
                RequestId = _subId,
                Topic = _filter,
                TopicId = _topic,
                ResumeNumber = 0 //?
            }, false);
        }

        /// <inheritdoc />
        public override Query? GetUnsubQuery()
        {
            return new DeepCoinQuery(new Internal.SocketRequest
            {
                Action = "2",
                RequestId = _subId,
                Topic = _filter,
                TopicId = _topic,
                ResumeNumber = 0 //?
            }, false);
        }

        /// <inheritdoc />
        public override CallResult DoHandleMessage(SocketConnection connection, DataEvent<object> message)
        {
            var data = (SocketUpdate<T>)message.Data!;
            _handler.Invoke(message.As(data.Result, data.Action, null, SocketUpdateType.Update));
            return new CallResult(null);
        }
    }
}

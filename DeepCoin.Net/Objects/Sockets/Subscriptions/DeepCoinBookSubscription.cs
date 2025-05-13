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
using System.Linq;
using System.Diagnostics;

namespace DeepCoin.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class DeepCoinBookSubscription : Subscription<SocketResponse, SocketResponse>
    {
        /// <inheritdoc />
        public override HashSet<string> ListenerIdentifiers { get; set; }

        private readonly Action<DataEvent<DeepCoinOrderBookUpdate>> _handler;
        private readonly string _filter;
        private readonly string _topic;
        private readonly string _table;
        private int _subId;
        private DeepCoinOrderBookUpdate? _incompleteUpdate;

        /// <inheritdoc />
        public override Type? GetMessageType(IMessageAccessor message)
        {
            return typeof(SocketUpdate<DeepCoinOrderBookUpdateEntry>);
        }

        /// <summary>
        /// ctor
        /// </summary>
        public DeepCoinBookSubscription(ILogger logger, string pushAction, string table, string filter, string topic, Action<DataEvent<DeepCoinOrderBookUpdate>> handler, bool auth) : base(logger, auth)
        {
            _handler = handler;
            _filter = filter;
            _topic = topic;
            _table = table;

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
            var data = (SocketUpdate<DeepCoinOrderBookUpdateEntry>)message.Data!;
            var update = new DeepCoinOrderBookUpdate
            {
                SequenceNumber = data.BusinessNumber,
                Asks = data.Result.Where(x => x.Table.Equals(_table) && x.Data.Direction == Enums.OrderSide.Sell).Select(x => x.Data).ToArray(),
                Bids = data.Result.Where(x => x.Table.Equals(_table) && x.Data.Direction == Enums.OrderSide.Buy).Select(x => x.Data).ToArray()
            };

            // An update only seems to be complete when the last table is of the "CurrentTime" table.
            // If an update doesn't have that there will be another update message with the same sequence number
            var complete = data.Result.Any(x => x.Table == "CurrentTime") || data.BusinessNumber == 0;
            if (!complete)
            {
                // Cache this incomplete update, we need the next message to complete it
                _incompleteUpdate = update;
                return CallResult.SuccessResult;
            }

            if (_incompleteUpdate != null)
            {
                if (_incompleteUpdate.SequenceNumber != update.SequenceNumber)
                {
                    // We have a cached update, but the next message is not the same sequence?
                    _handler.Invoke(message.As(update, data.Action, data.Result.First().Data.Symbol, data.BusinessNumber == 0 ? SocketUpdateType.Snapshot : SocketUpdateType.Update));
                    _incompleteUpdate = null;
                }
                else
                {
                    // Complete the cached update
                    var newAsks = _incompleteUpdate.Asks.ToList();
                    var newBids = _incompleteUpdate.Bids.ToList();
                    foreach (var ask in update.Asks)
                        newAsks.Add(ask);
                    foreach (var bid in update.Bids)
                        newBids.Add(bid);

                    update.Asks = newAsks.ToArray();
                    update.Bids = newBids.ToArray();

                    _incompleteUpdate = null;
                }
            }

            _handler.Invoke(message.As(update, data.Action, data.Result.First().Data.Symbol, data.BusinessNumber == 0 ? SocketUpdateType.Snapshot : SocketUpdateType.Update));
            return CallResult.SuccessResult;
        }
    }
}

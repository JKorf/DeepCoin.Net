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
using CryptoExchange.Net.Clients;

namespace DeepCoin.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class DeepCoinBookSubscription : Subscription
    {
        private readonly SocketApiClient _client;
        private readonly Action<DataEvent<DeepCoinOrderBookUpdate>> _handler;
        private readonly string _filter;
        private readonly string _topic;
        private readonly string _table;
        private int _subId;
        private DeepCoinOrderBookUpdate? _incompleteUpdate;

        /// <summary>
        /// ctor
        /// </summary>
        public DeepCoinBookSubscription(ILogger logger, SocketApiClient client, string pushAction, string table, string filter, string topic, Action<DataEvent<DeepCoinOrderBookUpdate>> handler, bool auth) : base(logger, auth)
        {
            _client = client;
            _handler = handler;
            _filter = filter;
            _topic = topic;
            _table = table;

            MessageMatcher = MessageMatcher.Create<SocketUpdate<DeepCoinOrderBookUpdateEntry>>([pushAction + filter, pushAction + "SwapU," + filter], DoHandleMessage);
            MessageRouter = MessageRouter.CreateWithoutTopicFilter<SocketUpdate<DeepCoinOrderBookUpdateEntry>>([pushAction + filter, pushAction + "SwapU," + filter], DoHandleMessage);
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
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, SocketUpdate<DeepCoinOrderBookUpdateEntry> message)
        {
            var update = new DeepCoinOrderBookUpdate
            {
                SequenceNumber = message.BusinessNumber,
                Asks = message.Result.Where(x => x.Table.Equals(_table) && x.Data.Direction == Enums.OrderSide.Sell).Select(x => x.Data).ToArray(),
                Bids = message.Result.Where(x => x.Table.Equals(_table) && x.Data.Direction == Enums.OrderSide.Buy).Select(x => x.Data).ToArray()
            };

            // An update only seems to be complete when the last table is of the "CurrentTime" table.
            // If an update doesn't have that there will be another update message with the same sequence number
            var complete = message.Result.Any(x => x.Table == "CurrentTime") || message.BusinessNumber == 0;
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
                    _handler.Invoke(new DataEvent<DeepCoinOrderBookUpdate>(update, receiveTime, originalData)
                        .WithStreamId(message.Action)
                        .WithSymbol(message.Result.First().Data.Symbol)
                        .WithUpdateType(message.BusinessNumber == 0 ? SocketUpdateType.Snapshot : SocketUpdateType.Update));
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

            _handler.Invoke(new DataEvent<DeepCoinOrderBookUpdate>(update, receiveTime, originalData)
                .WithStreamId(message.Action)
                .WithSymbol(message.Result.First().Data.Symbol)
                .WithUpdateType(message.BusinessNumber == 0 ? SocketUpdateType.Snapshot : SocketUpdateType.Update));
            return CallResult.SuccessResult;
        }
    }
}

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
using CryptoExchange.Net.Converters.MessageParsing;

namespace DeepCoin.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class DeepCoinUserSubscription : Subscription<SocketResponse, SocketResponse>
    {
        private static readonly MessagePath _actionPath = MessagePath.Get().Property("action");

        /// <inheritdoc />
        public override HashSet<string> ListenerIdentifiers { get; set; }

        private readonly Action<DataEvent<DeepCoinOrderUpdate[]>>? _orderUpdateHandler;
        private readonly Action<DataEvent<DeepCoinBalanceUpdate[]>>? _balanceUpdateHandler;
        private readonly Action<DataEvent<DeepCoinPositionUpdate[]>>? _positionUpdateHandler;
        private readonly Action<DataEvent<DeepCoinUserTradeUpdate[]>>? _userTradeUpdateHandler;
        private readonly Action<DataEvent<DeepCoinAccountUpdate[]>>? _accountUpdateHandler;
        private readonly Action<DataEvent<DeepCoinTriggerOrderUpdate[]>>? _triggerUpdateHandler;

        /// <inheritdoc />
        public override Type? GetMessageType(IMessageAccessor message)
        {
            var action = message.GetValue<string>(_actionPath)!;
            if (action.Equals("PushOrder", StringComparison.OrdinalIgnoreCase))
                return typeof(SocketUpdate<DeepCoinOrderUpdate>);
            if (action.Equals("PushAccount", StringComparison.OrdinalIgnoreCase))
                return typeof(SocketUpdate<DeepCoinBalanceUpdate>);
            if (action.Equals("PushPosition", StringComparison.OrdinalIgnoreCase))
                return typeof(SocketUpdate<DeepCoinPositionUpdate>);
            if (action.Equals("PushTrade", StringComparison.OrdinalIgnoreCase))
                return typeof(SocketUpdate<DeepCoinUserTradeUpdate>);
            if (action.Equals("PushAccountDetail", StringComparison.OrdinalIgnoreCase))
                return typeof(SocketUpdate<DeepCoinAccountUpdate>);
            if (action.Equals("PushTriggerOrder", StringComparison.OrdinalIgnoreCase))
                return typeof(SocketUpdate<DeepCoinTriggerOrderUpdate>);

            return null;
        }

        /// <summary>
        /// ctor
        /// </summary>
        public DeepCoinUserSubscription(
            ILogger logger,
            Action<DataEvent<DeepCoinOrderUpdate[]>>? orderUpdate,
            Action<DataEvent<DeepCoinBalanceUpdate[]>>? balanceUpdate,
            Action<DataEvent<DeepCoinPositionUpdate[]>>? positionUpdate,
            Action<DataEvent<DeepCoinUserTradeUpdate[]>>? userTradeUpdate,
            Action<DataEvent<DeepCoinAccountUpdate[]>>? accountUpdate,
            Action<DataEvent<DeepCoinTriggerOrderUpdate[]>>? triggerUpdateHandler
            ) : base(logger, false)
        {
            _orderUpdateHandler = orderUpdate;
            _balanceUpdateHandler = balanceUpdate;
            _positionUpdateHandler = positionUpdate;
            _userTradeUpdateHandler = userTradeUpdate;
            _accountUpdateHandler = accountUpdate;
            _triggerUpdateHandler = triggerUpdateHandler;

            ListenerIdentifiers = new HashSet<string>() { "PushOrder", "PushAccount", "PushPosition", "PushTrade", "PushAccountDetail", "PushTriggerOrder" };
        }

        /// <inheritdoc />
        public override Query? GetSubQuery(SocketConnection connection) => null;

        /// <inheritdoc />
        public override Query? GetUnsubQuery() => null;

        /// <inheritdoc />
        public override CallResult DoHandleMessage(SocketConnection connection, DataEvent<object> message)
        {
            if (message.Data is SocketUpdate<DeepCoinOrderUpdate> orderUpdate)
                _orderUpdateHandler?.Invoke(message.As(orderUpdate.Result.Select(x => x.Data).ToArray()).WithSymbol(orderUpdate.Result.First().Data.Symbol).WithDataTimestamp(orderUpdate.Result.Max(x => x.Data.UpdateTime)));
            if (message.Data is SocketUpdate<DeepCoinBalanceUpdate> balanceUpdate)
                _balanceUpdateHandler?.Invoke(message.As(balanceUpdate.Result.Select(x => x.Data).ToArray()));
            if (message.Data is SocketUpdate<DeepCoinPositionUpdate> positionUpdate)
                _positionUpdateHandler?.Invoke(message.As(positionUpdate.Result.Select(x => x.Data).ToArray()).WithSymbol(positionUpdate.Result.First().Data.Symbol).WithDataTimestamp(positionUpdate.Result.Max(x => x.Data.UpdateTime)));
            if (message.Data is SocketUpdate<DeepCoinUserTradeUpdate> userTradeUpdate)
                _userTradeUpdateHandler?.Invoke(message.As(userTradeUpdate.Result.Select(x => x.Data).ToArray()).WithSymbol(userTradeUpdate.Result.First().Data.Symbol).WithDataTimestamp(userTradeUpdate.Result.Max(x => x.Data.TradeTime)));
            if (message.Data is SocketUpdate<DeepCoinAccountUpdate> accountUpdate)
                _accountUpdateHandler?.Invoke(message.As(accountUpdate.Result.Select(x => x.Data).ToArray()).WithDataTimestamp(accountUpdate.Result.Max(x => x.Data.CreateTime)));
            if (message.Data is SocketUpdate<DeepCoinTriggerOrderUpdate> triggerOrderUpdate)
                _triggerUpdateHandler?.Invoke(message.As(triggerOrderUpdate.Result.Select(x => x.Data).ToArray()).WithSymbol(triggerOrderUpdate.Result.First().Data.Symbol).WithDataTimestamp(triggerOrderUpdate.Result.Max(x => x.Data.UpdateTime)));

            return new CallResult(null);
        }
    }
}

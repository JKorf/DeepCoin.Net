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

        private readonly Action<DataEvent<DeepCoinOrderUpdate[]>>? _orderUpdateHandler;
        private readonly Action<DataEvent<DeepCoinBalanceUpdate[]>>? _balanceUpdateHandler;
        private readonly Action<DataEvent<DeepCoinPositionUpdate[]>>? _positionUpdateHandler;
        private readonly Action<DataEvent<DeepCoinUserTradeUpdate[]>>? _userTradeUpdateHandler;
        private readonly Action<DataEvent<DeepCoinAccountUpdate[]>>? _accountUpdateHandler;
        private readonly Action<DataEvent<DeepCoinTriggerOrderUpdate[]>>? _triggerUpdateHandler;

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

            MessageMatcher = MessageMatcher.Create([
                new MessageHandlerLink<SocketUpdate<DeepCoinOrderUpdate>>("PushOrder", DoHandleMessage),
                new MessageHandlerLink<SocketUpdate<DeepCoinBalanceUpdate>>("PushAccount", DoHandleMessage),
                new MessageHandlerLink<SocketUpdate<DeepCoinPositionUpdate>>("PushPosition", DoHandleMessage),
                new MessageHandlerLink<SocketUpdate<DeepCoinUserTradeUpdate>>("PushTrade", DoHandleMessage),
                new MessageHandlerLink<SocketUpdate<DeepCoinAccountUpdate>>("PushAccountDetail", DoHandleMessage),
                new MessageHandlerLink<SocketUpdate<DeepCoinTriggerOrderUpdate>>("PushTriggerOrder", DoHandleMessage)
                ]);
        }

        /// <inheritdoc />
        public override Query? GetSubQuery(SocketConnection connection) => null;

        /// <inheritdoc />
        public override Query? GetUnsubQuery() => null;

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<SocketUpdate<DeepCoinOrderUpdate>> message)
        {
            _orderUpdateHandler?.Invoke(message.As(message.Data.Result.Select(x => x.Data).ToArray()).WithSymbol(message.Data.Result.First().Data.Symbol).WithDataTimestamp(message.Data.Result.Max(x => x.Data.UpdateTime)));
            return CallResult.SuccessResult;
        }

        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<SocketUpdate<DeepCoinBalanceUpdate>> message)
        {
            _balanceUpdateHandler?.Invoke(message.As(message.Data.Result.Select(x => x.Data).ToArray()));
            return CallResult.SuccessResult;
        }
        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<SocketUpdate<DeepCoinPositionUpdate>> message)
        {
            _positionUpdateHandler?.Invoke(message.As(message.Data.Result.Select(x => x.Data).ToArray()).WithSymbol(message.Data.Result.First().Data.Symbol).WithDataTimestamp(message.Data.Result.Max(x => x.Data.UpdateTime)));
            return CallResult.SuccessResult;
        }
        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<SocketUpdate<DeepCoinUserTradeUpdate>> message)
        {
            _userTradeUpdateHandler?.Invoke(message.As(message.Data.Result.Select(x => x.Data).ToArray()).WithSymbol(message.Data.Result.First().Data.Symbol).WithDataTimestamp(message.Data.Result.Max(x => x.Data.TradeTime)));
            return CallResult.SuccessResult;
        }
        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<SocketUpdate<DeepCoinAccountUpdate>> message)
        {
            _accountUpdateHandler?.Invoke(message.As(message.Data.Result.Select(x => x.Data).ToArray()).WithDataTimestamp(message.Data.Result.Max(x => x.Data.CreateTime)));
            return CallResult.SuccessResult;
        }
        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<SocketUpdate<DeepCoinTriggerOrderUpdate>> message)
        {
            _triggerUpdateHandler?.Invoke(message.As(message.Data.Result.Select(x => x.Data).ToArray()).WithSymbol(message.Data.Result.First().Data.Symbol).WithDataTimestamp(message.Data.Result.Max(x => x.Data.UpdateTime)));
            return CallResult.SuccessResult;
        }
    }
}

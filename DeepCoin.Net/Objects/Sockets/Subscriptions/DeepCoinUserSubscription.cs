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
        protected override Query? GetSubQuery(SocketConnection connection) => null;

        /// <inheritdoc />
        protected override Query? GetUnsubQuery(SocketConnection connection) => null;

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, SocketUpdate<DeepCoinOrderUpdate> message)
        {
            _orderUpdateHandler?.Invoke(
                new DataEvent<DeepCoinOrderUpdate[]>(message.Result.Select(x => x.Data).ToArray(), receiveTime, originalData)
                    .WithSymbol(message.Result.First().Data.Symbol)
                    .WithDataTimestamp(message.Result.Max(x => x.Data.UpdateTime))
                );
            return CallResult.SuccessResult;
        }

        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, SocketUpdate<DeepCoinBalanceUpdate> message)
        {
            _balanceUpdateHandler?.Invoke(
                new DataEvent<DeepCoinBalanceUpdate[]>(message.Result.Select(x => x.Data).ToArray(), receiveTime, originalData)
                );
            return CallResult.SuccessResult;
        }
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, SocketUpdate<DeepCoinPositionUpdate> message)
        {
            _positionUpdateHandler?.Invoke(
                new DataEvent<DeepCoinPositionUpdate[]>(message.Result.Select(x => x.Data).ToArray(), receiveTime, originalData)
                    .WithSymbol(message.Result.First().Data.Symbol)
                    .WithDataTimestamp(message.Result.Max(x => x.Data.UpdateTime))
                );
            return CallResult.SuccessResult;
        }
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, SocketUpdate<DeepCoinUserTradeUpdate> message)
        {
            _userTradeUpdateHandler?.Invoke(
                new DataEvent<DeepCoinUserTradeUpdate[]>(message.Result.Select(x => x.Data).ToArray(), receiveTime, originalData)
                    .WithSymbol(message.Result.First().Data.Symbol)
                    .WithDataTimestamp(message.Result.Max(x => x.Data.TradeTime))
                );
            return CallResult.SuccessResult;
        }
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, SocketUpdate<DeepCoinAccountUpdate> message)
        {
            _accountUpdateHandler?.Invoke(
                new DataEvent<DeepCoinAccountUpdate[]>(message.Result.Select(x => x.Data).ToArray(), receiveTime, originalData)
                    .WithSymbol(message.Result.First().Data.Symbol)
                    .WithDataTimestamp(message.Result.Max(x => x.Data.CreateTime))
                );
            return CallResult.SuccessResult;
        }
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, SocketUpdate<DeepCoinTriggerOrderUpdate> message)
        {
            _triggerUpdateHandler?.Invoke(
                new DataEvent<DeepCoinTriggerOrderUpdate[]>(message.Result.Select(x => x.Data).ToArray(), receiveTime, originalData)
                    .WithSymbol(message.Result.First().Data.Symbol)
                    .WithDataTimestamp(message.Result.Max(x => x.Data.CreateTime))
                );
            return CallResult.SuccessResult;
        }
    }
}

using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;
using DeepCoin.Net.Objects.Models;
using DeepCoin.Net.Objects.Internal;
using CryptoExchange.Net.Clients;
using System;

namespace DeepCoin.Net.Objects.Sockets
{
    internal class DeepCoinQuery : Query<SocketResponse>
    {
        private readonly SocketApiClient _client;

        public DeepCoinQuery(SocketApiClient client, SocketRequest request, bool authenticated, int weight = 1) : base(new Dictionary<string, object>
        {
            { "SendTopicAction", request }
        }, authenticated, weight)
        {
            _client = client;
            MessageMatcher = MessageMatcher.Create<SocketResponse>(request.RequestId.ToString(), HandleMessage);
            MessageRouter = MessageRouter.Create<SocketResponse>(request.RequestId.ToString(), HandleMessage);
        }

        public CallResult<SocketResponse> HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, SocketResponse message)
        {
            if (message.ErrorCode != 0)
                return new CallResult<SocketResponse>(new ServerError(message.ErrorCode, _client.GetErrorInfo(message.ErrorCode, message.ErrorMessage)));

            return new CallResult<SocketResponse>(message, originalData, null);
        }
    }
}

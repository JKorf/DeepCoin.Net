using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;
using DeepCoin.Net.Objects.Internal;
using CryptoExchange.Net.Clients;
using System;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;

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
            MessageRouter = MessageRouter.CreateForQuery<SocketResponse>(request.RequestId.ToString(), HandleMessage);
        }

        public CallResult<SocketResponse> HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, SocketResponse message)
        {
            if (message.ErrorCode != 0)
                return CallResult<SocketResponse>.Fail(new ServerError(message.ErrorCode, _client.GetErrorInfo(message.ErrorCode, message.ErrorMessage)), originalData);

            return CallResult<SocketResponse>.Ok(message, originalData);
        }
    }
}

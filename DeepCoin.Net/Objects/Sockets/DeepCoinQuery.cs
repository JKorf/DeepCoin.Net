using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;
using DeepCoin.Net.Objects.Models;
using DeepCoin.Net.Objects.Internal;
using CryptoExchange.Net.Clients;

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
        }

        public CallResult<SocketResponse> HandleMessage(SocketConnection connection, DataEvent<SocketResponse> message)
        {
            var result = message.Data;
            if (result.ErrorCode != 0)
                return message.ToCallResult<SocketResponse>(new ServerError(result.ErrorCode, _client.GetErrorInfo(result.ErrorCode, result.ErrorMessage)));

            return message.ToCallResult();
        }
    }
}

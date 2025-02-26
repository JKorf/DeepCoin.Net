using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;
using DeepCoin.Net.Objects.Models;
using DeepCoin.Net.Objects.Internal;

namespace DeepCoin.Net.Objects.Sockets
{
    internal class DeepCoinQuery : Query<SocketResponse>
    {
        public override HashSet<string> ListenerIdentifiers { get; set; }

        public DeepCoinQuery(SocketRequest request, bool authenticated, int weight = 1) : base(new Dictionary<string, object>
        {
            { "SendTopicAction", request }
        }, authenticated, weight)
        {
            ListenerIdentifiers = new HashSet<string> { request.RequestId.ToString() };
        }

        public override CallResult<SocketResponse> HandleMessage(SocketConnection connection, DataEvent<SocketResponse> message)
        {
            var result = message.Data;
            if (result.ErrorCode != 0)
                return message.ToCallResult<SocketResponse>(new ServerError(result.ErrorCode, result.ErrorMessage));

            return message.ToCallResult();
        }
    }
}

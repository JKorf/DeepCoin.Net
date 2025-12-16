using CryptoExchange.Net.Sockets;
using System;

namespace DeepCoin.Net.Objects.Sockets
{
    internal class DeepCoinPingQuery : Query<string>
    {
        public DeepCoinPingQuery() : base("ping", false, 1)
        {
            RequestTimeout = TimeSpan.FromSeconds(5);
            MessageMatcher = MessageMatcher.Create<string>("pong");
            MessageRouter = MessageRouter.CreateWithoutHandler<string>("pong");
        }
    }
}

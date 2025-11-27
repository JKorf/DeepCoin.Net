using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;
using DeepCoin.Net.Objects.Models;
using DeepCoin.Net.Objects.Internal;
using System;

namespace DeepCoin.Net.Objects.Sockets
{
    internal class DeepCoinPingQuery : Query<string>
    {
        public DeepCoinPingQuery() : base("ping", false, 1)
        {
            RequestTimeout = TimeSpan.FromSeconds(5);
            MessageMatcher = MessageMatcher.Create<string>("pong");
            MessageRouter = MessageRouter.Create<string>("pong");
        }
    }
}

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
        public override HashSet<string> ListenerIdentifiers { get; set; }

        public DeepCoinPingQuery() : base("ping", false, 1)
        {
            ListenerIdentifiers = new HashSet<string> { "pong" };
            RequestTimeout = TimeSpan.FromSeconds(5);
        }

        public override CallResult<string> HandleMessage(SocketConnection connection, DataEvent<string> message)
        {
            return message.ToCallResult();
        }
    }
}

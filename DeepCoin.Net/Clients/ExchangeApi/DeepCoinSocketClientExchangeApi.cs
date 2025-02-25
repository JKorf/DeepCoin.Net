using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using DeepCoin.Net.Interfaces.Clients.ExchangeApi;
using DeepCoin.Net.Objects.Models;
using DeepCoin.Net.Objects.Options;
using DeepCoin.Net.Objects.Sockets.Subscriptions;
using DeepCoin.Net.Objects;
using System.Linq;
using CryptoExchange.Net;
using DeepCoin.Net.Objects.Internal;
using DeepCoin.Net.Objects.Sockets;

namespace DeepCoin.Net.Clients.ExchangeApi
{
    /// <summary>
    /// Client providing access to the DeepCoin Exchange websocket Api
    /// </summary>
    internal partial class DeepCoinSocketClientExchangeApi : SocketApiClient, IDeepCoinSocketClientExchangeApi
    {
        #region fields
        private static readonly MessagePath _actionPath = MessagePath.Get().Property("action");
        private static readonly MessagePath _actionIdPath = MessagePath.Get().Property("result").Index(0).Property("data").Property("LocalNo");
        private static readonly MessagePath _indexPath = MessagePath.Get().Property("index");
        #endregion

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal DeepCoinSocketClientExchangeApi(ILogger logger, DeepCoinSocketOptions options) :
            base(logger, options.Environment.SocketClientAddress!, options, options.ExchangeOptions)
        {
            RegisterPeriodicQuery("ping",
                TimeSpan.FromSeconds(30), 
                (x) => new DeepCoinPingQuery(), 
                (connection, result) =>
                {
                    if (result.Error?.Message.Equals("Query timeout") == true)
                    {
                        // Ping timeout, reconnect
                        _logger.LogWarning("[Sckt {SocketId}] Ping response timeout, reconnecting", connection.SocketId);
                        _ = connection.TriggerReconnectAsync();
                    }
                });
        }
        #endregion

        /// <inheritdoc />
        protected override IByteMessageAccessor CreateAccessor() => new SystemTextJsonByteMessageAccessor();
        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer();

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new DeepCoinAuthenticationProvider((DeepCoinApiCredentials)credentials);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolUpdatesAsync(string symbol, Action<DataEvent<DeepCoinSymbolUpdate>> onMessage, CancellationToken ct = default)
        {
            var subscription = new DeepCoinSubscription<DeepCoinSymbolUpdate>(_logger, "PushMarketDataOverView", "DeepCoin_" + symbol, "7", x => onMessage(x.As(x.Data.First().Data).WithSymbol(x.Data.First().Data.Symbol)), false);
            return await SubscribeAsync(BaseAddress.AppendPath("public/ws"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<DeepCoinTradeUpdate>> onMessage, CancellationToken ct = default)
        {
            var subscription = new DeepCoinSubscription<DeepCoinTradeUpdate>(_logger, "PushMarketTrade", "DeepCoin_" + symbol, "2", x => onMessage(
                x.As(x.Data.First().Data)
                .WithSymbol(x.Data.First().Data.Symbol)
                .WithDataTimestamp(x.Data.Max(x => x.Data.Timestamp))
                ), false);
            return await SubscribeAsync(BaseAddress.AppendPath("public/ws"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override string? GetListenerIdentifier(IMessageAccessor message)
        {
            if (!message.IsJson)
                return "pong";

            var action = message.GetValue<string>(_actionPath);
            if (action.Equals("RecvTopicAction", StringComparison.InvariantCulture))
            {
                var id = message.GetValue<int?>(_actionIdPath);
                return id?.ToString();
            }

            var index = message.GetValue<string>(_indexPath);
            return action + index;
        }

        /// <inheritdoc />
        protected override Task<Query?> GetAuthenticationRequestAsync(SocketConnection connection) => Task.FromResult<Query?>(null);

        /// <inheritdoc />
        public IDeepCoinSocketClientExchangeApiShared SharedClient => this;

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => DeepCoinExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);
    }
}

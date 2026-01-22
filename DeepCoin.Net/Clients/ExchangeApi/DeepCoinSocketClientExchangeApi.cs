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
using System.Linq;
using CryptoExchange.Net;
using DeepCoin.Net.Objects.Internal;
using DeepCoin.Net.Objects.Sockets;
using System.Net.WebSockets;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using DeepCoin.Net.Clients.MessageHandlers;
using CryptoExchange.Net.Sockets.Default;

namespace DeepCoin.Net.Clients.ExchangeApi
{
    /// <summary>
    /// Client providing access to the DeepCoin Exchange websocket Api
    /// </summary>
    internal partial class DeepCoinSocketClientExchangeApi : SocketApiClient, IDeepCoinSocketClientExchangeApi
    {
        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal DeepCoinSocketClientExchangeApi(ILogger logger, DeepCoinSocketOptions options) :
            base(logger, options.Environment.SocketClientAddress!, options, options.ExchangeOptions)
        {
            KeepAliveInterval = TimeSpan.Zero;

            RegisterPeriodicQuery("ping",
                TimeSpan.FromSeconds(30), 
                (x) => new DeepCoinPingQuery(), 
                (connection, result) =>
                {
                    if (connection.ConnectionUri.AbsolutePath.Contains("v1/private"))
                        // Private endpoint doesn't return pong on pings
                        return;

                    if (result.Error?.ErrorType == ErrorType.Timeout)
                    {
                        // Ping timeout, reconnect
                        _logger.LogWarning("[Sckt {SocketId}] Ping response timeout, reconnecting", connection.SocketId);
                        _ = connection.TriggerReconnectAsync();
                    }
                });
        }
        #endregion

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(DeepCoinExchange._serializerContext));

        public override ISocketMessageHandler CreateMessageConverter(WebSocketMessageType messageType) => new DeepCoinSocketMessageHandler();

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new DeepCoinAuthenticationProvider(credentials);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolUpdatesAsync(string symbol, Action<DataEvent<DeepCoinSymbolUpdate>> onMessage, CancellationToken ct = default)
        {
            string path;
            if (symbol.EndsWith("-SWAP", StringComparison.InvariantCultureIgnoreCase))
            {
                path = "public/ws";
                symbol = symbol.Replace("-SWAP", "").Replace("-", "");
            }
            else
            {
                path = "public/spotws";
                symbol = symbol.Replace("-", "/");
            }

            var handler = new Action<DateTime, string?, SocketUpdate<DeepCoinSymbolUpdate>>((receiveTime, originalData, data) =>
            {
                var timestamp = data.Result.Max(x => x.Data.UpdateTime);
                UpdateTimeOffset(timestamp);

                onMessage(
                    new DataEvent<DeepCoinSymbolUpdate>(DeepCoinExchange.ExchangeName, data.Result.Where(x => x.Table.Equals("MarketDataOverView")).First().Data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Action)
                        .WithDataTimestamp(timestamp, GetTimeOffset())
                    );
            });
            var subscription = new DeepCoinSubscription<DeepCoinSymbolUpdate>(_logger, this, "PushMarketDataOverView", symbol, "7", handler, false);
            return await SubscribeAsync(BaseAddress.AppendPath(path), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<DeepCoinTradeUpdate>> onMessage, CancellationToken ct = default)
        {
            string path;
            if (symbol.EndsWith("-SWAP", StringComparison.InvariantCultureIgnoreCase))
            {
                path = "public/ws";
                symbol = symbol.Replace("-SWAP", "").Replace("-", "");
            }
            else
            {
                path = "public/spotws";
                symbol = symbol.Replace("-", "/");
            }

            var handler = new Action<DateTime, string?, SocketUpdate<DeepCoinTradeUpdate>>((receiveTime, originalData, data) =>
            {
                var updateData = data.Result.Where(x => x.Table.Equals("MarketTrade")).First().Data;
                UpdateTimeOffset(updateData.Timestamp);

                onMessage(
                    new DataEvent<DeepCoinTradeUpdate>(DeepCoinExchange.ExchangeName, updateData, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Action)
                        .WithSymbol(updateData.Symbol)
                        .WithDataTimestamp(updateData.Timestamp, GetTimeOffset())
                    );
            });
            var subscription = new DeepCoinSubscription<DeepCoinTradeUpdate>(_logger, this, "PushMarketTrade", symbol, "2", handler, false);
            return await SubscribeAsync(BaseAddress.AppendPath(path), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, Action<DataEvent<DeepCoinKlineUpdate>> onMessage, CancellationToken ct = default)
        {
            string path;
            if (symbol.EndsWith("-SWAP", StringComparison.InvariantCultureIgnoreCase))
            {
                path = "public/ws";
                symbol = symbol.Replace("-SWAP", "").Replace("-", "");
            }
            else
            {
                path = "public/spotws";
                symbol = symbol.Replace("-", "/");
            }

            var handler = new Action<DateTime, string?, SocketUpdate<DeepCoinKlineUpdate>>((receiveTime, originalData, data) =>
            {
                var updateData = data.Result.Where(x => x.Table.Equals("KLine") || x.Table.Equals("LastKLine")).First().Data;
                UpdateTimeOffset(updateData.UpdateTime);
                onMessage(
                    new DataEvent<DeepCoinKlineUpdate>(DeepCoinExchange.ExchangeName, updateData, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Action)
                        .WithSymbol(updateData.Symbol)
                        .WithDataTimestamp(updateData.UpdateTime, GetTimeOffset())
                    );
            });
            var topic = symbol + "_1m";
            var subscription = new DeepCoinSubscription<DeepCoinKlineUpdate>(_logger, this, "PushKLine", topic, "11", handler, false);
            return await SubscribeAsync(BaseAddress.AppendPath(path), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, Action<DataEvent<DeepCoinOrderBookUpdate>> onMessage, CancellationToken ct = default)
        {
            string path;
            if (symbol.EndsWith("-SWAP", StringComparison.InvariantCultureIgnoreCase))
            {
                path = "public/ws";
                symbol = symbol.Replace("-SWAP", "").Replace("-", "");
            }
            else
            {
                path = "public/spotws";
                symbol = symbol.Replace("-", "/");
            }

            var subscription = new DeepCoinBookSubscription(_logger, this, "PushMarketOrder", "MarketOrder", symbol, "25", onMessage, false);
            return await SubscribeAsync(BaseAddress.AppendPath(path), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToUserDataUpdatesAsync(
            string listenKey,
            Action<DataEvent<DeepCoinOrderUpdate[]>>? onOrderMessage = null, 
            Action<DataEvent<DeepCoinBalanceUpdate[]>>? onBalanceMessage = null, 
            Action<DataEvent<DeepCoinPositionUpdate[]>>? onPositionMessage = null, 
            Action<DataEvent<DeepCoinUserTradeUpdate[]>>? onUserTradeMessage = null, 
            Action<DataEvent<DeepCoinAccountUpdate[]>>? onAccountMessage = null, 
            Action<DataEvent<DeepCoinTriggerOrderUpdate[]>>? onTriggerOrderMessage = null, 
            CancellationToken ct = default)
        {
            var subscription = new DeepCoinUserSubscription(_logger, this, onOrderMessage, onBalanceMessage, onPositionMessage, onUserTradeMessage, onAccountMessage, onTriggerOrderMessage);
            return await SubscribeAsync(BaseAddress.AppendPath("v1/private?listenKey=" + listenKey), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public IDeepCoinSocketClientExchangeApiShared SharedClient => this;

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => DeepCoinExchange.FormatWebsocketSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);
    }
}

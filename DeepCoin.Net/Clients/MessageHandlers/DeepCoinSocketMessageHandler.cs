using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using DeepCoin.Net.Objects.Internal;
using DeepCoin.Net.Objects.Models;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text.Json;

namespace DeepCoin.Net.Clients.MessageHandlers
{
    internal class DeepCoinSocketMessageHandler : JsonSocketMessageHandler
    {
        public override JsonSerializerOptions Options { get; } = SerializerOptions.WithConverters(DeepCoinExchange._serializerContext);

        public DeepCoinSocketMessageHandler()
        {
            AddTopicMapping<SocketUpdate<DeepCoinSymbolUpdate>>(x => x.Result.First().Data.Symbol);
            AddTopicMapping<SocketUpdate<DeepCoinTradeUpdate>>(x => x.Result.First().Data.Symbol);
            AddTopicMapping<SocketUpdate<DeepCoinKlineUpdate>>(x => x.Result.First().Data.Symbol + "_1m");
            //AddTopicMapping<SocketUpdate<DeepCoinOrderBookUpdate>>(x => x.Result.First().Data.Symbol);
        }

        protected override MessageTypeDefinition[] TypeEvaluators { get; } = [

            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("action").WithEqualConstraint("RecvTopicAction"),
                    new PropertyFieldReference("LocalNo") { Depth = 4 }
                ],
                TypeIdentifierCallback = x => x.FieldValue("LocalNo")!
            },

            //new MessageTypeDefinition {
            //    Fields = [
            //        new PropertyFieldReference("action"),
            //        new PropertyFieldReference("index"),
            //    ],
            //    TypeIdentifierCallback = x => $"{x.FieldValue("action")}{x.FieldValue("index")}"
            //},


            //new MessageTypeDefinition {
            //    Fields = [
            //        new PropertyFieldReference("action"),
            //        new PropertyFieldReference("errorMsg").WithNotEqualConstraint("Success"),
            //    ],
            //    TypeIdentifierCallback = x => $"{x.FieldValue("action")}{x.FieldValue("errorMsg")}"
            //},

            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("action"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("action")!
            }
        ];

        public override string? GetTypeIdentifier(ReadOnlySpan<byte> data, WebSocketMessageType? webSocketMessageType)
        {
            if (data.Length == 4)
                return "pong";

            return base.GetTypeIdentifier(data, webSocketMessageType);
        }
    }
}

using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using DeepCoin.Net;
using System;
using System.Net.WebSockets;
using System.Text.Json;

namespace CryptoCom.Net.Clients.MessageHandlers
{
    internal class DeepCoinSocketMessageHandler : JsonSocketMessageHandler
    {
        public override JsonSerializerOptions Options { get; } = SerializerOptions.WithConverters(DeepCoinExchange._serializerContext);

        protected override MessageEvaluator[] MessageEvaluators { get; } = [

            new MessageEvaluator {
                Priority = 1,
                Fields = [
                    new PropertyFieldReference("action") { Constraint = x => x!.Equals("RecvTopicAction", StringComparison.Ordinal) },
                    new PropertyFieldReference("LocalNo") { Depth = 4 }
                ],
                IdentifyMessageCallback = x => x.FieldValue("LocalNo")!
            },

            new MessageEvaluator {
                Priority = 2,
                Fields = [
                    new PropertyFieldReference("action"),
                    new PropertyFieldReference("index"),
                ],
                IdentifyMessageCallback = x => $"{x.FieldValue("action")}{x.FieldValue("index")}"
            },


            new MessageEvaluator {
                Priority = 3,
                Fields = [
                    new PropertyFieldReference("action"),
                    new PropertyFieldReference("errorMsg"),
                ],
                IdentifyMessageCallback = x => $"{x.FieldValue("action")}{x.FieldValue("errorMsg")}"
            },

            new MessageEvaluator {
                Priority = 4,
                Fields = [
                    new PropertyFieldReference("action"),
                ],
                IdentifyMessageCallback = x => x.FieldValue("action")!
            }
        ];

        public override string? GetMessageIdentifier(ReadOnlySpan<byte> data, WebSocketMessageType? webSocketMessageType)
        {
            if (data.Length == 4)
                return "pong";

            return base.GetMessageIdentifier(data, webSocketMessageType);
        }
    }
}

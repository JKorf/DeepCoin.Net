using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using DeepCoin.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using System;
using System.Net.WebSockets;
using System.Text.Json;

namespace DeepCoin.Net.Clients.MessageHandlers;

/// <summary>
/// Routes V2 public envelopes and the unchanged private-stream envelopes.
/// </summary>
internal sealed class DeepCoinV2SocketMessageHandler : JsonSocketMessageHandler
{
    #region Fields

    private readonly MessageTypeDefinition[] _typeEvaluators;

    #endregion

    #region Properties

    /// <inheritdoc />
    protected override MessageTypeDefinition[] TypeEvaluators => _typeEvaluators;

    /// <inheritdoc />
    public override JsonSerializerOptions Options { get; } = SerializerOptions.WithConverters(DeepCoinExchange._serializerContext);

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes the V2 message routes and reusable field evaluators.
    /// </summary>
    public DeepCoinV2SocketMessageHandler()
    {
        // The parser retains field-reference identities while collecting values. Reuse these
        // exact evaluator instances when it checks the completed message, rather than reallocating.
        _typeEvaluators = new[]
        {
            new MessageTypeDefinition
            {
                Fields = [new PropertyFieldReference("a").WithEqualConstraint("RecvTopicAction"), new PropertyFieldReference("L") { Depth = 2 }],
                TypeIdentifierCallback = fields => fields.FieldValue("L")!
            },
            new MessageTypeDefinition
            {
                Fields = [new PropertyFieldReference("a")],
                TypeIdentifierCallback = fields => fields.FieldValue("a")!
            },
            // V2 listen keys still use the private stream's original action envelope.
            new MessageTypeDefinition
            {
                Fields = [new PropertyFieldReference("action")],
                TypeIdentifierCallback = fields => fields.FieldValue("action")!
            }
        };

        // PO frames identify the instrument inside d, without a top-level symbol.
        AddTopicMapping<DeepCoinV2SymbolMessage>(message => message.Data.Length == 0 ? null : message.Data[0].Symbol);
        
        AddTopicMapping<DeepCoinV2SocketMessage>(message => message.Action switch
        {
            "PK" => message.Symbol + "_" + EnumConverter.GetString(message.Period),
            _ => message.Symbol
        });
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override string? GetTypeIdentifierNonJson(ReadOnlySpan<byte> data, WebSocketMessageType? webSocketMessageType)
        => data.Length == 4 ? "pong" : null;

    #endregion
}

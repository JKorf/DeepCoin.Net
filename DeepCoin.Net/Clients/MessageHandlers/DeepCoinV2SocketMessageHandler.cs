using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using DeepCoin.Net.Objects.Internal;
using System.Linq;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace DeepCoin.Net.Clients.MessageHandlers;

/// <summary>
/// Routes V2 public envelopes and the unchanged private-stream envelopes.
/// </summary>
internal sealed class DeepCoinV2SocketMessageHandler : DeepCoinSocketMessageHandler
{
    #region Fields

    private readonly MessageTypeDefinition[] _typeEvaluators;

    #endregion

    #region Properties

    /// <inheritdoc />
    protected override MessageTypeDefinition[] TypeEvaluators => _typeEvaluators;

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
            }
        }.Concat(base.TypeEvaluators).ToArray();
        AddTopicMapping<DeepCoinV2SocketMessage>(message => message.Action switch
        {
            // Live ticker batches identify each instrument inside d, so no single topic applies.
            "PO" => null,
            "PK" => message.Symbol + "_" + EnumConverter.GetString(message.Period),
            _ => message.Symbol
        });
    }

    #endregion
}

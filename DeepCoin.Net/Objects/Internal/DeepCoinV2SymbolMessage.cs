using System.Text.Json.Serialization;
using DeepCoin.Net.Converters;
using DeepCoin.Net.Objects.Models;

namespace DeepCoin.Net.Objects.Internal;

/// <summary>
/// V2 symbol stream message.
/// </summary>
internal sealed class DeepCoinV2SymbolMessage : DeepCoinV2SocketMessage
{
    /// <summary>
    /// [<c>d</c>] Symbol updates.
    /// </summary>
    [JsonPropertyName("d")]
    [JsonConverter(typeof(DeepCoinV2DataConverter<DeepCoinV2SymbolData>))]
    public DeepCoinV2SymbolData[] Data { get; set; } = [];
}

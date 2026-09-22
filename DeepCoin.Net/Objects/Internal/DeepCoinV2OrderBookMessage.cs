using System.Text.Json.Serialization;
using DeepCoin.Net.Objects.Models;

namespace DeepCoin.Net.Objects.Internal;

/// <summary>
/// V2 orderbook stream message.
/// </summary>
internal sealed class DeepCoinV2OrderBookMessage : DeepCoinV2SocketMessage
{
    /// <summary>
    /// [<c>d</c>] Order book update. Live snapshots and increments contain one object, unlike the array in the docs example.
    /// </summary>
    [JsonPropertyName("d")]
    public DeepCoinV2OrderBookData Data { get; set; } = null!;
}

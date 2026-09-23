using System.Text.Json.Serialization;
using DeepCoin.Net.Objects.Models;

namespace DeepCoin.Net.Objects.Internal;

/// <summary>
/// V2 ticker stream message.
/// </summary>
internal sealed class DeepCoinV2TickerMessage : DeepCoinV2SocketMessage
{
    /// <summary>
    /// [<c>d</c>] Ticker updates. The live API returns an array, unlike the object in the docs example.
    /// </summary>
    [JsonPropertyName("d")]
    public DeepCoinV2TickerData[] Data { get; set; } = [];
}

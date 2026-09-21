using System.Text.Json.Serialization;
using DeepCoin.Net.Objects.Models;

namespace DeepCoin.Net.Objects.Internal;

/// <summary>
/// V2 kline stream message.
/// </summary>
internal sealed class DeepCoinV2KlineMessage : DeepCoinV2SocketMessage
{
    /// <summary>
    /// [<c>d</c>] Kline updates.
    /// </summary>
    [JsonPropertyName("d")]
    public DeepCoinKline[] Data { get; set; } = [];
}

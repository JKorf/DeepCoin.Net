using System.Text.Json.Serialization;
using DeepCoin.Net.Objects.Models;

namespace DeepCoin.Net.Objects.Internal;

/// <summary>
/// V2 trade stream message.
/// </summary>
internal sealed class DeepCoinV2TradeMessage : DeepCoinV2SocketMessage
{
    /// <summary>
    /// [<c>d</c>] Trade updates.
    /// </summary>
    [JsonPropertyName("d")]
    public DeepCoinV2TradeData[] Data { get; set; } = [];
}

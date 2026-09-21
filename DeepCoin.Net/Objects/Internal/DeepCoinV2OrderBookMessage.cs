using System.Text.Json.Serialization;
using DeepCoin.Net.Converters;
using DeepCoin.Net.Objects.Models;

namespace DeepCoin.Net.Objects.Internal;

/// <summary>
/// V2 orderbook stream message.
/// </summary>
internal sealed class DeepCoinV2OrderBookMessage : DeepCoinV2SocketMessage
{
    /// <summary>
    /// [<c>d</c>] OrderBook updates.
    /// </summary>
    [JsonPropertyName("d")]
    [JsonConverter(typeof(DeepCoinV2DataConverter<DeepCoinV2OrderBookData>))]
    public DeepCoinV2OrderBookData[] Data { get; set; } = [];
}

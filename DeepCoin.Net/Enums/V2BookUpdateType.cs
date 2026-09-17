using System.Text.Json.Serialization;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace DeepCoin.Net.Enums;

/// <summary>
/// V2 order book update kind.
/// </summary>
[JsonConverter(typeof(EnumConverter<V2BookUpdateType>))]
internal enum V2BookUpdateType
{
    /// <summary>
    /// [<c>f</c>] Full order book snapshot.
    /// </summary>
    [Map("f")]
    Snapshot,
    /// <summary>
    /// [<c>i</c>] Incremental order book update.
    /// </summary>
    [Map("i")]
    Incremental
}

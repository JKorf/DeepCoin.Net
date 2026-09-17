using System.Text.Json.Serialization;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace DeepCoin.Net.Enums;

/// <summary>
/// V2 public subscription operation.
/// </summary>
[JsonConverter(typeof(EnumConverter<V2SubscriptionAction>))]
internal enum V2SubscriptionAction
{
    /// <summary>
    /// [<c>1</c>] Subscribe to a topic.
    /// </summary>
    [Map("1")]
    Subscribe = 1,
    /// <summary>
    /// [<c>0</c>] Unsubscribe using the original subscription's LocalNo, as verified on live V2 streams.
    /// </summary>
    [Map("0")]
    Unsubscribe = 0
}

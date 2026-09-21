using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace DeepCoin.Net.Converters;

/// <summary>
/// Handles V2 ticker and order book data returned as either a JSON array or a plain JSON object.
/// Arrays are read as-is; a single object is wrapped in an array so both shapes map to <c>T[]</c>.
/// </summary>
internal sealed class DeepCoinV2DataConverter<T> : JsonConverter<T[]>
{
    /// <inheritdoc />
    public override T[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Use generated type metadata for both shapes to support Native AOT without reflection.
        if (reader.TokenType == JsonTokenType.StartObject)
            return [JsonSerializer.Deserialize(ref reader, (JsonTypeInfo<T>)options.GetTypeInfo(typeof(T)))!];

        return JsonSerializer.Deserialize(ref reader, (JsonTypeInfo<T[]>)options.GetTypeInfo(typeof(T[])));
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, T[] value, JsonSerializerOptions options)
        => JsonSerializer.Serialize(writer, value, (JsonTypeInfo<T[]>)options.GetTypeInfo(typeof(T[])));
}

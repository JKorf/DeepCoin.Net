using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Internal
{
    /// <summary>
    /// DeepCoin response
    /// </summary>
    internal record DeepCoinResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("msg")]
        public string? Message { get; set; }
    }

    internal record DeepCoinResponse<T> : DeepCoinResponse
    {
        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }
}

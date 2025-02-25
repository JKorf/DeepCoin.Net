using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Internal
{
    internal record TableData<T>
    {
        [JsonPropertyName("table")]
        public string Table { get; set; }
        [JsonPropertyName("data")]
        public T Data { get; set; }
    }
}

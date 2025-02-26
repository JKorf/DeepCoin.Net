using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Internal
{
    internal class SocketResponse
    {
        [JsonPropertyName("errorCode")]
        public int ErrorCode { get; set; }
        [JsonPropertyName("errorMsg")]
        public string ErrorMessage { get; set; }
    }
}

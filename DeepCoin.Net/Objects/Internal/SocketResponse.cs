using System.Text.Json.Serialization;

namespace DeepCoin.Net.Objects.Internal
{
    internal class SocketResponse
    {
        [JsonPropertyName("errorCode")]
        public int ErrorCode { get; set; }
        [JsonPropertyName("errorMsg")]
        public string ErrorMessage { get; set; } = string.Empty;
    }
}

using System.Globalization;
using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;

namespace DeepCoin.Net.Clients.MessageHandlers
{
    /// <summary>
    /// Parses V2 HTTP failures using the documented string code and msg envelope.
    /// </summary>
    internal class DeepCoinRestV2MessageHandler : DeepCoinRestMessageHandler
    {
        private readonly ErrorMapping _errorMapping;

        /// <summary>
        /// Initializes V2 error parsing.
        /// </summary>
        internal DeepCoinRestV2MessageHandler(ErrorMapping errorMapping) : base(errorMapping) => _errorMapping = errorMapping;

        /// <inheritdoc />
        public override async ValueTask<Error> ParseErrorResponse(int httpStatusCode, HttpResponseHeaders responseHeaders, Stream responseStream)
        {
            var (jsonError, jsonDocument) = await GetJsonDocument(responseStream).ConfigureAwait(false);
            if (jsonError != null)
                return httpStatusCode is 401 or 403 ? new ServerError(new ErrorInfo(ErrorType.Unauthorized, "Unauthorized")) : jsonError;

            using var document = jsonDocument!;
            var root = document.RootElement;
            int? code = null;
            if (root.TryGetProperty("code", out var codeValue))
            {
                if (codeValue.ValueKind == JsonValueKind.Number && codeValue.TryGetInt32(out var number))
                    code = number;
                else if (codeValue.ValueKind == JsonValueKind.String && int.TryParse(codeValue.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out number))
                    code = number;
            }
            var message = root.TryGetProperty("msg", out var msg) ? msg.GetString()
                : root.TryGetProperty("message", out msg) ? msg.GetString() : null;
            if (code.HasValue)
                return new ServerError(code.Value, _errorMapping.GetErrorInfo(code.Value.ToString(CultureInfo.InvariantCulture), message ?? "Unknown V2 error"));
            if (httpStatusCode is 401 or 403)
                return new ServerError(new ErrorInfo(ErrorType.Unauthorized, message ?? "Unauthorized"));
            return new ServerError(ErrorInfo.Unknown with { Message = message ?? "Unknown V2 error" });
        }
    }
}

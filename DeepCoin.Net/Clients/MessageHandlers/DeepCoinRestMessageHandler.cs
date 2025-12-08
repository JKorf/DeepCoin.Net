using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using DeepCoin.Net.Objects.Internal;
using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeepCoin.Net.Clients.MessageHandlers
{
    internal class DeepCoinRestMessageHandler : JsonRestMessageHandler
    {
        private readonly ErrorMapping _errorMapping;

        public override JsonSerializerOptions Options { get; } = SerializerOptions.WithConverters(DeepCoinExchange._serializerContext);

        public DeepCoinRestMessageHandler(ErrorMapping errorMapping)
        {
            _errorMapping = errorMapping;
        }

        public override Error? CheckDeserializedResponse<T>(HttpResponseHeaders responseHeaders, T result)
        {
            if (result is not DeepCoinResponse deepCoinResponse)
                return null;

            if (deepCoinResponse.Code == 0)
                return null;

            return new ServerError(deepCoinResponse.Code, _errorMapping.GetErrorInfo(deepCoinResponse.Code.ToString(), deepCoinResponse.Message!));
        }

        public override async ValueTask<Error> ParseErrorResponse(
            int httpStatusCode,
            HttpResponseHeaders responseHeaders,
            Stream responseStream)
        {
            if (httpStatusCode == 401 || httpStatusCode == 403)
                return new ServerError(new ErrorInfo(ErrorType.Unauthorized, "Unauthorized"));

            var (jsonError, jsonDocument) = await GetJsonDocument(responseStream).ConfigureAwait(false);
            if (jsonError != null)
                return jsonError;

            int? code = jsonDocument!.RootElement.TryGetProperty("code", out var codeProp) ? codeProp.GetInt32() : null;
            var msg = jsonDocument.RootElement.TryGetProperty("message", out var msgProp) ? msgProp.GetString() : null;
            if (msg == null)
                return new ServerError(ErrorInfo.Unknown);

            if (code == null)
                return new ServerError(ErrorInfo.Unknown with { Message = msg });

            return new ServerError(code.Value, _errorMapping.GetErrorInfo(code.Value.ToString(), msg));
        }
    }
}

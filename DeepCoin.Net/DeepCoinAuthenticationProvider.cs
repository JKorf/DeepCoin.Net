using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using DeepCoin.Net.Objects;

namespace DeepCoin.Net
{
    internal class DeepCoinAuthenticationProvider : AuthenticationProvider<DeepCoinApiCredentials>
    {
        private static readonly IMessageSerializer _serializer = new SystemTextJsonMessageSerializer();

        public DeepCoinAuthenticationProvider(DeepCoinApiCredentials credentials) : base(credentials)
        {
        }

        public override void AuthenticateRequest(
            RestApiClient apiClient,
            Uri uri,
            HttpMethod method,
            ref IDictionary<string, object>? uriParameters,
            ref IDictionary<string, object>? bodyParameters,
            ref Dictionary<string, string>? headers,
            bool auth,
            ArrayParametersSerialization arraySerialization,
            HttpMethodParameterPosition parameterPosition,
            RequestBodyFormat requestBodyFormat)
        {
            headers = new Dictionary<string, string>() { };

            if (!auth)
                return;

            var timestamp = GetTimestamp(apiClient);
            var timestampString = timestamp.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

            var signStr = timestampString + method + uri.PathAndQuery 
                + (uriParameters?.Any() == true ? ("?" + uriParameters.CreateParamString(false, arraySerialization)): "")
                + (bodyParameters?.Any() == true ? GetSerializedBody(_serializer, bodyParameters): "");
            var sign = SignHMACSHA256(signStr, SignOutputType.Base64);
            headers.Add("DC-ACCESS-KEY", ApiKey);
            headers.Add("DC-ACCESS-SIGN", sign);
            headers.Add("DC-ACCESS-TIMESTAMP", timestampString);
            headers.Add("DC-ACCESS-PASSPHRASE", _credentials.PassPhrase);

        }
    }
}

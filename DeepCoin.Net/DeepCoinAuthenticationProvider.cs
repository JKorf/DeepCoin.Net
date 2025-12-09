using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;

namespace DeepCoin.Net
{
    internal class DeepCoinAuthenticationProvider : AuthenticationProvider
    {
        private static readonly IMessageSerializer _serializer = new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(DeepCoinExchange._serializerContext));

        public override ApiCredentialsType[] SupportedCredentialTypes => [ApiCredentialsType.Hmac];
        public DeepCoinAuthenticationProvider(ApiCredentials credentials) : base(credentials)
        {
            if (string.IsNullOrEmpty(credentials.Pass))
                throw new ArgumentNullException(nameof(ApiCredentials.Pass), "Passphrase is required for DeepCoin authentication");
        }

        public override void ProcessRequest(RestApiClient apiClient, RestRequestConfiguration request)
        {
            if (!request.Authenticated)
                return;

            var timestamp = GetTimestamp(apiClient).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var queryParams = request.QueryParameters?.Count > 0 ? $"?{request.GetQueryString(false)}" : "";
            var bodyParams = request.BodyParameters?.Count > 0 ? GetSerializedBody(_serializer, request.BodyParameters) : "";
            var signStr = $"{timestamp}{request.Method}{request.Path}{queryParams}{bodyParams}";
            var signature = SignHMACSHA256(signStr, SignOutputType.Base64);

            request.Headers ??= new Dictionary<string, string>();
            request.Headers.Add("DC-ACCESS-KEY", ApiKey);
            request.Headers.Add("DC-ACCESS-SIGN", signature);
            request.Headers.Add("DC-ACCESS-TIMESTAMP", timestamp);
            request.Headers.Add("DC-ACCESS-PASSPHRASE", _credentials.Pass!);

            request.SetQueryString(queryParams);
            request.SetBodyContent(bodyParams);
        }
    }
}

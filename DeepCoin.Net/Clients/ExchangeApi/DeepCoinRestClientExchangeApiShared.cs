using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Text;
using DeepCoin.Net.Interfaces.Clients.ExchangeApi;

namespace DeepCoin.Net.Clients.ExchangeApi
{
    internal partial class DeepCoinRestClientExchangeApi : IDeepCoinRestClientExchangeApiShared
    {
        public string Exchange => "DeepCoin";

        public TradingMode[] SupportedTradingModes => new[] { TradingMode.Spot };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();
    }
}

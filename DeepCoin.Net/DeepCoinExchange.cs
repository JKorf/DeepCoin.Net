using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Filters;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net.RateLimiting.Interfaces;
using CryptoExchange.Net.RateLimiting;
using System;
using System.Collections.Generic;
using System.Text;
using CryptoExchange.Net.SharedApis;

namespace DeepCoin.Net
{
    /// <summary>
    /// DeepCoin exchange information and configuration
    /// </summary>
    public static class DeepCoinExchange
    {
        /// <summary>
        /// Exchange name
        /// </summary>
        public static string ExchangeName => "DeepCoin";

        /// <summary>
        /// Url to the main website
        /// </summary>
        public static string Url { get; } = "https://www.deepcoin.com/";

        /// <summary>
        /// Urls to the API documentation
        /// </summary>
        public static string[] ApiDocsUrl { get; } = new[] {
            "https://www.deepcoin.com/docs/authentication"
            };

        /// <summary>
        /// Format a base and quote asset to an DeepCoin recognized symbol 
        /// </summary>
        /// <param name="baseAsset">Base asset</param>
        /// <param name="quoteAsset">Quote asset</param>
        /// <param name="tradingMode">Trading mode</param>
        /// <param name="deliverTime">Delivery time for delivery futures</param>
        /// <returns></returns>
        public static string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
        {
            if (tradingMode == TradingMode.Spot)
                return baseAsset + "-" + quoteAsset;

            return baseAsset + "-" + quoteAsset + "-SWAP";
        }

        /// <summary>
        /// Rate limiter configuration for the DeepCoin API
        /// </summary>
        public static DeepCoinRateLimiters RateLimiter { get; } = new DeepCoinRateLimiters();
    }

    /// <summary>
    /// Rate limiter configuration for the DeepCoin API
    /// </summary>
    public class DeepCoinRateLimiters
    {
        /// <summary>
        /// Event for when a rate limit is triggered
        /// </summary>
        public event Action<RateLimitEvent> RateLimitTriggered;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal DeepCoinRateLimiters()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Initialize();
        }

        private void Initialize()
        {
            DeepCoin = new RateLimitGate("DeepCoin");
            DeepCoin.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
        }


        internal IRateLimitGate DeepCoin { get; private set; }

    }
}

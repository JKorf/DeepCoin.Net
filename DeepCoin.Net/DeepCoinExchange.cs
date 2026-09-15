using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Interfaces;
using CryptoExchange.Net.RateLimiting;
using System;
using CryptoExchange.Net.SharedApis;
using System.Text.Json.Serialization;
using DeepCoin.Net.Converters;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.RateLimiting.Guards;

namespace DeepCoin.Net
{
    /// <summary>
    /// DeepCoin exchange information and configuration
    /// </summary>
    public static class DeepCoinExchange
    {
        /// <summary>
        /// Platform metadata
        /// </summary>
        public static PlatformInfo Metadata { get; } = new PlatformInfo(
                "DeepCoin",
                "DeepCoin",
                "https://raw.githubusercontent.com/JKorf/DeepCoin.Net/master/DeepCoin.Net/Icon/icon.png",
                "https://www.deepcoin.com/",
                ["https://www.deepcoin.com/docs/authentication"],
                PlatformType.CryptoCurrencyExchange,
                CentralizationType.Centralized,
                DeepCoinEnvironment.All
                );

        /// <summary>
        /// Exchange name
        /// </summary>
        public static string ExchangeName => "DeepCoin";

        /// <summary>
        /// Display name
        /// </summary>
        public static string DisplayName => "DeepCoin";

        /// <summary>
        /// Url to exchange image
        /// </summary>
        public static string ImageUrl { get; } = "https://raw.githubusercontent.com/JKorf/DeepCoin.Net/master/DeepCoin.Net/Icon/icon.png";

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
        /// Type of exchange
        /// </summary>
        public static ExchangeType Type { get; } = ExchangeType.CEX;

        internal static JsonSerializerContext _serializerContext = JsonSerializerContextCache.GetOrCreate<DeepCoinSourceGenerationContext>();
        internal static ParameterSerializationSettings _parameterSerializationSettings = new ParameterSerializationSettings
        {
            Decimal = DecimalSerialization.String,
            DateTimes = DateTimeSerialization.MillisecondsNumber
        };

        /// <summary>
        /// Aliases for DeepCoin assets
        /// </summary>
        public static AssetAliasConfiguration AssetAliases { get; } = new AssetAliasConfiguration
        {
            Aliases = [
                new AssetAlias("USDT", SharedSymbol.UsdOrStable.ToUpperInvariant(), AliasType.OnlyToExchange)
            ]
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
            baseAsset = AssetAliases.CommonToExchangeName(baseAsset.ToUpperInvariant());
            quoteAsset = AssetAliases.CommonToExchangeName(quoteAsset.ToUpperInvariant());

            if (tradingMode == TradingMode.Spot)
                return baseAsset + "-" + quoteAsset;

            return baseAsset + "-" + quoteAsset + "-SWAP";
        }

        /// <summary>
        /// Format a base and quote asset to an DeepCoin recognized symbol 
        /// </summary>
        /// <param name="baseAsset">Base asset</param>
        /// <param name="quoteAsset">Quote asset</param>
        /// <param name="tradingMode">Trading mode</param>
        /// <param name="deliverTime">Delivery time for delivery futures</param>
        /// <returns></returns>
        public static string FormatWebsocketSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
        {
            if (quoteAsset.Equals(SharedSymbol.UsdOrStable))
                quoteAsset = AssetAliases.CommonToExchangeName(SharedSymbol.UsdOrStable.ToUpperInvariant());

            if (tradingMode == TradingMode.Spot)
                return baseAsset + "/" + quoteAsset;

            return baseAsset + quoteAsset + "-SWAP";
        }

        /// <summary>
        /// Rate limiter configuration for the DeepCoin API
        /// </summary>
        public static DeepCoinRateLimiters RateLimiter { get; set; } = new DeepCoinRateLimiters();
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

        /// <summary>
        /// Event when the rate limit is updated. Note that it's only updated when a request is send, so there are no specific updates when the current usage is decaying.
        /// </summary>
        public event Action<RateLimitUpdateEvent> RateLimitUpdated;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        /// <summary>
        /// ctor
        /// </summary>
        public DeepCoinRateLimiters()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Initialize();
        }

        /// <summary>
        /// Initialize the rate limits
        /// </summary>
        protected virtual void Initialize()
        {
            DeepCoin = new RateLimitGate("DeepCoin");
            // Minute budgets apply separately per path. SingleLimitGuard on each request handles its second budget.
            // Count HTTP methods together, as documented at https://www.deepcoin.com/docs/v2/rateLimit.
            // Counting all keys together is conservative for UID-based limits.
            RestMarket = new RateLimitGate("DeepCoin Market")
                .AddGuard(new RateLimitGuard((definition, _) => definition.BaseAddress + definition.Path, [], 600, TimeSpan.FromMinutes(1), RateLimitWindowType.Sliding));
            RestAccount = new RateLimitGate("DeepCoin Account")
                .AddGuard(new RateLimitGuard((definition, _) => definition.BaseAddress + definition.Path, [], 300, TimeSpan.FromMinutes(1), RateLimitWindowType.Sliding));
            RestHistory = new RateLimitGate("DeepCoin History")
                .AddGuard(new RateLimitGuard((definition, _) => definition.BaseAddress + definition.Path, [], 150, TimeSpan.FromMinutes(1), RateLimitWindowType.Sliding));
            // Order lookup and placement share a path, so both windows must also be shared across GET/POST.
            RestOrder = new RateLimitGate("DeepCoin Order")
                .AddGuard(new RateLimitGuard((definition, _) => definition.BaseAddress + definition.Path, [], 15, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding))
                .AddGuard(new RateLimitGuard((definition, _) => definition.BaseAddress + definition.Path, [], 450, TimeSpan.FromMinutes(1), RateLimitWindowType.Sliding));
            DeepCoin.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            DeepCoin.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
            RestMarket.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            RestMarket.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
            RestAccount.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            RestAccount.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
            RestHistory.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            RestHistory.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
            RestOrder.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            RestOrder.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
        }

        internal IRateLimitGate DeepCoin { get; private set; }

        /// <summary>
        /// Per-path market request minute limits.
        /// </summary>
        internal IRateLimitGate RestMarket { get; private set; }

        /// <summary>
        /// Per-path account request minute limits.
        /// </summary>
        internal IRateLimitGate RestAccount { get; private set; }

        /// <summary>
        /// Per-path history, asset and listen-key request minute limits.
        /// </summary>
        internal IRateLimitGate RestHistory { get; private set; }

        /// <summary>
        /// Per-path order request second and minute limits shared across HTTP methods.
        /// </summary>
        internal IRateLimitGate RestOrder { get; private set; }
    }
}

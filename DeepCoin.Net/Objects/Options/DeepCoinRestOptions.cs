using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects.Options;

namespace DeepCoin.Net.Objects.Options
{
    /// <summary>
    /// Options for the DeepCoinRestClient
    /// </summary>
    public class DeepCoinRestOptions : RestExchangeOptions<DeepCoinEnvironment, DeepCoinCredentials>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        internal static DeepCoinRestOptions Default { get; set; } = new DeepCoinRestOptions()
        {
            Environment = DeepCoinEnvironment.Live
        };

        /// <summary>
        /// ctor
        /// </summary>
        public DeepCoinRestOptions()
        {
            Default?.Set(this);
        }
                
        /// <summary>
        /// Exchange API options
        /// </summary>
        public RestApiOptions ExchangeOptions { get; private set; } = new RestApiOptions();

        /// <summary>
        /// V2 API options, independent of the legacy Exchange API.
        /// </summary>
        public RestApiOptions V2Options { get; private set; } = new RestApiOptions();

        /// <summary>
        /// Copies the exchange and version-specific REST options to the target.
        /// </summary>
        internal DeepCoinRestOptions Set(DeepCoinRestOptions targetOptions)
        {
            targetOptions = base.Set<DeepCoinRestOptions>(targetOptions);            
            targetOptions.ExchangeOptions = ExchangeOptions.Set(targetOptions.ExchangeOptions);
            targetOptions.V2Options = V2Options.Set(targetOptions.V2Options);
            return targetOptions;
        }
    }
}

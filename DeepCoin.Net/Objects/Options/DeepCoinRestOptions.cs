using CryptoExchange.Net.Objects.Options;

namespace DeepCoin.Net.Objects.Options
{
    /// <summary>
    /// Options for the DeepCoinRestClient
    /// </summary>
    public class DeepCoinRestOptions : RestExchangeOptions<DeepCoinEnvironment>
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

        internal DeepCoinRestOptions Set(DeepCoinRestOptions targetOptions)
        {
            targetOptions = base.Set<DeepCoinRestOptions>(targetOptions);            
            targetOptions.ExchangeOptions = ExchangeOptions.Set(targetOptions.ExchangeOptions);
            return targetOptions;
        }
    }
}

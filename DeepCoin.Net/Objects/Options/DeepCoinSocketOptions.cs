using CryptoExchange.Net.Objects.Options;

namespace DeepCoin.Net.Objects.Options
{
    /// <summary>
    /// Options for the DeepCoinSocketClient
    /// </summary>
    public class DeepCoinSocketOptions : SocketExchangeOptions<DeepCoinEnvironment, DeepCoinApiCredentials>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        internal static DeepCoinSocketOptions Default { get; set; } = new DeepCoinSocketOptions()
        {
            Environment = DeepCoinEnvironment.Live,
            SocketSubscriptionsCombineTarget = 10
        };


        /// <summary>
        /// ctor
        /// </summary>
        public DeepCoinSocketOptions()
        {
            Default?.Set(this);
        }


        
         /// <summary>
        /// Exchange API options
        /// </summary>
        public SocketApiOptions ExchangeOptions { get; private set; } = new SocketApiOptions();


        internal DeepCoinSocketOptions Set(DeepCoinSocketOptions targetOptions)
        {
            targetOptions = base.Set<DeepCoinSocketOptions>(targetOptions);
            
            targetOptions.ExchangeOptions = ExchangeOptions.Set(targetOptions.ExchangeOptions);

            return targetOptions;
        }
    }
}

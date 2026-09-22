using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects.Options;

namespace DeepCoin.Net.Objects.Options
{
    /// <summary>
    /// Options for the DeepCoinSocketClient
    /// </summary>
    public class DeepCoinSocketOptions : SocketExchangeOptions<DeepCoinEnvironment, DeepCoinCredentials>
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

        /// <summary>
        /// Options for the native V2 socket API. Defaults to at most ten connections; the venue's per-IP budget is shared with other client instances.
        /// </summary>
        public SocketApiOptions V2Options { get; private set; } = new SocketApiOptions { MaxSocketConnections = 10 };

        /// <summary>
        /// Copies the exchange and version-specific socket options to the target.
        /// </summary>
        internal DeepCoinSocketOptions Set(DeepCoinSocketOptions targetOptions)
        {
            targetOptions = base.Set<DeepCoinSocketOptions>(targetOptions);            
            targetOptions.ExchangeOptions = ExchangeOptions.Set(targetOptions.ExchangeOptions);
            targetOptions.V2Options = V2Options.Set(targetOptions.V2Options);
            return targetOptions;
        }
    }
}

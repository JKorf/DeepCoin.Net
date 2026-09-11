using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects.Options;
using CryptoExchange.Net.SharedApis;

namespace DeepCoin.Net.Objects.Options
{
    /// <summary>
    /// DeepCoin options
    /// </summary>
    public class DeepCoinOptions: LibraryOptions<DeepCoinRestOptions, DeepCoinSocketOptions, DeepCoinCredentials, DeepCoinEnvironment>
    {
        /// <summary>
        /// Options for Shared API usage
        /// </summary>
        public SharedApiOptions SharedApi { get; set; } = new();
    }
}

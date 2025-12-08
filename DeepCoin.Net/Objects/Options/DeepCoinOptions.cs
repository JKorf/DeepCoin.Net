using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects.Options;

namespace DeepCoin.Net.Objects.Options
{
    /// <summary>
    /// DeepCoin options
    /// </summary>
    public class DeepCoinOptions: LibraryOptions<DeepCoinRestOptions, DeepCoinSocketOptions, ApiCredentials, DeepCoinEnvironment>
    {
    }
}

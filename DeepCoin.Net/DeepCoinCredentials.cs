using CryptoExchange.Net.Authentication;

namespace DeepCoin.Net
{
    /// <summary>
    /// DeepCoin API credentials
    /// </summary>
    public class DeepCoinCredentials : HMACCredential
    {
        /// <summary>
        /// Create new credentials providing only credentials in HMAC format
        /// </summary>
        /// <param name="key">API key</param>
        /// <param name="secret">API secret</param>
        /// <param name="pass">Passphrase</param>
        public DeepCoinCredentials(string key, string secret, string pass) : base(key, secret, pass)
        {
        }
    }
}

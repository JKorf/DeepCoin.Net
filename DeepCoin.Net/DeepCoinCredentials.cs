using CryptoExchange.Net.Authentication;

namespace DeepCoin.Net
{
    /// <summary>
    /// DeepCoin credentials
    /// </summary>
    public class DeepCoinCredentials : ApiCredentials
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="apiKey">The API key</param>
        /// <param name="secret">The API secret</param>
        /// <param name="passphrase">Passphrase</param>
        public DeepCoinCredentials(string apiKey, string secret, string passphrase) : this(new HMACCredential(apiKey, secret, passphrase)) { }
       
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="credential">The HMAC credentials</param>
        public DeepCoinCredentials(HMACCredential credential) : base(credential) { }

        /// <inheritdoc />
        public override ApiCredentials Copy() => new DeepCoinCredentials(Hmac!);
    }
}

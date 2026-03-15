using CryptoExchange.Net.Authentication;
using System;

namespace DeepCoin.Net
{
    /// <summary>
    /// DeepCoin credentials
    /// </summary>
    public class DeepCoinCredentials : ApiCredentials
    {
        /// <summary>
        /// </summary>
        [Obsolete("Parameterless constructor is only for deserialization purposes and should not be used directly. Use parameterized constructor instead.")]
        public DeepCoinCredentials() { }

        /// <summary>
        /// Create credentials using an HMAC key, secret and passphrase
        /// </summary>
        /// <param name="apiKey">The API key</param>
        /// <param name="secret">The API secret</param>
        /// <param name="passphrase">Passphrase</param>
        public DeepCoinCredentials(string apiKey, string secret, string passphrase) : this(new HMACCredential(apiKey, secret, passphrase)) { }

        /// <summary>
        /// Create DeepCoin credentials using HMAC credentials
        /// </summary>
        /// <param name="credential">The HMAC credentials</param>
        public DeepCoinCredentials(HMACCredential credential) : base(credential) { }

        /// <inheritdoc />
#pragma warning disable CS0618 // Type or member is obsolete
        public override ApiCredentials Copy() => new DeepCoinCredentials { CredentialPairs = CredentialPairs };
#pragma warning restore CS0618 // Type or member is obsolete
    }
}

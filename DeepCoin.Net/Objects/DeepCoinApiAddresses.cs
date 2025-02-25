namespace DeepCoin.Net.Objects
{
    /// <summary>
    /// Api addresses
    /// </summary>
    public class DeepCoinApiAddresses
    {
        /// <summary>
        /// The address used by the DeepCoinRestClient for the API
        /// </summary>
        public string RestClientAddress { get; set; } = "";
        /// <summary>
        /// The address used by the DeepCoinSocketClient for the websocket API
        /// </summary>
        public string SocketClientAddress { get; set; } = "";

        /// <summary>
        /// The default addresses to connect to the DeepCoin API
        /// </summary>
        public static DeepCoinApiAddresses Default = new DeepCoinApiAddresses
        {
            RestClientAddress = "https://api.deepcoin.com",
            SocketClientAddress = "wss://stream.deepcoin.com"
        };
    }
}

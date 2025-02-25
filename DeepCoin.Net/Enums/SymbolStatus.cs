using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Symbol status
    /// </summary>
    public enum SymbolStatus
    {
        /// <summary>
        /// Live
        /// </summary>
        [Map("live", "2")]
        Live,
        /// <summary>
        /// Suspended
        /// </summary>
        [Map("suspend", "1")]
        Suspended,
        /// <summary>
        /// Pre-open
        /// </summary>
        [Map("preopen", "0")]
        PreOpen,
        /// <summary>
        /// Funding fee settling
        /// </summary>
        [Map("settlement", "3", "4", "5")]
        Settling,
        /// <summary>
        /// Closing
        /// </summary>
        [Map("6")]
        Closing,
        /// <summary>
        /// Inactive
        /// </summary>
        [Map("7")]
        Inactive,
    }
}

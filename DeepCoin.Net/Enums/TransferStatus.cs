using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Transfer status
    /// </summary>
    public enum TransferStatus
    {
        /// <summary>
        /// Processing
        /// </summary>
        [Map("1", "In Process")]
        Processing,
        /// <summary>
        /// Approved
        /// </summary>
        [Map("2")]
        Approved,
        /// <summary>
        /// Rejected
        /// </summary>
        [Map("3")]
        Rejected
    }
}

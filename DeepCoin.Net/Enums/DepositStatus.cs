using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Deposit status
    /// </summary>
    public enum DepositStatus
    {
        /// <summary>
        /// Confirming
        /// </summary>
        [Map("confirming")]
        Confirming,
        /// <summary>
        /// Success
        /// </summary>
        [Map("succeed")]
        Success
    }
}

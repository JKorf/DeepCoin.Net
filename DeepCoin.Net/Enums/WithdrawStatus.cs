using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Withdrawal status
    /// </summary>
    public enum WithdrawStatus
    {
        /// <summary>
        /// Auditing
        /// </summary>
        [Map("auditing")]
        Auditing,
        /// <summary>
        /// Confirming
        /// </summary>
        [Map("confirming")]
        Confirming,
        /// <summary>
        /// Rejected
        /// </summary>
        [Map("rejected")]
        Rejected,
        /// <summary>
        /// Success
        /// </summary>
        [Map("succeed")]
        Success
    }
}

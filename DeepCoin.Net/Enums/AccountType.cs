using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Account type
    /// </summary>
    public enum AccountType
    {
        /// <summary>
        /// Email
        /// </summary>
        [Map("email")]
        Email,
        /// <summary>
        /// Phone
        /// </summary>
        [Map("phone")]
        Phone
    }
}

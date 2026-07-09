using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Balance type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<BalanceType>))]
    public enum BalanceType
    {
        /// <summary>
        /// Funding
        /// </summary>
        [Map("funding")]
        Funding,
        /// <summary>
        /// Spot
        /// </summary>
        [Map("spot")]
        Spot,
        /// <summary>
        /// USDT swap account
        /// </summary>
        [Map("swapU")]
        UsdtSwap,
        /// <summary>
        /// Coin swap account
        /// </summary>
        [Map("swap")]
        CoinSwap,
        /// <summary>
        /// Bonus
        /// </summary>
        [Map("bonus")]
        Bonus,
        /// <summary>
        /// Rebate
        /// </summary>
        [Map("rebate")]
        Rebate,
        /// <summary>
        /// Event
        /// </summary>
        [Map("event")]
        Event,
        /// <summary>
        /// Copy trade
        /// </summary>
        [Map("copyTrade")]
        CopyTrade,
        /// <summary>
        /// Robot
        /// </summary>
        [Map("robot")]
        Robot,
        /// <summary>
        /// All
        /// </summary>
        [Map("all")]
        All,
    }

}

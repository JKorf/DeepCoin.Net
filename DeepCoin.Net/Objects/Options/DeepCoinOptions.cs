using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepCoin.Net.Objects.Options
{
    /// <summary>
    /// DeepCoin options
    /// </summary>
    public class DeepCoinOptions: LibraryOptions<DeepCoinRestOptions, DeepCoinSocketOptions, ApiCredentials, DeepCoinEnvironment>
    {
    }
}

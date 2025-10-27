# ![DeepCoin.Net](https://raw.githubusercontent.com/JKorf/DeepCoin.Net/main/DeepCoin.Net/Icon/icon.png) DeepCoin.Net  

[![.NET](https://img.shields.io/github/actions/workflow/status/JKorf/DeepCoin.Net/dotnet.yml?style=for-the-badge)](https://github.com/JKorf/DeepCoin.Net/actions/workflows/dotnet.yml) ![License](https://img.shields.io/github/license/JKorf/DeepCoin.Net?style=for-the-badge)

DeepCoin.Net is a client library for accessing the [DeepCoin REST and Websocket API](https://www.deepcoin.com/docs/authentication). 

## Features
* Response data is mapped to descriptive models
* Input parameters and response values are mapped to discriptive enum values where possible
* Automatic websocket (re)connection management 
* Client side rate limiting 
* Client side order book implementation
* Support for managing different accounts
* Extensive logging
* Support for different environments
* Easy integration with other exchange client based on the CryptoExchange.Net base library
* Native AOT support

## Supported Frameworks
The library is targeting both `.NET Standard 2.0` and `.NET Standard 2.1` for optimal compatibility, as well as dotnet 8.0 and 9.0 to use the latest framework features.

|.NET implementation|Version Support|
|--|--|
|.NET Core|`2.0` and higher|
|.NET Framework|`4.6.1` and higher|
|Mono|`5.4` and higher|
|Xamarin.iOS|`10.14` and higher|
|Xamarin.Android|`8.0` and higher|
|UWP|`10.0.16299` and higher|
|Unity|`2018.1` and higher|

## Install the library

### NuGet 
[![NuGet version](https://img.shields.io/nuget/v/DeepCoin.net.svg?style=for-the-badge)](https://www.nuget.org/packages/DeepCoin.Net)  [![Nuget downloads](https://img.shields.io/nuget/dt/DeepCoin.Net.svg?style=for-the-badge)](https://www.nuget.org/packages/DeepCoin.Net)

	dotnet add package DeepCoin.Net
	
### GitHub packages
DeepCoin.Net is available on [GitHub packages](https://github.com/JKorf/DeepCoin.Net/pkgs/nuget/DeepCoin.Net). You'll need to add `https://nuget.pkg.github.com/JKorf/index.json` as a NuGet package source.

### Download release
[![GitHub Release](https://img.shields.io/github/v/release/JKorf/DeepCoin.Net?style=for-the-badge&label=GitHub)](https://github.com/JKorf/DeepCoin.Net/releases)

The NuGet package files are added along side the source with the latest GitHub release which can found [here](https://github.com/JKorf/DeepCoin.Net/releases).

## How to use
* REST Endpoints
	```csharp
	// Get the ETH/USDT ticker via rest request
	var restClient = new DeepCoinRestClient();
    var tickerResult = await restClient.ExchangeApi.ExchangeData.GetTickersAsync(SymbolType.Spot);
    var ticker = tickerResult.Data.Single(x => x.Symbol == "ETH-USDT");
    var lastPrice = ticker.LastPrice;
	```
* Websocket streams
	```csharp
	// Subscribe to ETH/USDT ticker updates via the websocket API
	var socketClient = new DeepCoinSocketClient();
	var tickerSubscriptionResult = socketClient.ExchangeApi.SubscribeToSymbolUpdatesAsync("ETH-USDT", (update) => 
	{
	  var lastPrice = update.Data.LastPrice;
	});
	```

For information on the clients, dependency injection, response processing and more see the [documentation](https://cryptoexchange.jkorf.dev?library=DeepCoin.Net), or have a look at the examples [here](https://github.com/JKorf/DeepCoin.Net/tree/main/Examples) or [here](https://github.com/JKorf/CryptoExchange.Net/tree/master/Examples).

## CryptoExchange.Net
DeepCoin.Net is based on the [CryptoExchange.Net](https://github.com/JKorf/CryptoExchange.Net) base library. Other exchange API implementations based on the CryptoExchange.Net base library are available and follow the same logic.

CryptoExchange.Net also allows for [easy access to different exchange API's](https://cryptoexchange.jkorf.dev/client-libs/shared).

|Exchange|Repository|Nuget|
|--|--|--|
|Aster|[JKorf/Aster.Net](https://github.com/JKorf/Aster.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Aster.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Aster.Net)|
|Binance|[JKorf/Binance.Net](https://github.com/JKorf/Binance.Net)|[![Nuget version](https://img.shields.io/nuget/v/Binance.net.svg?style=flat-square)](https://www.nuget.org/packages/Binance.Net)|
|BingX|[JKorf/BingX.Net](https://github.com/JKorf/BingX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.BingX.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.BingX.Net)|
|Bitfinex|[JKorf/Bitfinex.Net](https://github.com/JKorf/Bitfinex.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bitfinex.net.svg?style=flat-square)](https://www.nuget.org/packages/Bitfinex.Net)|
|Bitget|[JKorf/Bitget.Net](https://github.com/JKorf/Bitget.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Bitget.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Bitget.Net)|
|BitMart|[JKorf/BitMart.Net](https://github.com/JKorf/BitMart.Net)|[![Nuget version](https://img.shields.io/nuget/v/BitMart.net.svg?style=flat-square)](https://www.nuget.org/packages/BitMart.Net)|
|BitMEX|[JKorf/BitMEX.Net](https://github.com/JKorf/BitMEX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.BitMEX.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.BitMEX.Net)|
|BloFin|[JKorf/BloFin.Net](https://github.com/JKorf/BloFin.Net)|[![Nuget version](https://img.shields.io/nuget/v/BloFin.net.svg?style=flat-square)](https://www.nuget.org/packages/BloFin.Net)|
|Bybit|[JKorf/Bybit.Net](https://github.com/JKorf/Bybit.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bybit.net.svg?style=flat-square)](https://www.nuget.org/packages/Bybit.Net)|
|Coinbase|[JKorf/Coinbase.Net](https://github.com/JKorf/Coinbase.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Coinbase.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Coinbase.Net)|
|CoinEx|[JKorf/CoinEx.Net](https://github.com/JKorf/CoinEx.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinEx.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinEx.Net)|
|CoinGecko|[JKorf/CoinGecko.Net](https://github.com/JKorf/CoinGecko.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinGecko.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinGecko.Net)|
|CoinW|[JKorf/CoinW.Net](https://github.com/JKorf/CoinW.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinW.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinW.Net)|
|Crypto.com|[JKorf/CryptoCom.Net](https://github.com/JKorf/CryptoCom.Net)|[![Nuget version](https://img.shields.io/nuget/v/CryptoCom.net.svg?style=flat-square)](https://www.nuget.org/packages/CryptoCom.Net)|
|Gate.io|[JKorf/GateIo.Net](https://github.com/JKorf/GateIo.Net)|[![Nuget version](https://img.shields.io/nuget/v/GateIo.net.svg?style=flat-square)](https://www.nuget.org/packages/GateIo.Net)|
|HTX|[JKorf/HTX.Net](https://github.com/JKorf/HTX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.HTX.net.svg?style=flat-square)](https://www.nuget.org/packages/Jkorf.HTX.Net)|
|HyperLiquid|[JKorf/HyperLiquid.Net](https://github.com/JKorf/HyperLiquid.Net)|[![Nuget version](https://img.shields.io/nuget/v/HyperLiquid.Net.svg?style=flat-square)](https://www.nuget.org/packages/HyperLiquid.Net)|
|Kraken|[JKorf/Kraken.Net](https://github.com/JKorf/Kraken.Net)|[![Nuget version](https://img.shields.io/nuget/v/KrakenExchange.net.svg?style=flat-square)](https://www.nuget.org/packages/KrakenExchange.Net)|
|Kucoin|[JKorf/Kucoin.Net](https://github.com/JKorf/Kucoin.Net)|[![Nuget version](https://img.shields.io/nuget/v/Kucoin.net.svg?style=flat-square)](https://www.nuget.org/packages/Kucoin.Net)|
|Mexc|[JKorf/Mexc.Net](https://github.com/JKorf/Mexc.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Mexc.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Mexc.Net)|
|OKX|[JKorf/OKX.Net](https://github.com/JKorf/OKX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.OKX.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.OKX.Net)|
|Toobit|[JKorf/Toobit.Net](https://github.com/JKorf/Toobit.Net)|[![Nuget version](https://img.shields.io/nuget/v/Toobit.net.svg?style=flat-square)](https://www.nuget.org/packages/Toobit.Net)|
|Upbit|[JKorf/Upbit.Net](https://github.com/JKorf/Upbit.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Upbit.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Upbit.Net)|
|WhiteBit|[JKorf/WhiteBit.Net](https://github.com/JKorf/WhiteBit.Net)|[![Nuget version](https://img.shields.io/nuget/v/WhiteBit.net.svg?style=flat-square)](https://www.nuget.org/packages/WhiteBit.Net)|
|XT|[JKorf/XT.Net](https://github.com/JKorf/XT.Net)|[![Nuget version](https://img.shields.io/nuget/v/XT.net.svg?style=flat-square)](https://www.nuget.org/packages/XT.Net)|

When using multiple of these API's the [CryptoClients.Net](https://github.com/JKorf/CryptoClients.Net) package can be used which combines this and the other packages and allows easy access to all exchange API's.

## Discord
[![Nuget version](https://img.shields.io/discord/847020490588422145?style=for-the-badge)](https://discord.gg/MSpeEtSY8t)  
A Discord server is available [here](https://discord.gg/MSpeEtSY8t). For discussion and/or questions around the CryptoExchange.Net and implementation libraries, feel free to join.

## Supported functionality

### REST
|API|Supported|Location|
|--|--:|--|
|DeepCoinAccount|✓|`restClient.ExchangeApi.Account`|
|DeepCoinMarket|✓|`restClient.ExchangeApi.ExchangeData`|
|DeepCoinTrade|✓|`restClient.ExchangeApi.Trading`|
|CopyTrade|X||
|Internal Transfer|X|(API not available)|
|Rebate|X||
|Assets|✓|`restClient.ExchangeApi.Account`|

### WebSocket
|API|Supported|Location|
|--|--:|--|
|Private websocket|✓|`restClient.ExchangeApi.ExchangeData`|
|Public websocket|✓|`restClient.ExchangeApi.ExchangeData`|

## Support the project
Any support is greatly appreciated.

### Referal
If you do not yet have an account please consider using this referal link to sign up:  
[Link](https://s.deepcoin.com/jddhfca)

### Donate
Make a one time donation in a crypto currency of your choice. If you prefer to donate a currency not listed here please contact me.

**Btc**:  bc1q277a5n54s2l2mzlu778ef7lpkwhjhyvghuv8qf  
**Eth**:  0xcb1b63aCF9fef2755eBf4a0506250074496Ad5b7   
**USDT (TRX)**  TKigKeJPXZYyMVDgMyXxMf17MWYia92Rjd 

### Sponsor
Alternatively, sponsor me on Github using [Github Sponsors](https://github.com/sponsors/JKorf). 

## Release notes
* Version 2.9.0 - 16 Oct 2025
    * Updated CryptoExchange.Net version to 9.10.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Added ClientOrderId mapping on SharedUserTrade models

* Version 2.8.0 - 30 Sep 2025
    * Updated CryptoExchange.Net version to 9.8.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Added ITrackerFactory to TrackerFactory implementation

* Version 2.7.0 - 01 Sep 2025
    * Updated CryptoExchange.Net version to 9.7.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * HTTP REST requests will now use HTTP version 2.0 by default

* Version 2.6.0 - 25 Aug 2025
    * Updated CryptoExchange.Net version to 9.6.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Added ClearUserClients method to user client provider

* Version 2.5.1 - 21 Aug 2025
    * Added error handling for non 200 status responses

* Version 2.5.0 - 20 Aug 2025
    * Updated CryptoExchange.Net to version 9.5.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Added improved error parsing
    * Updated rest request sending too prevent duplicate parameter serialization
    * Fixed error responses not correctly getting logged as error

* Version 2.4.0 - 04 Aug 2025
    * Updated CryptoExchange.Net to version 9.4.0, see https://github.com/JKorf/CryptoExchange.Net/releases/

* Version 2.3.0 - 23 Jul 2025
    * Updated CryptoExchange.Net to version 9.3.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Updated websocket message matching

* Version 2.2.1 - 16 Jul 2025
    * Updated CryptoExchange.Net to version 9.2.1, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Fixed issue with websocket ping response parsing

* Version 2.2.0 - 15 Jul 2025
    * Updated CryptoExchange.Net to version 9.2.0, see https://github.com/JKorf/CryptoExchange.Net/releases/

* Version 2.1.0 - 02 Jun 2025
    * Updated CryptoExchange.Net to version 9.1.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Added (I)DeepCoinUserClientProvider allowing for easy client management when handling multiple users

* Version 2.0.0 - 13 May 2025
    * Updated CryptoExchange.Net to version 9.0.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Added support for Native AOT compilation
    * Added RateLimitUpdated event
    * Added SharedSymbol response property to all Shared interfaces response models returning a symbol name
    * Added GenerateClientOrderId method to ExchangeApi Shared clients
    * Added IBookTickerRestClient implementation to ExchangeApi Shared client
    * Added takeProfitPrice, stopLossPrice support to ExchangeApi Shared PlaceFuturesOrderAsync
    * Added TakeProfitPrice, StopLossPrice properties to SharedFuturesOrder model
    * Added MaxLongLeverage, MaxShortLeverage to SharedFuturesSymbol model
    * Added OptionalExchangeParameters and Supported properties to EndpointOptions
    * Added All property to retrieve all available environment on DeepCoinEnvironment
    * Refactored Shared clients quantity parameters and responses to use SharedQuantity
    * Replaced DeepCoinApiCredentials with ApiCredentials
    * Updated all IEnumerable response and model types to array response types
    * Removed Newtonsoft.Json dependency
    * Fixed incorrect Shared SpotSupportOrderQuantity configuration for limit orders
    * Fixed Shared spot PlaceOrder by quote asset quantity
    * Fixed DeepCoinExchange.ImageUrl link
    * Fixed incorrect DataTradeMode on certain Shared interface responses
    * Fixed parsing of order types
    * Fixed some typos

* Version 2.0.0-beta3 - 01 May 2025
    * Updated CryptoExchange.Net version to 9.0.0-beta5
    * Added property to retrieve all available API environments
    * Disabled parsing of Shared spot ticker QuoteVolume as value is incorrect

* Version 2.0.0-beta2 - 23 Apr 2025
    * Updated CryptoExchange.Net to version 9.0.0-beta2
    * Added Shared spot ticker QuoteVolume mapping
    * Fixed incorrect DataTradeMode on responses

* Version 2.0.0-beta1 - 22 Apr 2025
    * Updated CryptoExchange.Net to version 9.0.0-beta1, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Added support for Native AOT compilation
    * Added RateLimitUpdated event
    * Added SharedSymbol response property to all Shared interfaces response models returning a symbol name
    * Added GenerateClientOrderId method to ExchangeApi Shared clients
    * Added IBookTickerRestClient implementation to ExchangeApi Shared client
    * Added takeProfitPrice, stopLossPrice support to ExchangeApi Shared PlaceFuturesOrderAsync
    * Added TakeProfitPrice, StopLossPrice properties to SharedFuturesOrder model
    * Added MaxLongLeverage, MaxShortLeverage to SharedFuturesSymbol model
    * Added OptionalExchangeParameters and Supported properties to EndpointOptions
    * Refactored Shared clients quantity parameters and responses to use SharedQuantity
    * Replaced DeepCoinApiCredentials with ApiCredentials
    * Updated all IEnumerable response and model types to array response types
    * Removed Newtonsoft.Json dependency
    * Fixed parsing of order types
    * Fixed some typos

* Version 1.0.5 - 06 Mar 2025
    * Fixed restClient.ExchangeApi.Account.KeepAliveUserStreamAsync endpoint

* Version 1.0.4 - 04 Mar 2025
    * Fix for handling websocket order book updates with the same sequence number split over 2 messages

* Version 1.0.3 - 04 Mar 2025
    * Fixed Volume and QuoteVolume properties being inversed on DeepCoinTicker model
    * Fixed DeepCoinOrderBookFactory Create with SharedSymbol parameter not formatting correctly

* Version 1.0.2 - 04 Mar 2025
    * Fix for spot websocket subscriptions not getting updates

* Version 1.0.1 - 04 Mar 2025
    * Added Type property to DeepCoinExchange
    * Updated DeepCoinOptions to derive from LibraryOptions

* Version 1.0.0 - 04 Mar 2025
    * Initial release


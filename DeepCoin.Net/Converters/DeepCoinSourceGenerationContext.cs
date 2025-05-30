using CryptoExchange.Net.Objects;
using DeepCoin.Net.Objects.Internal;
using DeepCoin.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CryptoCom.Net.Converters
{
    [JsonSerializable(typeof(string[]))]
    [JsonSerializable(typeof(SocketRequest))]

    // End manual defined attributes

    [JsonSerializable(typeof(DeepCoinResponse<DeepCoinBalance[]>))]
    [JsonSerializable(typeof(DeepCoinResponse<DeepCoinBill[]>))]
    [JsonSerializable(typeof(DeepCoinResponse<DeepCoinTicker[]>))]
    [JsonSerializable(typeof(DeepCoinResponse<DeepCoinSymbol[]>))]
    [JsonSerializable(typeof(DeepCoinResponse<DeepCoinKline[]>))]
    [JsonSerializable(typeof(DeepCoinResponse<DeepCoinFundingRate[]>))]
    [JsonSerializable(typeof(DeepCoinResponse<DeepCoinPosition[]>))]
    [JsonSerializable(typeof(DeepCoinResponse<DeepCoinUserTrade[]>))]
    [JsonSerializable(typeof(DeepCoinResponse<DeepCoinOrder[]>))]
    [JsonSerializable(typeof(DeepCoinResponse<DeepCoinLeverage>))]
    [JsonSerializable(typeof(DeepCoinResponse<DeepCoinTransferResult>))]
    [JsonSerializable(typeof(DeepCoinResponse<DeepCoinTransferPage>))]
    [JsonSerializable(typeof(DeepCoinResponse<DeepCoinDepositPage>))]
    [JsonSerializable(typeof(DeepCoinResponse<DeepCoinWithdrawPage>))]
    [JsonSerializable(typeof(DeepCoinResponse<DeepCoinListenKey>))]
    [JsonSerializable(typeof(DeepCoinResponse<DeepCoinOrderBook>))]
    [JsonSerializable(typeof(DeepCoinResponse<DeepCoinOrderResult>))]
    [JsonSerializable(typeof(DeepCoinResponse<DeepCoinCancellationResult>))]
    [JsonSerializable(typeof(Dictionary<string, object>))]
    [JsonSerializable(typeof(SocketUpdate<DeepCoinOrderBookUpdateEntry>))]
    [JsonSerializable(typeof(SocketUpdate<DeepCoinSymbolUpdate>))]
    [JsonSerializable(typeof(SocketUpdate<DeepCoinTradeUpdate>))]
    [JsonSerializable(typeof(SocketUpdate<DeepCoinKlineUpdate>))]
    [JsonSerializable(typeof(SocketUpdate<DeepCoinOrderUpdate>))]
    [JsonSerializable(typeof(SocketUpdate<DeepCoinBalanceUpdate>))]
    [JsonSerializable(typeof(SocketUpdate<DeepCoinPositionUpdate>))]
    [JsonSerializable(typeof(SocketUpdate<DeepCoinUserTradeUpdate>))]
    [JsonSerializable(typeof(SocketUpdate<DeepCoinAccountUpdate>))]
    [JsonSerializable(typeof(SocketUpdate<DeepCoinTriggerOrderUpdate>))]
    [JsonSerializable(typeof(IDictionary<string, object>))]
    [JsonSerializable(typeof(DeepCoinOrderUpdate[]))]
    [JsonSerializable(typeof(DeepCoinBalanceUpdate[]))]
    [JsonSerializable(typeof(DeepCoinPositionUpdate[]))]
    [JsonSerializable(typeof(DeepCoinUserTradeUpdate[]))]
    [JsonSerializable(typeof(DeepCoinAccountUpdate[]))]
    [JsonSerializable(typeof(DeepCoinTriggerOrderUpdate[]))]
    [JsonSerializable(typeof(DeepCoinResponse[]))]
    [JsonSerializable(typeof(DeepCoinCancellationResult[]))]
    [JsonSerializable(typeof(DeepCoinCancellationResultEntry[]))]
    [JsonSerializable(typeof(DeepCoinDepositPage[]))]
    [JsonSerializable(typeof(DeepCoinDeposit[]))]
    [JsonSerializable(typeof(DeepCoinKlineUpdate[]))]
    [JsonSerializable(typeof(DeepCoinLeverage[]))]
    [JsonSerializable(typeof(DeepCoinListenKey[]))]
    [JsonSerializable(typeof(DeepCoinOrderBook[]))]
    [JsonSerializable(typeof(DeepCoinOrderBookEntry[]))]
    [JsonSerializable(typeof(DeepCoinOrderBookUpdate[]))]
    [JsonSerializable(typeof(DeepCoinOrderBookUpdateEntry[]))]
    [JsonSerializable(typeof(DeepCoinOrderResult[]))]
    [JsonSerializable(typeof(DeepCoinSymbolUpdate[]))]
    [JsonSerializable(typeof(DeepCoinTradeUpdate[]))]
    [JsonSerializable(typeof(DeepCoinTransferPage[]))]
    [JsonSerializable(typeof(DeepCoinTransfer[]))]
    [JsonSerializable(typeof(DeepCoinTransferResult[]))]
    [JsonSerializable(typeof(DeepCoinWithdrawPage[]))]
    [JsonSerializable(typeof(DeepCoinWithdrawal[]))]
    [JsonSerializable(typeof(string))]
    [JsonSerializable(typeof(SocketResponse))]
    [JsonSerializable(typeof(int?))]
    [JsonSerializable(typeof(int))]
    [JsonSerializable(typeof(long?))]
    [JsonSerializable(typeof(long))]
    [JsonSerializable(typeof(decimal?))]
    [JsonSerializable(typeof(decimal))]
    [JsonSerializable(typeof(DateTime))]
    [JsonSerializable(typeof(DateTime?))]
    internal partial class DeepCoinSourceGenerationContext : JsonSerializerContext
    {
    }
}

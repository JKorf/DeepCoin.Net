using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeepCoin.Net.Clients;
using DeepCoin.Net.Enums;
using NUnit.Framework.Constraints;
using DeepCoin.Net.Objects;
using System.Linq;
using System.Drawing;
using CryptoExchange.Net.Authentication;

namespace DeepCoin.Net.UnitTests
{
    [TestFixture]
    public class RestRequestTests
    {
        [TestCase(false)]
        [TestCase(true)]
        public async Task ValidateAccountCalls(bool useUpdatedDeserialization)
        {
            var client = new DeepCoinRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.UseUpdatedDeserialization = useUpdatedDeserialization;
                opts.ApiCredentials = new ApiCredentials("123", "456", "789");
            });
            var tester = new RestRequestValidator<DeepCoinRestClient>(client, "Endpoints/Exchange/Account", "https://api.deepcoin.com", IsAuthenticated, nestedPropertyForCompare: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.Account.GetBalancesAsync(SymbolType.Spot), "GetBalances", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.Account.GetBillsAsync(SymbolType.Spot), "GetBills", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.Account.SetLeverageAsync("123", 0.1m, TradeMode.Cross, PositionType.Merge), "SetLeverage", nestedJsonProperty: "data");
            //await tester.ValidateAsync(client => client.ExchangeApi.Account.TransferAsync("123", 0.1m, "123", AccountType.Email), "Transfer", nestedJsonProperty: "data");
            //await tester.ValidateAsync(client => client.ExchangeApi.Account.GetTransferHistoryAsync(), "GetTransferHistory", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.Account.GetDepositHistoryAsync(), "GetDepositHistory", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.Account.GetWithdrawHistoryAsync(), "GetWithdrawHistory", nestedJsonProperty: "data");
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task ValidateExchangeDataCalls(bool useUpdatedDeserialization)
        {
            var client = new DeepCoinRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.UseUpdatedDeserialization = useUpdatedDeserialization;
                opts.ApiCredentials = new ApiCredentials("123", "456", "789");
            });
            var tester = new RestRequestValidator<DeepCoinRestClient>(client, "Endpoints/Exchange/ExchangeData", "https://api.deepcoin.com", IsAuthenticated, nestedPropertyForCompare: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetTickersAsync(Enums.SymbolType.Spot), "GetTickers", ignoreProperties: ["lastSz", "sodUtc0", "sodUtc8"]);
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetSymbolsAsync(SymbolType.Swap), "GetSymbols", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetKlinesAsync("123", KlineInterval.FiveMinutes), "GetKlines", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetOrderBookAsync("123"), "GetOrderBook", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetFundingRateAsync(ProductGroup.USDTMargined), "GetFundingRate", nestedJsonProperty: "data");
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task ValidateTradingCalls(bool useUpdatedDeserialization)
        {
            var client = new DeepCoinRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.UseUpdatedDeserialization = useUpdatedDeserialization;
                opts.ApiCredentials = new ApiCredentials("123", "456", "789");
            });
            var tester = new RestRequestValidator<DeepCoinRestClient>(client, "Endpoints/Exchange/Trading", "https://api.deepcoin.com", IsAuthenticated, nestedPropertyForCompare: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.GetPositionsAsync(SymbolType.Swap), "GetPositions", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.PlaceOrderAsync("123", OrderSide.Buy, OrderType.Limit, 0.1m), "PlaceOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.EditOrderAsync("123"), "EditOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.CancelOrderAsync("123", "123"), "CancelOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.CancelOrdersAsync(["123"]), "CancelOrders", nestedJsonProperty: "data.errorList");
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.CancelAllOrdersAsync("123", ProductGroup.CoinMargined, TradeMode.Isolated, PositionType.Merge), "CancelAllOrders", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.GetUserTradesAsync(SymbolType.Swap), "GetUserTrades", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.GetOpenOrderAsync("123", "123"), "GetOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.GetClosedOrderAsync("123", "123"), "GetClosedOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.GetClosedOrdersAsync(SymbolType.Swap), "GetClosedOrders", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.GetOpenOrdersAsync("123"), "GetOpenOrders", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.SetTpSlAsync("123"), "SetTpSl");
        }

        private bool IsAuthenticated(WebCallResult result)
        {
            return result.RequestHeaders.Any(x => x.Key == "DC-ACCESS-SIGN");
        }
    }
}

using System.Text.Json.Serialization;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// V2 bill sources returned by account history. These differ from the request filter categories in BillType.
    /// </summary>
    [JsonConverter(typeof(EnumConverter<V2BillType>))]
    public enum V2BillType
    {
        /// <summary>
        /// [<c>1</c>] Profit and loss.
        /// </summary>
        [Map("1")]
        ProfitLoss = 1,
        /// <summary>
        /// [<c>2</c>] Income and expenditure.
        /// </summary>
        [Map("2")]
        IncomeAndExpenditure = 2,
        /// <summary>
        /// [<c>3</c>] System transfer in.
        /// </summary>
        [Map("3")]
        SystemTransferIn = 3,
        /// <summary>
        /// [<c>4</c>] Transfer out.
        /// </summary>
        [Map("4")]
        TransferOut = 4,
        /// <summary>
        /// [<c>5</c>] Transaction fee.
        /// </summary>
        [Map("5")]
        TransactionFee = 5,
        /// <summary>
        /// [<c>7</c>] Funding fee.
        /// </summary>
        [Map("7")]
        FundingFee = 7,
        /// <summary>
        /// [<c>8</c>] Settlement.
        /// </summary>
        [Map("8")]
        Settlement = 8,
        /// <summary>
        /// [<c>a</c>] Liquidation.
        /// </summary>
        [Map("a")]
        Liquidation,
        /// <summary>
        /// [<c>g</c>] Withheld profit.
        /// </summary>
        [Map("g")]
        WithheldProfit,
        /// <summary>
        /// [<c>h</c>] Refunded withheld profit share.
        /// </summary>
        [Map("h")]
        RefundedProfitShare,
        /// <summary>
        /// [<c>i</c>] Copy trading profit share.
        /// </summary>
        [Map("i")]
        CopyTradingProfitShare,
        /// <summary>
        /// [<c>j</c>] Trial bonus issuance.
        /// </summary>
        [Map("j")]
        TrialBonusIssuance,
        /// <summary>
        /// [<c>k</c>] Trial money recovery.
        /// </summary>
        [Map("k")]
        TrialMoneyRecovery
    }
}

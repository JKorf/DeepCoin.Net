namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Physical wallet identifiers for V2 internal transfers.
    /// </summary>
    public enum TransferAccountType
    {
        /// <summary>
        /// [<c>1</c>] Spot trading wallet.
        /// </summary>
        Spot = 1,
        /// <summary>
        /// [<c>2</c>] Funding wallet.
        /// </summary>
        Funding = 2,
        /// <summary>
        /// [<c>3</c>] Rebate wallet.
        /// </summary>
        Rebate = 3,
        /// <summary>
        /// [<c>5</c>] Coin-margined swap wallet.
        /// </summary>
        InverseSwap = 5,
        /// <summary>
        /// [<c>7</c>] USDT-margined swap wallet.
        /// </summary>
        LinearSwap = 7,
        /// <summary>
        /// [<c>10</c>] Trial fund wallet.
        /// </summary>
        TrialFund = 10
    }
}

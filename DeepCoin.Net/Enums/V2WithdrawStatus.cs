using System.Text.Json.Serialization;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace DeepCoin.Net.Enums
{
    /// <summary>
    /// Normalized V2 withdrawal status.
    /// </summary>
    [JsonConverter(typeof(EnumConverter<V2WithdrawStatus>))]
    public enum V2WithdrawStatus
    {
        /// <summary>
        /// [<c>pending</c>] Pending processing.
        /// </summary>
        [Map("pending")]
        Pending,
        /// <summary>
        /// [<c>auditing</c>] Under review.
        /// </summary>
        [Map("auditing")]
        Auditing,
        /// <summary>
        /// [<c>cancelling</c>] Cancellation in progress.
        /// </summary>
        [Map("cancelling")]
        Cancelling,
        /// <summary>
        /// [<c>succeed</c>] Completed successfully.
        /// </summary>
        [Map("succeed")]
        Success,
        /// <summary>
        /// [<c>failed</c>] Failed.
        /// </summary>
        [Map("failed")]
        Failed,
        /// <summary>
        /// [<c>cancelled</c>] Cancelled.
        /// </summary>
        [Map("cancelled")]
        Cancelled
    }
}

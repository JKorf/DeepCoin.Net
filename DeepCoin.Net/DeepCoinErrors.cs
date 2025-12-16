using CryptoExchange.Net.Objects.Errors;

namespace DeepCoin.Net
{
    internal static class DeepCoinErrors
    {
        public static ErrorMapping Errors { get; } = new ErrorMapping([

            new ErrorInfo(ErrorType.Unauthorized, false, "API access frozen, contact customer service", "50100"),
            new ErrorInfo(ErrorType.Unauthorized, false, "API environment not correct", "50101"),
            new ErrorInfo(ErrorType.Unauthorized, false, "Incorrect passphrase", "50105"),
            new ErrorInfo(ErrorType.Unauthorized, false, "IP address not allowed", "50110"),
            new ErrorInfo(ErrorType.Unauthorized, false, "Invalid API key", "50113"),

            new ErrorInfo(ErrorType.InvalidTimestamp, false, "Request timestamp expired", "50102"),
            new ErrorInfo(ErrorType.InvalidTimestamp, false, "Invalid timestamp", "50112"),

            new ErrorInfo(ErrorType.InvalidTimestamp, false, "Invalid signature", "50111"),

            new ErrorInfo(ErrorType.UnknownSymbol, false, "Unknown symbol", "50"),

            new ErrorInfo(ErrorType.InsufficientBalance, false, "Insufficient balance", "36"),

            new ErrorInfo(ErrorType.InvalidPrice, false, "Invalid price", "175"),

            new ErrorInfo(ErrorType.InvalidQuantity, false, "Order quantity tick invalid", "44"),
            new ErrorInfo(ErrorType.InvalidQuantity, false, "Order quantity too large", "193"),
            new ErrorInfo(ErrorType.InvalidQuantity, false, "Order quantity too small", "194"),

            ]);
    }
}

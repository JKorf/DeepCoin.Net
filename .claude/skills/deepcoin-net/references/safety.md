# Operational considerations

## Credentials and user-stream ownership

Private operations require API key, secret, and passphrase through `DeepCoinCredentials`. Keep all three outside source and grant permissions for the selected operations. Clients default to `DeepCoinEnvironment.Live`; custom addresses do not imply simulated trading.

The credentialed socket overload without a listen key obtains and renews one automatically. The explicit-key overload leaves key creation and keepalive to the application using `StartUserStreamAsync` and `KeepAliveUserStreamAsync`. Treat listen keys as private account access and keep ownership clear during reconnects and shutdown.

## Spot and swap execution

Use listed instrument metadata for price increments, quantity steps, and minimums. Spot market quantity can be base or quote according to `QuantityType`; select it deliberately. Limit orders can execute immediately when the price crosses the book.

Swap quantities are contracts. Match `TradeMode.Cross`/`Isolated`, `PositionType.Merge`/`Split`, `PositionSide`, and leverage to the actual account position settings. Leverage and TP/SL changes affect exposure. Closing a selected position uses its position ID, appropriate opposite order side, matching position side/mode, and reduce-only behavior; verify the position belongs to the intended workflow rather than closing pre-existing exposure accidentally.

The native edit endpoint is documented as unsupported for spot. Use open versus closed order lookup methods according to order state rather than assuming a single `GetOrderAsync` method exists.

## Cancellation and nested results

Single-order responses contain `ResultCode` and `ResultMessage`; native placement/cancellation maps reported nonzero codes to outer errors. Batch and cancel-all calls return `DeepCoinCancellationResult.ErrorList`: inspect each failed order's ID, error code, and message after outer success.

Cancellation can race with a fill. Reconcile order state and trades instead of assuming an accepted cancellation prevented execution. Cancel-all requires symbol, product group, margin mode, and position type; preserve all intended scope filters.

## Uncertain submissions and retries

A timeout or lost response can occur after acceptance of an order, cancellation, or settings change. Query supported order/client identifiers and relevant account/position state before deciding whether to resubmit. Use a durable client-order ID when correlation is needed, without assuming that it alone proves retry idempotency. Transient errors do not make account-changing calls safe to repeat; bound public-read retries separately.

## Funding scope

The public library exposes deposit and withdrawal history. Transfer methods are commented out as unusable, and there is no public withdrawal-placement capability in this surface. Do not infer fund-movement methods from another exchange's interface.

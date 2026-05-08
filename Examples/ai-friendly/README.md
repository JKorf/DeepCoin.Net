# AI-Friendly Examples

These examples are optimized for AI coding assistants and quick onboarding. Each file is:

- **Compilable** - drop into a console project with `dotnet add package DeepCoin.Net` and it builds (you only need to substitute API keys for trading examples).
- **Self-contained** - single file, no external setup, no shared helpers.
- **Heavily commented** - explains why each line is present, not just what it does.
- **Idiomatic** - follows current DeepCoin.Net 3.x patterns.

## Files

| File | What it shows |
|---|---|
| `01-spot-quickstart.cs` | Client setup, public ticker lookup, authenticated account balance, place limit order, query/cancel order |
| `02-futures.cs` | Swap/futures: set leverage, place market order, get position, close position |
| `03-websocket.cs` | Subscribe to ticker, trade, kline, order book, and user data streams with proper teardown |
| `04-multi-exchange.cs` | `CryptoExchange.Net.SharedApis` pattern for exchange-agnostic code |
| `05-error-handling.cs` | `WebCallResult` patterns, retry, common error scenarios |

## Running

```bash
dotnet new console -n MyDeepCoinApp
cd MyDeepCoinApp
dotnet add package DeepCoin.Net
# Copy the example .cs file content into Program.cs
# Replace API_KEY / API_SECRET / API_PASS placeholders with your own for private examples
dotnet run
```

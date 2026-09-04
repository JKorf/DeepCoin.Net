using CryptoExchange.Net;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using DeepCoin.Net.Enums;
using DeepCoin.Net.Interfaces.Clients.ExchangeApi;
using DeepCoin.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeepCoin.Net.Clients.ExchangeApi
{
    internal partial class DeepCoinRestClientExchangeSharedApi
    {
        public SharedLeverageSettingMode LeverageSettingType => SharedLeverageSettingMode.PerSymbol;

        #region Get Leverage

        async Task<ICallResult<SharedLeverage>> IGetLeverage.GetLeverageAsync(GetLeverageRequest request, CancellationToken ct)
            => await GetLeverageAsync(request, ct).ConfigureAwait(false);

        public GetLeverageOptions GetLeverageOptions { get; } = new GetLeverageOptions(_exchangeName, true);
        public async Task<HttpResult<SharedLeverage>> GetLeverageAsync(GetLeverageRequest request, CancellationToken ct)
        {
            var validationError = GetLeverageOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedLeverage>(Exchange, validationError);

            var result = await _api.Trading.GetPositionsAsync(SymbolType.Swap, symbol: request.Symbol!.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedLeverage>(result);

            if (!result.Data.Any())
                return HttpResult.Fail<SharedLeverage>(result, new ServerError(new ErrorInfo(ErrorType.Unknown, "Not found")));

            return HttpResult.Ok(result, new SharedLeverage(result.Data.First().Leverage)
            {
                Side = request.PositionSide
            });
        }

        #endregion

        #region Set Leverage

        async Task<ICallResult<SharedLeverage>> ISetLeverage.SetLeverageAsync(SetLeverageRequest request, CancellationToken ct)
            => await SetLeverageAsync(request, ct).ConfigureAwait(false);

        public SetLeverageOptions SetLeverageOptions { get; } = new SetLeverageOptions(_exchangeName)
        {
            RequiredRequestParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(SetLeverageRequest.MarginMode), typeof(SharedMarginMode), "Margin mode to set leverage for", TradeMode.Cross)
            },
            OptionalExchangeParameters = new List<ParameterDescription>
            {
                new ParameterDescription(["PositionType", "mrgPosition"], typeof(PositionType), "Merge or split position mode", PositionType.Merge)
            }
        };

        public async Task<HttpResult<SharedLeverage>> SetLeverageAsync(SetLeverageRequest request, CancellationToken ct)
        {
            var validationError = SetLeverageOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedLeverage>(Exchange, validationError);

            var positionType = request.GetParamValue<PositionType?>(Exchange, "PositionType", "mrgPosition");
            var result = await _api.Account.SetLeverageAsync(
                symbol: request.Symbol!.GetSymbol(FormatSymbol), 
                request.Leverage,
                tradeMode: request.MarginMode == SharedMarginMode.Cross ? TradeMode.Cross : TradeMode.Isolated,
                positionType ?? PositionType.Merge,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedLeverage>(result);

            return HttpResult.Ok(result, new SharedLeverage(result.Data.Leverage));
        }

        #endregion
    }
}

////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading.Tasks;
using System.Threading;
using SharedLib;
using System.Collections.Generic;

namespace RemoteCallLib;

/// <summary>
/// 
/// </summary>
public partial class DataStockSharpTransmission(IMQTTClient mqClient) : IStockSharpDataService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<List<BoardStockSharpModel>>> GetBoardsAsync(int[]? req = null, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<List<BoardStockSharpModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetBoardsStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<ExchangeStockSharpModel>>> GetExchangesAsync(int[]? req = null, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<List<ExchangeStockSharpModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetExchangesStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<InstrumentTradeStockSharpModel>>> GetInstrumentsAsync(int[]? req = null, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<List<InstrumentTradeStockSharpModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetInstrumentsStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<OrderStockSharpModel>>> GetOrdersAsync(int[]? req = null, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<List<OrderStockSharpModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetOrdersStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<PortfolioStockSharpModel>>> GetPortfoliosAsync(int[]? req = null, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<List<PortfolioStockSharpModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetPortfoliosStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> InstrumentFavoriteToggleAsync(InstrumentTradeStockSharpViewModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.InstrumentFavoriteToggleStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<InstrumentTradeStockSharpViewModel>> InstrumentsSelectAsync(TPaginationRequestStandardModel<InstrumentsRequestModel> req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TPaginationResponseModel<InstrumentTradeStockSharpViewModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.InstrumentsSelectStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> SaveBoard(BoardStockSharpModel req)
        => await mqClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.SaveBoardStockSharpReceive, req) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> SaveExchange(ExchangeStockSharpModel req)
        => await mqClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.SaveExchangeStockSharpReceive, req) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> SaveInstrument(InstrumentTradeStockSharpModel req)
        => await mqClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.SaveInstrumentStockSharpReceive, req) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> SaveOrder(OrderStockSharpModel req)
        => await mqClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.SaveOrderStockSharpReceive, req) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> SavePortfolio(PortfolioStockSharpModel req)
        => await mqClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.SavePortfolioStockSharpReceive, req) ?? new();
}
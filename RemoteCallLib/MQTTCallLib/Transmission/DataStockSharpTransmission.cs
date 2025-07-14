////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// DataStockSharpTransmission
/// </summary>
public partial class DataStockSharpTransmission(IMQTTClient mqClient) : IDataStockSharpService
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateInstrumentAsync(InstrumentTradeStockSharpViewModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.UpdateInstrumentStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<InstrumentTradeStockSharpViewModel>>> GetInstrumentsAsync(int[]? req = null, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<List<InstrumentTradeStockSharpViewModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetInstrumentsStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> InstrumentFavoriteToggleAsync(InstrumentTradeStockSharpViewModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.InstrumentFavoriteToggleStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<InstrumentTradeStockSharpViewModel>> InstrumentsSelectAsync(InstrumentsRequestModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TPaginationResponseModel<InstrumentTradeStockSharpViewModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.InstrumentsSelectStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetMarkersForInstrumentAsync(SetMarkersForInstrumentRequestModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.SetMarkersForInstrumentStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<MarkerInstrumentStockSharpViewModel>>> GetMarkersForInstrumentAsync(int instrumentId, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<List<MarkerInstrumentStockSharpViewModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetMarkersForInstrumentsStockSharpReceive, instrumentId, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<BoardStockSharpViewModel>>> GetBoardsAsync(int[]? req = null, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<List<BoardStockSharpViewModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetBoardsStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<ExchangeStockSharpModel>>> GetExchangesAsync(int[]? req = null, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<List<ExchangeStockSharpModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetExchangesStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<OrderStockSharpModel>>> GetOrdersAsync(int[]? req = null, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<List<OrderStockSharpModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetOrdersStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<PortfolioStockSharpViewModel>>> GetPortfoliosAsync(int[]? req = null, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<List<PortfolioStockSharpViewModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetPortfoliosStockSharpReceive, req, token: cancellationToken) ?? new();
}
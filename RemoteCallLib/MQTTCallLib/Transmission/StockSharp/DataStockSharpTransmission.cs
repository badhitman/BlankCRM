////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteCallLib;

/// <summary>
/// DataStockSharpTransmission
/// </summary>
public partial class DataStockSharpTransmission(IMQTTClient mqClient) : IDataStockSharpService
{
    #region CashFlow
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CashFlowDelete(int cashFlowId, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.CashFlowDeleteStockSharpReceive, cashFlowId, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<CashFlowViewModel>>> CashFlowList(int instrumentId, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<List<CashFlowViewModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.CashFlowListStockSharpReceive, instrumentId, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CashFlowUpdateAsync(CashFlowViewModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.CashFlowUpdateStockSharpReceive, req, token: cancellationToken) ?? new();
    #endregion

    #region Instrument
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateInstrumentAsync(InstrumentTradeStockSharpViewModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.UpdateInstrumentStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<InstrumentTradeStockSharpViewModel>>> GetInstrumentsAsync(int[]? req = null, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<List<InstrumentTradeStockSharpViewModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetInstrumentsStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> InstrumentFavoriteToggleAsync(int instrumentId, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.InstrumentFavoriteToggleStockSharpReceive, instrumentId, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<InstrumentTradeStockSharpViewModel>> InstrumentsSelectAsync(InstrumentsRequestModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TPaginationResponseModel<InstrumentTradeStockSharpViewModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.InstrumentsSelectStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetMarkersForInstrumentAsync(SetMarkersForInstrumentRequestModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.SetMarkersForInstrumentStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<MarkerInstrumentStockSharpViewModel>>> GetMarkersForInstrumentAsync(int instrumentId, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<List<MarkerInstrumentStockSharpViewModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetMarkersForInstrumentsStockSharpReceive, instrumentId, token: cancellationToken) ?? new();
    #endregion

    #region rubrics/instruments
    /// <inheritdoc/>
    public async Task<TResponseModel<List<PortfolioStockSharpViewModel>>> GetPortfoliosAsync(int[]? req = null, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<List<PortfolioStockSharpViewModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetPortfoliosStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<BoardStockSharpViewModel>>> GetBoardsAsync(int[]? req = null, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<List<BoardStockSharpViewModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetBoardsStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<ExchangeStockSharpModel>>> GetExchangesAsync(int[]? req = null, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<List<ExchangeStockSharpModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetExchangesStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> RubricsInstrumentUpdateAsync(RubricsInstrumentUpdateModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.RubricsInstrumentUpdateStockSharpReceive, req, token: cancellationToken) ?? new();
    #endregion

    /// <inheritdoc/>
    public async Task<TResponseModel<List<OrderStockSharpModel>>> GetOrdersAsync(int[]? req = null, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<List<OrderStockSharpModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetOrdersStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> InstrumentRubricUpdateAsync(InstrumentRubricUpdateModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.InstrumentRubricUpdateStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<UniversalBaseModel>>> GetRubricsForInstrumentAsync(int idInstrument, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<List<UniversalBaseModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetRubricsForInstrumentStockSharpReceive, idInstrument, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<InstrumentTradeStockSharpViewModel>>> GetInstrumentsForRubricAsync(int idRubric, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<List<InstrumentTradeStockSharpViewModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetInstrumentsForRubricStockSharpReceive, idRubric, token: cancellationToken) ?? new();
}
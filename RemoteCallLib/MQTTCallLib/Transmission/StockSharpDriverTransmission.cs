////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// StockSharpDriverTransmission
/// </summary>
public partial class StockSharpDriverTransmission(IMQTTClient mqClient) : IStockSharpDriverService
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> Connect(CancellationToken? cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ConnectStockSharpReceive, token: cancellationToken ?? CancellationToken.None) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> Disconnect(CancellationToken? cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DisconnectStockSharpReceive, token: cancellationToken ?? CancellationToken.None) ?? new();

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
    public async Task<ResponseBaseModel> OrderRegisterAsync(CreateOrderRequestModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.OrderRegisterStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> PingAsync(CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.PingStockSharpDriverReceive, token: cancellationToken) ?? new();
}
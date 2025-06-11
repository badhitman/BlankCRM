////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading.Tasks;
using System.Threading;
using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// StockSharpEventsServiceTransmission
/// </summary>
public partial class StockSharpEventsServiceTransmission(IMQTTClient mqClient) : IEventsStockSharpService
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> BoardReceived(BoardStockSharpModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.BoardReceivedStockSharpNotifyReceive, req, waitResponse: false, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> InstrumentReceived(InstrumentTradeStockSharpViewModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.InstrumentReceivedStockSharpNotifyReceive, req, waitResponse: false, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> OrderReceived(OrderStockSharpModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.OrderReceivedStockSharpNotifyReceive, req, waitResponse: false, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> OwnTradeReceived(MyTradeStockSharpModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.OwnTradeReceivedStockSharpNotifyReceive, req, waitResponse: false, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> PositionReceived(PositionStockSharpModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.PositionReceivedStockSharpNotifyReceive, req, waitResponse: false, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> PortfolioReceived(PortfolioStockSharpViewModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.PortfolioReceivedStockSharpNotifyReceive, req, waitResponse: false, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ValuesChangedEvent(ConnectorValuesChangedEventPayloadModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ValuesChangedStockSharpNotifyReceive, req, waitResponse: false, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TelegramBotStarting(UserTelegramBaseModel bot, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.TelegramBotStartingStockSharpNotifyReceive, bot, waitResponse: false, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateConnectionHandle(UpdateConnectionHandleModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.UpdateConnectionStockSharpNotifyReceive, req, token: cancellationToken) ?? new();
}
////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading.Tasks;
using System.Threading;
using SharedLib;
using System.IO;

namespace RemoteCallLib;

/// <summary>
/// EventsWebChatsNotifiesTransmissionMQTT
/// </summary>
/// <param name="mqClient"></param>
public partial class EventsWebChatsNotifiesTransmissionMQTT(IMQTTClient mqClient) : IEventsWebChatsNotifies
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> NewMessageWebChatHandle(NewMessageWebChatEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.NewMessageWebChatHandleNotifyReceive, req.DialogId.ToString()), req, waitResponse: false, token: cancellationToken) ?? new();

}

/// <summary>
/// StockSharpEventsServiceTransmission
/// </summary>
public partial class EventsStockSharpTransmissionMQTT(IMQTTClient mqClient, IEventsNotify notifyTgRepo) : IEventsStockSharp
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> BoardReceived(BoardStockSharpModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.BoardReceivedStockSharpNotifyReceive, req, waitResponse: false, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> InstrumentReceived(InstrumentTradeStockSharpViewModel req, CancellationToken cancellationToken = default)
    {
        await mqClient.MqRemoteCallAsync<ResponseBaseModel>($"{GlobalStaticConstantsTransmission.TransmissionQueues.InstrumentReceivedStockSharpNotifyReceive}:{req.Id}", req, waitResponse: false, token: cancellationToken);
        return await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.InstrumentReceivedStockSharpNotifyReceive, req, waitResponse: false, token: cancellationToken) ?? new();
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> OrderReceived(OrderStockSharpModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.OrderReceivedStockSharpNotifyReceive, req, waitResponse: false, token: cancellationToken) ?? new();

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
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.UpdateConnectionStockSharpNotifyReceive, req, waitResponse: false, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ToastClientShow(ToastShowClientModel req, CancellationToken cancellationToken = default)
    {
        await Task.WhenAll([
                Task.Run(async ()=> { await notifyTgRepo.ToastClientShow(req); }),
                Task.Run(async ()=> { await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ToastClientShowStockSharpNotifyReceive, req, waitResponse: false, token: cancellationToken); }),
            ]);
        return ResponseBaseModel.CreateInfo("Ok");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DashboardTradeUpdate(DashboardTradeStockSharpModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>($"{GlobalStaticConstantsTransmission.TransmissionQueues.DashboardTradeUpdateStockSharpNotifyReceive}:{req.Id}", req, waitResponse: false, token: cancellationToken) ?? new();
}
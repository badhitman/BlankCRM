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
#pragma warning disable CS1998 // В асинхронном методе отсутствуют операторы await, будет выполнен синхронный метод
public partial class StockSharpEventsServiceTransmission(IMQTTClient mqClient) : IStockSharpEventsService
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> BoardReceived(BoardStockSharpModel req, CancellationToken cancellationToken = default)
    {
        return ResponseBaseModel.CreateError("метод не реализован");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> InstrumentReceived(InstrumentTradeStockSharpModel req, CancellationToken cancellationToken = default)
    {
        return ResponseBaseModel.CreateError("метод не реализован");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> OrderReceived(OrderStockSharpModel req)
    {
        return ResponseBaseModel.CreateError("метод не реализован");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> PortfolioReceived(PortfolioStockSharpModel req, CancellationToken cancellationToken = default)
    {
        return ResponseBaseModel.CreateError("метод не реализован");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ValuesChangedEvent(ConnectorValuesChangedEventPayloadModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ValuesChangedStockSharpNotifyReceive, req, waitResponse: false, token: cancellationToken) ?? new();
}
#pragma warning restore CS1998 // В асинхронном методе отсутствуют операторы await, будет выполнен синхронный метод
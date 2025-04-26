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
public partial class StockSharpEventsServiceTransmission(IMQTTClient mqClient) : IStockSharpEventsService
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> InstrumentReceived(InstrumentTradeModel req)
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ValuesChanged(ConnectorValuesChangedEventPayloadModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ValuesChangedStockSharpNotifyReceive, req, waitResponse: false, token: cancellationToken) ?? new();
}
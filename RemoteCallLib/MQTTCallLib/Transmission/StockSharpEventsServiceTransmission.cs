////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteCallLib;

/// <summary>
/// StockSharpEventsServiceTransmission
/// </summary>
public partial class StockSharpEventsServiceTransmission(IMQTTClient mqClient) : IStockSharpEventsService
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ValuesChanged(ConnectorValuesChangedEventPayloadModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ValuesChangedStockSharpNotifyReceive, req, waitResponse: false, token: cancellationToken) ?? new();
}
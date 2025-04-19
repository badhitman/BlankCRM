////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading.Tasks;
using System.Threading;
using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// StockSharpDriverTransmission
/// </summary>
public partial class StockSharpDriverTransmission(IZeroMQClient rabbitClient) : IStockSharpDriverService
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> PingAsync(CancellationToken cancellationToken = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.PingStockSharpDriverReceive, token: cancellationToken) ?? new();
}
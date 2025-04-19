////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading.Tasks;
using System.Threading;
using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// StockSharpMainServiceTransmission
/// </summary>
public partial class StockSharpMainServiceTransmission(IZeroMQClient rabbitClient) : IStockSharpMainService
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> PingAsync(CancellationToken cancellationToken = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.PingStockSharpMainReceive, token: cancellationToken) ?? new();
}
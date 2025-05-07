////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading.Tasks;
using System.Threading;
using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// ManageStockSharpTransmission
/// </summary>
public partial class ManageStockSharpTransmission(IMQTTClient mqClient) : IManageStockSharpService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<FixMessageAdapterModelDB[]>> AdaptersGetAsync(int[] req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<FixMessageAdapterModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.AdaptersGetStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<FixMessageAdapterModelDB>> AdaptersSelectAsync(TPaginationRequestStandardModel<AdaptersRequestModel> req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TPaginationResponseModel<FixMessageAdapterModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.AdaptersSelectStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteAdapterAsync(FixMessageAdapterModelDB req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.DeleteAdapterStockSharpReceive, req, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FixMessageAdapterModelDB>> UpdateOrCreateAdapterAsync(FixMessageAdapterModelDB req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<FixMessageAdapterModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.UpdateOrCreateAdapterStockSharpReceive, req, token: cancellationToken) ?? new();
}
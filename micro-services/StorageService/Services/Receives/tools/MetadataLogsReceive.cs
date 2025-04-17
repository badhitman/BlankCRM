////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.storage;

/// <summary>
/// MetadataLogsReceive
/// </summary>
/// <param name="storeRepo"></param>
public class MetadataLogsReceive(ISerializeStorage storeRepo)
    : IResponseReceive<PeriodDatesTimesModel?, TResponseModel<LogsMetadataResponseModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.MetadataLogsReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<LogsMetadataResponseModel>?> ResponseHandleActionAsync(PeriodDatesTimesModel? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await storeRepo.MetadataLogsAsync(payload, token);
    }
}
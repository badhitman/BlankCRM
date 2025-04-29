////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.storage;

/// <summary>
/// GoToPageForRowReceive
/// </summary>
public class GoToPageForRowReceive(ILogsService storeRepo)
    : IMQTTReceive<TPaginationRequestStandardModel<int>, TPaginationResponseModel<NLogRecordModelDB>>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GoToPageForRowLogsReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<NLogRecordModelDB>> ResponseHandleActionAsync(TPaginationRequestStandardModel<int> payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await storeRepo.GoToPageForRowAsync(payload, token);
    }
}
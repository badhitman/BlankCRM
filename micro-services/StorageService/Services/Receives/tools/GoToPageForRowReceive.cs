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
    : IResponseReceive<TPaginationRequestStandardModel<GoToPageForRowLogsRequestModel>?, TPaginationResponseStandardModel<NLogRecordModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GoToPageForRowLogsReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<NLogRecordModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<GoToPageForRowLogsRequestModel>? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await storeRepo.GoToPageForRowLogsAsync(payload, token);
    }
}
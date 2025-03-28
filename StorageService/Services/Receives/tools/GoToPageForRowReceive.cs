﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.storage;

/// <summary>
/// GoToPageForRowReceive
/// </summary>
public class GoToPageForRowReceive(ISerializeStorage storeRepo)
    : IResponseReceive<TPaginationRequestModel<int>?, TPaginationResponseModel<NLogRecordModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.GoToPageForRowReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<NLogRecordModelDB>?> ResponseHandleActionAsync(TPaginationRequestModel<int>? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await storeRepo.GoToPageForRowAsync(payload, token);
    }
}
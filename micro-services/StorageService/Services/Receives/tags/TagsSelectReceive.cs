////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.storage;

/// <summary>
/// TagsSelectReceive
/// </summary>
public class TagsSelectReceive(IParametersStorage serializeStorageRepo) 
    : IResponseReceive<TPaginationRequestStandardModel<SelectMetadataRequestModel>?, TPaginationResponseStandardModel<TagViewModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.TagsSelectReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<TagViewModel>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectMetadataRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await serializeStorageRepo.TagsSelectAsync(req, token);
    }
}
////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.storage;

/// <summary>
/// TagsSelectReceive
/// </summary>
public class TagsSelectReceive(ILogger<TagsSelectReceive> loggerRepo, IParametersStorage serializeStorageRepo) 
    : IResponseReceive<TPaginationRequestModel<SelectMetadataRequestModel>?, TPaginationResponseModel<TagViewModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.TagsSelectReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<TagViewModel>?> ResponseHandleActionAsync(TPaginationRequestModel<SelectMetadataRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        loggerRepo.LogDebug($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req)}");
        return await serializeStorageRepo.TagsSelectAsync(req, token);
    }
}
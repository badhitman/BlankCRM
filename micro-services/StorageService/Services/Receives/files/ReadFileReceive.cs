////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.storage;

/// <summary>
/// Read file
/// </summary>
public class ReadFileReceive(ILogger<ReadFileReceive> LoggerRepo, IFilesStorage serializeStorageRepo)
    : IResponseReceive<TAuthRequestModel<RequestFileReadModel>?, TResponseModel<FileContentModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ReadFileReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<FileContentModel>?> ResponseHandleActionAsync(TAuthRequestModel<RequestFileReadModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        LoggerRepo.LogDebug($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
        return await serializeStorageRepo.ReadFileAsync(req, token);
    }
}
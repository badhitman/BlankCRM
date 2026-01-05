////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.storage;

/// <summary>
/// Получить сводку (метаданные) по пространствам хранилища
/// </summary>
/// <remarks>
/// Общий размер и количество группируется по AppName
/// </remarks>
public class FilesAreaGetMetadataReceive(ILogger<FilesSelectReceive> loggerRepo, IFilesStorage serializeStorageRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<FilesAreaMetadataRequestModel?, TResponseModel<FilesAreaMetadataModel[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.FilesAreaGetMetadataReceive;

    /// <summary>
    /// Получить сводку (метаданные) по пространствам хранилища
    /// </summary>
    /// <remarks>
    /// Общий размер и количество группируется по AppName
    /// </remarks>
    public async Task<TResponseModel<FilesAreaMetadataModel[]>?> ResponseHandleActionAsync(FilesAreaMetadataRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(GetType().Name, req.GetType().Name, JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings));
        await indexingRepo.SaveTraceForReceiverAsync(trace, token);

        loggerRepo.LogDebug($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
        return await serializeStorageRepo.FilesAreaGetMetadataAsync(req, token);
    }
}
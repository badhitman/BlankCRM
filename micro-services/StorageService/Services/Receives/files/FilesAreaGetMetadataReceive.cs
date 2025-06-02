﻿////////////////////////////////////////////////
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
public class FilesAreaGetMetadataReceive(ILogger<FilesSelectReceive> loggerRepo, IFilesStorage serializeStorageRepo)
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
        loggerRepo.LogDebug($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req)}");
        return await serializeStorageRepo.FilesAreaGetMetadataAsync(req, token);
    }
}
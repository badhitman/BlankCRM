﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.kladr;

/// <inheritdoc/>
public class GetMetadataKladrReceive(ILogger<GetMetadataKladrReceive> loggerRepo, IKladrService kladrRepo)
    : IResponseReceive<GetMetadataKladrRequestModel?, MetadataKladrModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.GetMetadataKladrReceive;

    /// <inheritdoc/>
    public async Task<MetadataKladrModel?> ResponseHandleAction(GetMetadataKladrRequestModel? req)
    {
        ArgumentNullException.ThrowIfNull(req);
        loggerRepo.LogDebug($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req)}");
        return await kladrRepo.GetMetadataKladr(req);
    }
}
////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.kladr;

/// <inheritdoc/>
public class GetMetadataKladrReceive(IKladrService kladrRepo)
    : IResponseReceive<GetMetadataKladrRequestModel?, MetadataKladrModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetMetadataKladrReceive;

    /// <inheritdoc/>
    public async Task<MetadataKladrModel?> ResponseHandleActionAsync(GetMetadataKladrRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await kladrRepo.GetMetadataKladrAsync(req, token);
    }
}
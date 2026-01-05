////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// UpdateOrCreateDirectoryReceive
/// </summary>
public class UpdateOrCreateDirectoryReceive(IConstructorService conService, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestModel<EntryConstructedModel>?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.UpdateOrCreateDirectoryReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(TAuthRequestModel<EntryConstructedModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(GetType().Name, req.GetType().Name, JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings));
        TResponseModel<int> res = await conService.UpdateOrCreateDirectoryAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}
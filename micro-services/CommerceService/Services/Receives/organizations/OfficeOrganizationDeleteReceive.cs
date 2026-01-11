////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// OfficeOrganizationDelete
/// </summary>
public class OfficeOrganizationDeleteReceive(ICommerceService commerceRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<int, TResponseModel<OfficeOrganizationModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.OfficeOrganizationDeleteCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<OfficeOrganizationModelDB>?> ResponseHandleActionAsync(int req, CancellationToken token = default)
    {
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req);
        TResponseModel<OfficeOrganizationModelDB> res = await commerceRepo.OfficeOrganizationDeleteAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}
////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// UserOrganizationUpdateReceive
/// </summary>
public class UserOrganizationUpdateReceive(ICommerceService commerceRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<UserOrganizationModelDB>?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.OrganizationUserUpdateOrCreateCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(TAuthRequestStandardModel<UserOrganizationModelDB>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req?.Payload);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.Payload.Id.ToString());
        TResponseModel<int> res = await commerceRepo.UserOrganizationUpdateAsync(req, token);
        if (trace.TraceReceiverRecordId is null || string.IsNullOrWhiteSpace(trace.TraceReceiverRecordId))
            trace.TraceReceiverRecordId = res.Response.ToString();

        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, nameof(TResponseModel<int>)), token);
        return res;
    }
}
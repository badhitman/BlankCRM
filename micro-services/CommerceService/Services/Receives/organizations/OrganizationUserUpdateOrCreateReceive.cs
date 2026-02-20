////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// OrganizationUserUpdateOrCreate
/// </summary>
public class OrganizationUserUpdateOrCreateReceive(ICommerceService commerceRepo, IHistoryIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<UserOrganizationModelDB>?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.OrganizationUserUpdateOrCreateCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(TAuthRequestStandardModel<UserOrganizationModelDB>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req?.Payload);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.SenderActionUserId, req.Payload);
        TResponseModel<int> res = await commerceRepo.OrganizationUserUpdateOrCreateAsync(req, token);
        await indexingRepo.SaveHistoryForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}
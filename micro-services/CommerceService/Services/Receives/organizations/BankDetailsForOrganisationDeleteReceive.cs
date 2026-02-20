////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// BankDetailsForOrganizationDelete
/// </summary>
public class BankDetailsForOrganizationDeleteReceive(ICommerceService commerceRepo, IHistoryIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<BankDetailsForOrganizationDeleteRequestModel>?, TResponseModel<BankDetailsModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.BankDetailsForOrganizationDeleteCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<BankDetailsModelDB>?> ResponseHandleActionAsync(TAuthRequestStandardModel<BankDetailsForOrganizationDeleteRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.SenderActionUserId, req.Payload);
        TResponseModel<BankDetailsModelDB> res = await commerceRepo.BankDetailsForOrganizationDeleteAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}
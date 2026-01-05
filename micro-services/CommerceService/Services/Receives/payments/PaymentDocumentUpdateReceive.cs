////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// PaymentDocumentUpdateReceive
/// </summary>
public class PaymentDocumentUpdateReceive(ICommerceService commerceRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestModel<PaymentDocumentBaseModel>?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.PaymentDocumentUpdateCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(TAuthRequestModel<PaymentDocumentBaseModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(GetType().Name, req.GetType().Name, JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings));
        TResponseModel<int> res = await commerceRepo.PaymentDocumentUpdateAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}
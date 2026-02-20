////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// RubricsForNomenclaturesSet
/// </summary>
public class RubricsForNomenclaturesSetReceive(ICommerceService commerceRepo, IHistoryIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<RubricsSetModel>?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.NomenclatureRubricsSetCommerceReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TAuthRequestStandardModel<RubricsSetModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, null, req);
        ResponseBaseModel res = await commerceRepo.RubricsForNomenclaturesSetAsync(req, token);
        await indexingRepo.SaveHistoryForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}
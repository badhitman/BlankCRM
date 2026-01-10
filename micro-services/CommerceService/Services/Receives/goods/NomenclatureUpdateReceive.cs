////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// Обновление номенклатуры
/// </summary>
public class NomenclatureUpdateReceive(ICommerceService commerceRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<NomenclatureModelDB?, TResponseModel<int>?>
{
    /// <summary>
    /// Обновление номенклатуры
    /// </summary>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.NomenclatureUpdateCommerceReceive;

    /// <summary>
    /// Обновление номенклатуры
    /// </summary>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(NomenclatureModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.Id.ToString());
        TResponseModel<int> res = await commerceRepo.NomenclatureUpdateAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, nameof(TResponseModel<int>)), token);
        return res;
    }
}
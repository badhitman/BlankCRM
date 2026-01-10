////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// Обновление/создание номенклатуры
/// </summary>
public class NomenclatureUpdateOrCreateReceive(ICommerceService commerceRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<NomenclatureModelDB?, TResponseModel<int>?>
{
    /// <summary>
    /// Обновление номенклатуры
    /// </summary>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.NomenclatureUpdateOrCreateCommerceReceive;

    /// <summary>
    /// Обновление номенклатуры
    /// </summary>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(NomenclatureModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req);
        TResponseModel<int> res = await commerceRepo.NomenclatureUpdateOrCreateAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}
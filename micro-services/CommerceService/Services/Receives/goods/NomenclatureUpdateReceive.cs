////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
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

        TraceReceiverRecord trace = TraceReceiverRecord.Build(GetType().Name, req.GetType().Name, JsonConvert.SerializeObject(req));
        await indexingRepo.SaveTraceForReceiverAsync(trace, token);

        return await commerceRepo.NomenclatureUpdateAsync(req, token);
    }
}
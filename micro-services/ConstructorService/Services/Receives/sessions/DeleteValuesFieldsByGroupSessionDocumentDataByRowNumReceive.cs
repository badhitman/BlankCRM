////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Удалить набор значений сессии опроса/анкеты по номеру строки [GroupByRowNum].
/// Если индекс ниже нуля - удаляются все значения для указанной JoinForm (полная очистка таблицы или очистка всех значений всех полей стандартной формы)
/// </summary>
public class DeleteValuesFieldsByGroupSessionDocumentDataByRowNumReceive(IConstructorService conService, IFilesIndexing indexingRepo)
    : IResponseReceive<ValueFieldSessionDocumentDataBaseModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeleteValuesFieldsByGroupSessionDocumentDataByRowNumReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(ValueFieldSessionDocumentDataBaseModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(GetType().Name, req.GetType().Name, JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings));
        ResponseBaseModel res = await conService.DeleteValuesFieldsByGroupSessionDocumentDataByRowNumAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}
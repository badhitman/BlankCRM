////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Добавить новую строку в таблицу значений
/// </summary>
/// <returns>Номер п/п (начиная с 1) созданной строки</returns>
public class AddRowToTableConstructorReceive(IConstructorService conService, ITracesIndexing indexingRepo)
    : IResponseReceive<FieldSessionDocumentDataBaseModel?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.AddRowToTableConstructorReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(FieldSessionDocumentDataBaseModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, null, req);
        TResponseModel<int> res = await conService.AddRowToTableAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}
////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using DocumentFormat.OpenXml.Drawing;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Отправить опрос на проверку (от клиента)
/// </summary>
public class SetDoneSessionDocumentDataReceive(IConstructorService conService, IFilesIndexing indexingRepo)
    : IResponseReceive<string?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SetDoneSessionDocumentDataReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(string? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(GetType().Name, payload.GetType().Name, payload);
        await indexingRepo.SaveTraceForReceiverAsync(trace, token);

        return await conService.SetDoneSessionDocumentDataAsync(payload, token);
    }
}

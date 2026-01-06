////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Запросить порцию сессий (с пагинацией)
/// </summary>
public class RequestSessionsDocumentsReceive(IConstructorService conService) 
    : IResponseReceive<RequestSessionsDocumentsRequestPaginationModel?, TPaginationResponseStandardModel<SessionOfDocumentDataModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.RequestSessionsDocumentsConstructorReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<SessionOfDocumentDataModelDB>?> ResponseHandleActionAsync(RequestSessionsDocumentsRequestPaginationModel? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await conService.RequestSessionsDocumentsAsync(payload, token);
    }
}
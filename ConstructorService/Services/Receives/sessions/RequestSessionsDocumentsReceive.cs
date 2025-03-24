////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Запросить порцию сессий (с пагинацией)
/// </summary>
public class RequestSessionsDocumentsReceive(IConstructorService conService) : IResponseReceive<RequestSessionsDocumentsRequestPaginationModel?, TPaginationResponseModel<SessionOfDocumentDataModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.RequestSessionsDocumentsReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<SessionOfDocumentDataModelDB>?> ResponseHandleActionAsync(RequestSessionsDocumentsRequestPaginationModel? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await conService.RequestSessionsDocuments(payload, token);
    }
}
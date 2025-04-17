////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Запрос схем документов
/// </summary>
public class RequestDocumentsSchemesReceive(IConstructorService conService) : IResponseReceive<RequestDocumentsSchemesModel?, TPaginationResponseModel<DocumentSchemeConstructorModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.RequestDocumentsSchemesReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<DocumentSchemeConstructorModelDB>?> ResponseHandleActionAsync(RequestDocumentsSchemesModel? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await conService.RequestDocumentsSchemesAsync(payload, token);
    }
}
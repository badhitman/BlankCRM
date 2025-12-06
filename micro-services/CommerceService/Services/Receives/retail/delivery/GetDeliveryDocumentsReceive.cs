////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// GetDeliveryDocuments
/// </summary>
public class GetDeliveryDocumentsReceive(IRetailService commRepo)
    : IResponseReceive<GetDeliveryDocumentsRetailRequestModel?, TResponseModel<DeliveryDocumentRetailModelDB[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetDeliveryDocumentsRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<DeliveryDocumentRetailModelDB[]>?> ResponseHandleActionAsync(GetDeliveryDocumentsRetailRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.GetDeliveryDocumentsAsync(req, token);
    }
}
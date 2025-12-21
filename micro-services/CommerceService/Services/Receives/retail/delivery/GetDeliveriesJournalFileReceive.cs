////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// GetDeliveriesJournalFile
/// </summary>
public class GetDeliveriesJournalFileReceive(IRetailService commRepo)
    : IResponseReceive<SelectDeliveryDocumentsRetailRequestModel?, FileAttachModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetDeliveriesJournalFileRetailReceive;

    /// <inheritdoc/>
    public async Task<FileAttachModel?> ResponseHandleActionAsync(SelectDeliveryDocumentsRetailRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.GetDeliveriesJournalFileAsync(req, token);
    }
}
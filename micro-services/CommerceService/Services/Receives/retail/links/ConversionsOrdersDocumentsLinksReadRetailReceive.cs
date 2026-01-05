////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// ConversionsOrdersDocumentsLinksReadRetail
/// </summary>
public class ConversionsOrdersDocumentsLinksReadRetailReceive(IRetailService commRepo)
    : IResponseReceive<int[]?, TResponseModel<ConversionOrderRetailLinkModelDB[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ConversionsOrdersDocumentsLinksRetailReadReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<ConversionOrderRetailLinkModelDB[]>?> ResponseHandleActionAsync(int[]? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.ConversionsOrdersDocumentsLinksReadRetailAsync(req, token);
    }
}